using System;
using System.Text;
using System.Threading.Tasks;
using Convey.MessageBrokers.RabbitMQ;
using Elastic.Apm.Api;
using RabbitMQ.Client.Events;
using Agent = Elastic.Apm.Agent;

namespace Solari.Miranda.Tracer
{
    public class ElasticPlugin : RabbitMqPlugin
    {
        public override async Task HandleAsync(object message, object correlationContext, BasicDeliverEventArgs args)
        {
            bool createdTransaction = false;
            string messageName = message.GetType().Name;
            string messageId = args.BasicProperties.MessageId;
            var spanContext = string.Empty;
            if (args.BasicProperties.Headers.TryGetValue("span_context", out object spanContextHeader)
             && spanContextHeader is byte[] spanContextBytes)
            {
                spanContext = Encoding.UTF8.GetString(spanContextBytes);
            }

            ITransaction transaction = Agent.Tracer.CurrentTransaction;

            if (transaction == null)
            {
                CreateTransaction(messageName, spanContext, messageId);
                createdTransaction = true;
            }
            else
            {
                CreateSpan(Agent.Tracer.CurrentTransaction, messageName, messageId);
            }

            try
            {
                await Next(message, correlationContext, args);
            }
            catch (Exception e)
            {
                Agent.Tracer.CurrentSpan.Labels["error.message"] = e.Message;
            }
            finally
            {
                if (createdTransaction)
                {
                    Agent.Tracer.CurrentTransaction.End();
                }
                else
                {
                    Agent.Tracer.CurrentSpan.End();
                }
            }
        }

        private static void CreateTransaction(string messageName, string spanContext, string messageId)
        {
            ITransaction transaction = Agent.Tracer.StartTransaction($"processing-{messageName}",
                                                                     "RabbitMq",
                                                                     DistributedTracingData.TryDeserializeFromString(spanContext));
            transaction.Labels["message.type"] = messageName;
            transaction.Labels["message.id"] = messageId;
        }

        private static void CreateSpan(ITransaction transaction, string messageName, string messageId)
        {
            ISpan span = transaction.StartSpan($"processing-{messageName}", "RabbitMq");
            span.Labels["message.type"] = messageName;
            span.Labels["message.id"] = messageId;
        }
    }
}