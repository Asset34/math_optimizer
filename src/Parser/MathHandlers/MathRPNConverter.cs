﻿using System;
using System.Collections.Generic;
using System.Linq;

using MathOptimizer.Parser.Interfaces;
using MathOptimizer.Parser.Interfaces.Tokens;
using MathOptimizer.Parser.Interfaces.Predicates;
using MathOptimizer.Parser.MathHandlers.TokenPredicates;

namespace MathOptimizer.Parser.MathHandlers
{
    //
    // Summary:
    //     Represents a part of the Parser which implement convertation
    //     of the input list of tokens to RPN(Reverse Polish notation)
    class MathRPNConverter : EmptyTokenVisitor
    {
        public static List<IToken> Convert(List<IToken> tokens)
        {
            // Reset handler
            mathRPNConverter.Reset();

            // Handle tokens
            foreach (IToken t in tokens)
            {
                t.Accept(mathRPNConverter);
            }

            // Pop the rest of the operators in stack
            mathRPNConverter.PopRemainOperators();

            return mathRPNConverter.resultTokens;
        }

        public override void Visit(INumberToken t)
        {
            resultTokens.Add(t);
        }
        public override void Visit(IVariableToken t)
        {
            resultTokens.Add(t);
        }
        public override void Visit(IFunctionNameToken t)
        {
            operators.Push(t);
        }
        public override void Visit(ILBracketToken t)
        {
            operators.Push(t);
        }
        public override void Visit(IRBracketToken t)
        {
            // Get operators of subexpression
            List<IToken> subExpOperators = Utills.MoveUntil(operators, lbracketPr);

            // Add operators to the result(RPN) list
            resultTokens.AddRange(subExpOperators);

            // Remove left bracket
            operators.Pop();

            // Function check
            if (operators.Count !=0 && Utills.Check(operators, functionNamePr))
            {
                resultTokens.Add(operators.Pop());
            }
        }
        public override void Visit(IBinaryOpToken t)
        {
            // Get operators with lower or equal priority
            List<IToken> lowOperators = Utills.MoveWhile(t, operators, comparePriorityPr);

            // Add operators to the result(RPN) list
            resultTokens.AddRange(lowOperators);

            operators.Push(t);
        }
        public override void Visit(IUnaryOpToken t)
        {
            operators.Push(t);
        }

        private void Reset()
        {
            operators.Clear();
            resultTokens.Clear();
        }
        private void PopRemainOperators()
        {
            resultTokens.AddRange(operators.ToList());
        }

        /* Local predicate classes */
        private class ComparePriorityTokenPredicate : EmptyTokenVisitor, ITokenComparePredicate
        {
            public bool Execute(IToken t1, IToken t2)
            {
                int priority1, priority2;

                // Get priority of t1
                t1.Accept(this);
                priority1 = value;

                // Get priority of t2
                t2.Accept(this);
                priority2 = value;

                // Compare
                if (priority1 <= priority2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public override void Visit(IBinaryOpToken t)
            {
                SetPriority(t);
            }
            public override void Visit(IUnaryOpToken t)
            {
                SetPriority(t);
            }
            public override void Visit(ILBracketToken t)
            {
                SetPriority(t);
            }
            public override void Visit(IRBracketToken t)
            {
                SetPriority(t);
            }

            private void SetPriority(IPriority t)
            {
                value = t.Priority;
            }

            private int value;
        }

        /* Used predicates */
        private static readonly FunctionNameTokenPredicate functionNamePr = new FunctionNameTokenPredicate();
        private static readonly LBracketrTokenPredicate lbracketPr = new LBracketrTokenPredicate();
        private static readonly ComparePriorityTokenPredicate comparePriorityPr = new ComparePriorityTokenPredicate();

        /* Handler */
        private static MathRPNConverter mathRPNConverter = new MathRPNConverter();

        private Stack<IToken> operators = new Stack<IToken>();
        private List<IToken> resultTokens = new List<IToken>();
    }
}
