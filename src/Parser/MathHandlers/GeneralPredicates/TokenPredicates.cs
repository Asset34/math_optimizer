﻿using MathOptimizer.Parser.Interfaces.Tokens;
using MathOptimizer.Parser.Interfaces.Predicates;

namespace MathOptimizer.Parser.MathHandlers.TokenPredicates
{
    //
    // Summary:
    //     Represents a base class for the following predicates
    class FalseTokenPredicate : EmptyTokenVisitor, ITokenPredicate
    {
        public bool Execute(IToken t)
        {
            result = false;
            t.Accept(this);
            return result;
        }

        protected bool result;
    }

    class NumberTokenPredicate : FalseTokenPredicate
    {
        public override void Visit(INumberToken t)
        {
            result = true;
        }
    }
    class VariableTokenPredicate : FalseTokenPredicate
    {
        public override void Visit(IVariableToken t)
        {
            result = true;
        }
    }
    class ConstantTokenPredicate : FalseTokenPredicate
    {
        public override void Visit(IConstantToken t)
        {
            result = true;
        }
    }
    class BinaryOpTokenPredicate : FalseTokenPredicate
    {
        public override void Visit(IBinaryOpToken t)
        {
            result = true;
        }
    }
    class UnaryOpTokenPredicate : FalseTokenPredicate
    {
        public override void Visit(IUnaryOpToken t)
        {
            result = true;
        }
    }
    class FunctionNameTokenPredicate : FalseTokenPredicate
    {
        public override void Visit(IFunctionNameToken t)
        {
            result = true;
        }
    }
    class LBracketrTokenPredicate : FalseTokenPredicate
    {
        public override void Visit(ILBracketToken t)
        {
            result = true;
        }
    }
    class RBracketTokenPredicate : FalseTokenPredicate
    {
        public override void Visit(IRBracketToken t)
        {
            result = true;
        }
    }
    class FuncSeparatorPredicate : FalseTokenPredicate
    {
        public override void Visit(IFuncSeparatorToken t)
        {
            result = true;
        }
    }
}
