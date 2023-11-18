using System;
using System.Linq.Expressions;

namespace Rehawk.UIFramework
{
    public static class MemberPath
    {
        public static string Get<T>(Expression<Func<T>> expression)
        {
            string path = string.Empty;

            Expression nextExpression = expression.Body;

            while (nextExpression != null)
            {
                if (nextExpression is MemberExpression memberExpression)
                {
                    path = memberExpression.Member.Name + (path != string.Empty ? "." : "") + path;
                    nextExpression = memberExpression.Expression;
                }
                else if (nextExpression is UnaryExpression unaryExpression)
                {
                    switch (unaryExpression.NodeType)
                    {
                        case ExpressionType.ArrayLength:
                            
                            path = "Length" + (path != string.Empty ? "." : "") + path;
                            
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    
                    nextExpression = unaryExpression.Operand;
                }
                else
                {
                    nextExpression = null;
                }
            }
            
            return path;           
        }
    }
}