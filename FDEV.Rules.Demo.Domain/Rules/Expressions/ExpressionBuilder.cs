using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FDEV.Rules.Demo.Domain.Rules.Base;
using FDEV.Rules.Demo.Domain.Rules.Conditions;

namespace FDEV.Rules.Demo.Domain.Rules.Expressions
{
    internal class ExpressionBuilder
    {
        public Expression BuildExpression<T>(RuleConditionOperator ruleOperator, object value, ParameterExpression parameterExpression)
        {
            var expressionType = new ExpressionType();
            var leftOperand = parameterExpression;
            var rightOperand = Expression.Constant(Convert.ChangeType(value, typeof(T)));
            var expressionTypeValue = (ExpressionType)expressionType.GetType().GetField(Enum.GetName(typeof(RuleConditionOperator), ruleOperator)).GetValue(ruleOperator);
            return CastBuildExpression(expressionTypeValue, value, leftOperand, rightOperand);
        }

        public Expression BuildExpression(Type type, RuleConditionOperator ruleOperator, object value, ParameterExpression parameterExpression)
        {
            var expressionType = new ExpressionType();
            var leftOperand = parameterExpression;
            var rightOperand = Expression.Constant(Convert.ChangeType(value, type));
            var expressionTypeValue = (ExpressionType)expressionType.GetType().GetField(Enum.GetName(typeof(RuleConditionOperator), ruleOperator)).GetValue(ruleOperator);
            return CastBuildExpression(expressionTypeValue, value, leftOperand, rightOperand);
        }

        public Expression BuildExpression<T>(string propertyName, RuleConditionOperator ruleOperator, object value, ParameterExpression parameterExpression)
        {
            var expressionType = new ExpressionType();
            var leftOperand = Expression.Property(parameterExpression, propertyName);
            var rightOperand = Expression.Constant(Convert.ChangeType(value, value.GetType()));
            var fieldInfo = expressionType.GetType().GetField(Enum.GetName(typeof(RuleConditionOperator), ruleOperator));
            var expressionTypeValue = (ExpressionType)fieldInfo.GetValue(ruleOperator);
            return CastBuildExpression(expressionTypeValue, value, leftOperand, rightOperand);
        }

        public Tuple<Expression, ParameterExpression> BuildExpression<T>(string propertyName, RuleConditionOperator ruleOperator, ParameterExpression parameterExpression, List<T> values)
        {
            var listExpression = Expression.Parameter(typeof(List<T>));
            var counterExpression = Expression.Parameter(typeof(int));
            var toExpression = Expression.Parameter(typeof(int));
            var arrayExpression = Expression.Parameter(typeof(T[]));
            var valueExpression = Expression.Parameter(typeof(T));
            var checkExpression = Expression.Parameter(typeof(T));
            var returnExpression = Expression.Parameter(typeof(bool));
            var MemberExpression = Expression.Property(parameterExpression, propertyName);
            var expression = MemberExpression.Expression;
            var type = MemberExpression.Type;
            var propertyExpression = Expression.Parameter(type);
            var localPropertyExpression = Expression.Parameter(type);
            var breakLabel = Expression.Label();
            var result = typeof(List<T>).GetProperty("Count");
            var toArray = typeof(List<T>).GetMethod("ToArray");
            var toArrayName = toArray.Name;
            var getGetMethod = result.GetGetMethod();
            var constantExpression = Expression.Constant(true);
            if (ruleOperator == RuleConditionOperator.NotFoundIn)
            {
                constantExpression = Expression.Constant(false);
            }
            var loop = Expression.Block(new ParameterExpression[] { toExpression, arrayExpression, valueExpression, counterExpression, returnExpression, propertyExpression, localPropertyExpression, listExpression },
                       Expression.Assign(listExpression, Expression.Constant(values)), Expression.Assign(toExpression, Expression.Call(listExpression, getGetMethod)),
                       Expression.Assign(arrayExpression, Expression.Call(listExpression, toArray)), Expression.Assign(propertyExpression, Expression.Property(checkExpression, propertyName)),
                       Expression.Loop(Expression.IfThenElse(Expression.LessThan(counterExpression, toExpression), Expression.Block(Expression.Assign(valueExpression, Expression.ArrayAccess(arrayExpression, counterExpression)),
                       Expression.Assign(localPropertyExpression, Expression.Property(valueExpression, propertyName)), Expression.IfThen(Expression.Equal(propertyExpression, localPropertyExpression), Expression.Block(Expression.Assign(returnExpression, constantExpression), Expression.Break(breakLabel))), Expression.Assign(Expression.ArrayAccess(arrayExpression, counterExpression), checkExpression),
                       Expression.PostIncrementAssign(counterExpression)), Expression.Break(breakLabel)), breakLabel),Expression.And(returnExpression, constantExpression));

            return new Tuple<Expression, ParameterExpression>(Expression.Block(loop), checkExpression);
        }

        private Expression CastBuildExpression(ExpressionType expressionTypeValue, object value, Expression leftOperand, ConstantExpression rightOperand)
        {
            if (leftOperand.Type == rightOperand.Type)
            {
                return Expression.MakeBinary(expressionTypeValue, leftOperand, rightOperand);
            }
            else if (CanChangeType(value, leftOperand.Type))
            {
                if (rightOperand.Type != typeof(bool))
                {
                    rightOperand = Expression.Constant(Convert.ChangeType(value, leftOperand.Type));
                }
                else
                {
                    leftOperand = Expression.Constant(Convert.ChangeType(value, rightOperand.Type));
                }
                return Expression.MakeBinary(expressionTypeValue, leftOperand, rightOperand);
            }
            return null;
        }

        private bool CanChangeType(object sourceType, Type targetType)
        {
            try
            {
                Convert.ChangeType(sourceType, targetType);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
