using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace FF12RngHelper
{
    /// <summary>
    /// An Extension class for Expressions
    /// </summary>
    public static class Extensions
    {
        #region expression extensions
        /// <summary>
        /// Compiles the expression and get the functions return value
        /// </summary>
        /// <typeparam name="T">The type of the return value</typeparam>
        /// <param name="expression"><see cref="T"/> value</param>
        /// <returns></returns>
        public static T GetPropertyValue<T>(this Expression<Func<T>> expression) => expression.Compile().Invoke();

        /// <summary>
        /// Sets the underlying properties to the given value, from our expression the contains the property
        /// </summary>
        /// <typeparam name="T">the type of value to set</typeparam>
        /// <param name="lambdaExpression">the expression</param>
        /// <param name="value">the value to set the proprty to</param>
        /// <returns></returns>
        public static void SetPropertyValue<T>(this Expression<Func<T>> lambdaExpression, T value)
        {
            var expressionBody = (lambdaExpression as LambdaExpression).Body as MemberExpression;

            var propertyInfo = (PropertyInfo)expressionBody.Member;
            var target = Expression.Lambda(expressionBody.Expression).Compile().DynamicInvoke();

            propertyInfo.SetValue(target, value);
        }
        #endregion

        #region Enum Extentions
        /// <summary>
        /// An extension method that get the <see cref="DescriptionAttribute"/> value of the specified enumeration
        /// </summary>
        /// <param name="en">The name of the enumeration</param>
        /// <returns>a <see cref="string"/> value from the <see cref="DescriptionAttribute"/> of the specified enumeration.
        /// If no description was specified, returns the string value of the enumeration</returns>
        public static string GetDiscription(this Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }
            return en.ToString();
        }

        public static T ToEnum<T>(this string enumString)
        {
            return (T)Enum.Parse(typeof(T), enumString);
        }
        #endregion
    }
}
