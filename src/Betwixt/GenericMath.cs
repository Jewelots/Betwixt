using System;
using System.Linq.Expressions;

namespace Betwixt
{
    /// <summary>
    /// Generic math handlers for performing basic operations on unknown types
    /// </summary>
    /// <remarks>
    /// This is really bad but surprisingly not as slow as it could be, once they're used once for a specific
    /// type they're compiled and are fairly efficient. These functions are only used if the user does not
    /// specify their own lerp function.
    /// </remarks>
    internal static class GenericMath
    {
        /// <summary>
        /// Add two generics together
        /// </summary>
        /// <typeparam name="T">Type of elements</typeparam>
        /// <param name="a">First element</param>
        /// <param name="b">Second element</param>
        /// <returns>a + b</returns>
        public static T Add<T>(T a, T b)
        {
            ParameterExpression paramA = Expression.Parameter(typeof(T), "a");
            ParameterExpression paramB = Expression.Parameter(typeof(T), "b");

            BinaryExpression body = Expression.Add(paramA, paramB);

            Func<T, T, T> add = Expression.Lambda<Func<T, T, T>>(body, paramA, paramB).Compile();

            return add(a, b);
        }

        /// <summary>
        /// Subtract a generic from a generic
        /// </summary>
        /// <typeparam name="T">Type of elements</typeparam>
        /// <param name="a">First element</param>
        /// <param name="b">Second element</param>
        /// <returns>a - b</returns>
        public static T Subtract<T>(T a, T b)
        {
            ParameterExpression paramA = Expression.Parameter(typeof(T), "a");
            ParameterExpression paramB = Expression.Parameter(typeof(T), "b");

            BinaryExpression body = Expression.Subtract(paramA, paramB);

            Func<T, T, T> subtract = Expression.Lambda<Func<T, T, T>>(body, paramA, paramB).Compile();

            return subtract(a, b);
        }

        /// <summary>
        /// Multiply a generic with a float
        /// </summary>
        /// <typeparam name="T">Type of generic</typeparam>
        /// <param name="a">Generic element</param>
        /// <param name="b">Float element</param>
        /// <returns>a * b</returns>
        public static T Multiply<T>(T a, float b)
        {
            ParameterExpression paramA = Expression.Parameter(typeof(T), "a");
            ParameterExpression paramB = Expression.Parameter(typeof(float), "b");

            BinaryExpression body = Expression.Multiply(paramA, paramB);

            Func<T, float, T> multiply = Expression.Lambda<Func<T, float, T>>(body, paramA, paramB).Compile();

            return multiply(a, b);
        }
    }
}
