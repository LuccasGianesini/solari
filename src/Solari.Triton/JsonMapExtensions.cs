using System;
using System.Linq.Expressions;
using System.Reflection;
using Solari.Triton.Compat;

namespace Solari.Triton
{
    public static class JsonMapExtensions
    {
        private static MemberInfo GetMemberInfoFromExpression(LambdaExpression expr)
        {
            MemberExpression memberExpression = expr.Body as MemberExpression;
            if (memberExpression == null)
            {
                if (expr.Body is UnaryExpression unary)
                    if (unary.NodeType == ExpressionType.Convert || unary.NodeType == ExpressionType.ConvertChecked)
                        memberExpression = unary.Operand as MemberExpression;
            }

            if (memberExpression == null)
                throw new ArgumentException("Expression must be a property or field expression of the main type.", nameof(expr));

            MemberInfo memberInfo = memberExpression.Member;
            if (!(memberInfo is PropertyInfo || memberInfo is FieldInfo))
                throw new ArgumentException("Expression must be a property or field expression of the main type.", nameof(expr));

            return memberInfo;
        }

        public static void Ignore<T, TProp>(this JsonMap<T> jsonMap, Expression<Func<T, TProp>> expr)
        {
            jsonMap.Ignore((LambdaExpression)expr);
        }

        public static void Ignore<T, TProp>(this JsonSubclassMap<T> jsonMap, Expression<Func<T, TProp>> expr)
        {
            jsonMap.Ignore((LambdaExpression)expr);
        }

        public static void Ignore(this JsonMapBase jsonMap, LambdaExpression expr)
        {
            MemberInfo memberInfo = GetMemberInfoFromExpression(expr);

            jsonMap.Actions.Add(
                (member, property, mode) =>
                {
                    if (jsonMap.AcceptsMember(member, memberInfo))
                    {
                        property.ShouldSerialize = instance => false;
                    }
                });
        }

        public static void IgnoreAllByDefault(this JsonMap jsonMap)
        {
            jsonMap.Actions.Insert(
                0,
                (member, property, mode) =>
                {
                    property.ShouldSerialize = instance => false;
                });
        }

        public static void Map<T, TProp>(this JsonMap<T> jsonMap, Expression<Func<T, TProp>> expr, string name)
        {
            jsonMap.Map((LambdaExpression)expr, name);
        }

        public static void SubclassMap<TSubclass>(this IAndSubtypes jsonMap, Expression<Func<TSubclass, object>> expr, string name)
        {
            if (!jsonMap.SerializedType.GetTypeInfo().IsAssignableFrom(typeof(TSubclass).GetTypeInfo()))
                throw new Exception("The type `TSubclass` must be a subclass of the `JsonMap.SerializedType`.");
            ((JsonMapBase)jsonMap).Map((LambdaExpression)expr, name);
        }

        public static void SubclassMap<T, TSubclass>(this IAndSubtypes<T> jsonMap, Expression<Func<TSubclass, object>> expr, string name)
            where TSubclass : T
        {
            ((JsonMapBase)jsonMap).Map((LambdaExpression)expr, name);
        }

        public static void Map<T, TProp>(this JsonSubclassMap<T> jsonMap, Expression<Func<T, TProp>> expr, string name)
        {
            jsonMap.Map((LambdaExpression)expr, name);
        }

        public static void Map(this JsonMapBase jsonMap, LambdaExpression expr, string name)
        {
            MemberInfo memberInfo = GetMemberInfoFromExpression(expr);

            jsonMap.Actions.Add(
                (member, property, mode) =>
                {
                    if (TypeExtraInfo.MembersAreTheSame(member, memberInfo))
                    {
                        property.PropertyName = name;
                        property.ShouldSerialize = instance => true;
                    }
                });
        }

        public static void NamingConvention<T>(this JsonMap<T> jsonMap, Func<string, string> renamer)
        {
            jsonMap.Actions.Add((member, property, mode) =>
            {
                {
                    property.PropertyName = renamer(member.Name);
                    property.ShouldSerialize = instance => true;
                }
            });
        }
    }
}
