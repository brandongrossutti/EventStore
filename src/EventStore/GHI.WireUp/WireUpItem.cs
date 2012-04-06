using System;
using StructureMap;

namespace GHI.WireUp
{
    public class WireUpItem 
    {
        private readonly Action<ConfigurationExpression> _expression;

        public WireUpItem(Action<ConfigurationExpression> expression)
        {
            _expression = expression;
        }

        public Action<ConfigurationExpression> Expression
        {
            get { return _expression; }
        }
    }
}
    