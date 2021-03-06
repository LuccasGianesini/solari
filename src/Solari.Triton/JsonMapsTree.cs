using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Solari.Triton
{
    internal class JsonMapsTree
    {
        private readonly Dictionary<Type, JsonMapsTreeNode> dictionary;

        public JsonMapsTree(Dictionary<Type, JsonMapsTreeNode> dictionary)
        {
            this.dictionary = dictionary;
        }

        public IEnumerable<JsonMapBase> GetMappers(Type objectType)
        {
            // find mapper in the dictionary
            Type current = objectType;
            while (current != null)
            {
                int count = 0;
                JsonMapsTreeNode node;
                if (this.dictionary.TryGetValue(current, out node))
                    count++;

                List<JsonMapsTreeNode> nodes = this.dictionary
                                                   .Where(kv => kv.Value.Mapper is IAndSubtypes)
                                                   .Where(kv => kv.Value != node)
                                                   .Where(kv => kv.Value.Mapper.AcceptsType(current))
                                                   .Select(kv => kv.Value)
                                                   .ToList();

                count += nodes.Count;

                if (count > 1)
                    throw new Exception($"Ambiguous maps for the type `{current.Name}`");

                if (nodes.Count > 0)
                    node = nodes[0];

                if (node == null)
                    yield break;

                yield return node.Mapper;

                if (node.Mapper is JsonMap)
                    yield break;

                current = current.GetTypeInfo().BaseType;
            }
        }

        public IEnumerable<JsonMapBase[]> GetSubpathes(Type objectType)
        {
            JsonMapsTreeNode rootNode;
            if (!this.dictionary.TryGetValue(objectType, out rootNode) || rootNode.Children.Count == 0)
                yield break;

            Stack<JsonMapBase> list = new Stack<JsonMapBase>(20);
            Stack<Queue<JsonMapsTreeNode>> stack = new Stack<Queue<JsonMapsTreeNode>>(20);
            JsonMapsTreeNode node = rootNode;
            stack.Push(new Queue<JsonMapsTreeNode>(node.Children.Select(ct => this.dictionary[ct])));
            while (true)
            {
                Queue<JsonMapsTreeNode> currentLevel = stack.Peek();
                if (currentLevel.Count > 0)
                {
                    node = currentLevel.Dequeue();
                    stack.Push(new Queue<JsonMapsTreeNode>(node.Children.Select(ct => this.dictionary[ct])));
                    list.Push(node.Mapper);
                }
                else
                {
                    stack.Pop();
                    if (stack.Count == 0)
                        yield break;
                    if (list.Peek().CanCreateNew())
                        yield return list.ToArray();
                    list.Pop();
                }
            }
        }
    }
}
