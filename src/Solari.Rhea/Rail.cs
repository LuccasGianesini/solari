using System;
using System.Collections.Generic;
using Solari.Sol;

namespace Solari.Rhea
{
    public class Rail<T>
        where T : RailContext
    {
        private Queue<ICrosstie> Crossties { get; }
        private Action _preExecutionAction;

        public Rail<T> AddCrosstie(ICrosstie crosstie)
        {
            Check.ThrowIfNull(crosstie, nameof(ICrosstie));
            Crossties.Enqueue(crosstie);
            return this;
        }

        public Rail<T> AddPreExecutionAction(Action action)
        {
            Check.ThrowIfNull(action, nameof(Action));
            _preExecutionAction = action;
            return this;
        }
    }
}
