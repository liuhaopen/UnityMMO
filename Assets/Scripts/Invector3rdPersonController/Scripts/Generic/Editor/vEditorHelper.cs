using UnityEngine;
using System.Collections;
using System.Linq.Expressions;
using System;

public  class vEditorHelper {

    /// <summary>
    /// Get PropertyName
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="propertyLambda">You must pass a lambda of the form: '() => Class.Property' or '() => object.Property'</param>
    /// <returns></returns>
    public static string GetPropertyName<T>(Expression<Func<T>> propertyLambda)
    {
        var me = propertyLambda.Body as MemberExpression;

        if (me == null)
        {
            throw new ArgumentException("You must pass a lambda of the form: '() => Class.Property' or '() => object.Property'");
        }

        return me.Member.Name;
    }
}
