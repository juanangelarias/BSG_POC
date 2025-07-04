﻿using System.Linq.Expressions;

namespace BSG.Database.Helpers;

public static class PredicateBuilder
{
    public static Expression<Func<Thread, bool>> True<T>()
    {
        return f => true;
    }
    
    public static Expression<Func<Thread, bool>> False<T>()
    {
        return f => false;
    }

    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        var invokeExpr = Expression.Invoke(expr2, expr1.Parameters);

        return Expression.Lambda<Func<T, bool>>
            (Expression.OrElse(expr1.Body, invokeExpr), expr1.Parameters);
    }
    
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        var invokeExpr = Expression.Invoke(expr2, expr1.Parameters);

        return Expression.Lambda<Func<T, bool>>
            (Expression.AndAlso(expr1.Body, invokeExpr), expr1.Parameters);
    }
}