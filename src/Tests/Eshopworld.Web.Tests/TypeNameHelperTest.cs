﻿using System;
using System.Collections;
using System.Collections.Generic;
using Eshopworld.Tests.Core;
using Xunit;

namespace Eshopworld.Web.Tests
{
    /// <summary>
    /// TypeNameHelperTest has been moved from 'https://github.com/dotnet/extensions/blob/master/src/Shared/test/TypeNameHelperTest.cs'.
    /// </summary>
    /// <remarks>
    /// It has been decided to copy this file for backward compatibility. See the remarks for <see cref="TypeNameHelper"/>. Namespaces were changed.
    /// </remarks>
    public class TypeNameHelperTest
    {
        [Theory, IsLayer0]
        // Predefined Types
        [InlineData(typeof(int), "int")]
        [InlineData(typeof(List<int>), "System.Collections.Generic.List<int>")]
        [InlineData(typeof(Dictionary<int, string>), "System.Collections.Generic.Dictionary<int, string>")]
        [InlineData(typeof(Dictionary<int, List<string>>), "System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<string>>")]
        [InlineData(typeof(List<List<string>>), "System.Collections.Generic.List<System.Collections.Generic.List<string>>")]
        // Classes inside NonGeneric class
        [InlineData(typeof(A),
            "Eshopworld.Web.Tests.TypeNameHelperTest+A")]
        [InlineData(typeof(B<int>),
            "Eshopworld.Web.Tests.TypeNameHelperTest+B<int>")]
        [InlineData(typeof(C<int, string>),
            "Eshopworld.Web.Tests.TypeNameHelperTest+C<int, string>")]
        [InlineData(typeof(B<B<string>>),
            "Eshopworld.Web.Tests.TypeNameHelperTest+B<Eshopworld.Web.Tests.TypeNameHelperTest+B<string>>")]
        [InlineData(typeof(C<int, B<string>>),
            "Eshopworld.Web.Tests.TypeNameHelperTest+C<int, Eshopworld.Web.Tests.TypeNameHelperTest+B<string>>")]
        // Classes inside Generic class
        [InlineData(typeof(Outer<int>.D),
            "Eshopworld.Web.Tests.TypeNameHelperTest+Outer<int>+D")]
        [InlineData(typeof(Outer<int>.E<int>),
            "Eshopworld.Web.Tests.TypeNameHelperTest+Outer<int>+E<int>")]
        [InlineData(typeof(Outer<int>.F<int, string>),
            "Eshopworld.Web.Tests.TypeNameHelperTest+Outer<int>+F<int, string>")]
        [InlineData(typeof(Level1<int>.Level2<bool>.Level3<int>),
            "Eshopworld.Web.Tests.TypeNameHelperTest+Level1<int>+Level2<bool>+Level3<int>")]
        [InlineData(typeof(Outer<int>.E<Outer<int>.E<string>>),
            "Eshopworld.Web.Tests.TypeNameHelperTest+Outer<int>+E<Eshopworld.Web.Tests.TypeNameHelperTest+Outer<int>+E<string>>")]
        [InlineData(typeof(Outer<int>.F<int, Outer<int>.E<string>>),
            "Eshopworld.Web.Tests.TypeNameHelperTest+Outer<int>+F<int, Eshopworld.Web.Tests.TypeNameHelperTest+Outer<int>+E<string>>")]
        [InlineData(typeof(OuterGeneric<int>.InnerNonGeneric.InnerGeneric<int, string>.InnerGenericLeafNode<bool>),
            "Eshopworld.Web.Tests.TypeNameHelperTest+OuterGeneric<int>+InnerNonGeneric+InnerGeneric<int, string>+InnerGenericLeafNode<bool>")]
        public void Can_pretty_print_CLR_full_name(Type type, string expected)
        {
            Assert.Equal(expected, TypeNameHelper.GetTypeDisplayName(type));
        }

        [Fact, IsLayer0]
        public void DoesNotPrintNamespace_ForGenericTypes_IfNullOrEmpty()
        {
            // Arrange
            var type = typeof(ClassInGlobalNamespace<int>);

            // Act & Assert
            Assert.Equal("ClassInGlobalNamespace<int>", TypeNameHelper.GetTypeDisplayName(type));
        }

        [Theory, IsLayer0]
        // Predefined Types
        [InlineData(typeof(int), "int")]
        [InlineData(typeof(List<int>), "List<int>")]
        [InlineData(typeof(Dictionary<int, string>), "Dictionary<int, string>")]
        [InlineData(typeof(Dictionary<int, List<string>>), "Dictionary<int, List<string>>")]
        [InlineData(typeof(List<List<string>>), "List<List<string>>")]
        // Classes inside NonGeneric class
        [InlineData(typeof(A), "A")]
        [InlineData(typeof(B<int>), "B<int>")]
        [InlineData(typeof(C<int, string>), "C<int, string>")]
        [InlineData(typeof(C<int, B<string>>), "C<int, B<string>>")]
        [InlineData(typeof(B<B<string>>), "B<B<string>>")]
        // Classes inside Generic class
        [InlineData(typeof(Outer<int>.D), "D")]
        [InlineData(typeof(Outer<int>.E<int>), "E<int>")]
        [InlineData(typeof(Outer<int>.F<int, string>), "F<int, string>")]
        [InlineData(typeof(Outer<int>.F<int, Outer<int>.E<string>>), "F<int, E<string>>")]
        [InlineData(typeof(Outer<int>.E<Outer<int>.E<string>>), "E<E<string>>")]
        [InlineData(typeof(OuterGeneric<int>.InnerNonGeneric.InnerGeneric<int, string>.InnerGenericLeafNode<bool>), "InnerGenericLeafNode<bool>")]
        public void Can_pretty_print_CLR_name(Type type, string expected)
        {
            Assert.Equal(expected, TypeNameHelper.GetTypeDisplayName(type, false));
        }

        [Theory, IsLayer0]
        [InlineData(typeof(void), "void")]
        [InlineData(typeof(bool), "bool")]
        [InlineData(typeof(byte), "byte")]
        [InlineData(typeof(char), "char")]
        [InlineData(typeof(decimal), "decimal")]
        [InlineData(typeof(double), "double")]
        [InlineData(typeof(float), "float")]
        [InlineData(typeof(int), "int")]
        [InlineData(typeof(long), "long")]
        [InlineData(typeof(object), "object")]
        [InlineData(typeof(sbyte), "sbyte")]
        [InlineData(typeof(short), "short")]
        [InlineData(typeof(string), "string")]
        [InlineData(typeof(uint), "uint")]
        [InlineData(typeof(ulong), "ulong")]
        [InlineData(typeof(ushort), "ushort")]
        public void Returns_common_name_for_built_in_types(Type type, string expected)
        {
            Assert.Equal(expected, TypeNameHelper.GetTypeDisplayName(type));
        }

        [Theory, IsLayer0]
        [InlineData(typeof(int[]), true, "int[]")]
        [InlineData(typeof(string[][]), true, "string[][]")]
        [InlineData(typeof(int[,]), true, "int[,]")]
        [InlineData(typeof(bool[,,,]), true, "bool[,,,]")]
        [InlineData(typeof(A[,][,,]), true, "Eshopworld.Web.Tests.TypeNameHelperTest+A[,][,,]")]
        [InlineData(typeof(List<int[,][,,]>), true, "System.Collections.Generic.List<int[,][,,]>")]
        [InlineData(typeof(List<int[,,][,]>[,][,,]), false, "List<int[,,][,]>[,][,,]")]
        public void Can_pretty_print_array_name(Type type, bool fullName, string expected)
        {
            Assert.Equal(expected, TypeNameHelper.GetTypeDisplayName(type, fullName));
        }

        public static TheoryData GetOpenGenericsTestData()
        {
            return new TheoryData<Type, bool, string>
            {
                { typeof(List<>), false, "List<>" },
                { typeof(Dictionary<,>), false , "Dictionary<,>" },
                { typeof(List<>), true , "System.Collections.Generic.List<>" },
                { typeof(Dictionary<,>), true , "System.Collections.Generic.Dictionary<,>" },
                { typeof(Level1<>.Level2<>.Level3<>), true, "Eshopworld.Web.Tests.TypeNameHelperTest+Level1<>+Level2<>+Level3<>" },
            };
        }

        [Theory, IsLayer0]
        [MemberData(nameof(GetOpenGenericsTestData))]
        public void Can_pretty_print_open_generics(Type type, bool fullName, string expected)
        {
            Assert.Equal(expected, TypeNameHelper.GetTypeDisplayName(type, fullName));
        }

        [Fact, IsLayer0]
        public void Can_pretty_print_PartiallyClosedGeneric()
        {
            var type = typeof(PartiallyClosedGeneric<>).BaseType;

            string expected = "Eshopworld.Web.Tests.TypeNameHelperTest+C<, int>";
            Assert.Equal(expected, TypeNameHelper.GetTypeDisplayName(type, true));
        }

        [Fact, IsLayer0]
        public void Can_pretty_print_NestedGenerics()
        {
            var type = typeof(OuterGeneric<>.InnerNonGeneric.InnerGeneric<,>.InnerGenericLeafNode<>);

            string expected = "Eshopworld.Web.Tests.TypeNameHelperTest+OuterGeneric<>+InnerNonGeneric+InnerGeneric<,>+InnerGenericLeafNode<>";
            Assert.Equal(expected, TypeNameHelper.GetTypeDisplayName(type, true));
        }

        [Fact, IsLayer0]
        public void Can_pretty_print_ClosedDictionaryType()
        {
            var openDictionaryType = typeof(Dictionary<,>);
            var genArgsDictionary = openDictionaryType.GetGenericArguments();
            genArgsDictionary[0] = typeof(B<>);
            var closedDictionaryType = openDictionaryType.MakeGenericType(genArgsDictionary);

            string expected = "System.Collections.Generic.Dictionary<Eshopworld.Web.Tests.TypeNameHelperTest+B<>,>";
            Assert.Equal(expected, TypeNameHelper.GetTypeDisplayName(closedDictionaryType, true));
        }

        [Fact, IsLayer0]
        public void Can_pretty_print_ClosedLevelType()
        {
            var openLevelType = typeof(Level1<>.Level2<>.Level3<>);
            var genArgsLevel = openLevelType.GetGenericArguments();
            genArgsLevel[1] = typeof(string);
            var closedLevelType = openLevelType.MakeGenericType(genArgsLevel);

            string expected = "Eshopworld.Web.Tests.TypeNameHelperTest+Level1<>+Level2<string>+Level3<>";
            Assert.Equal(expected, TypeNameHelper.GetTypeDisplayName(closedLevelType, true));
        }

        [Fact, IsLayer0]
        public void Can_pretty_print_ClosedInnerType()
        {
            var openInnerType = typeof(OuterGeneric<>.InnerNonGeneric.InnerGeneric<,>.InnerGenericLeafNode<>);
            var genArgsInnerType = openInnerType.GetGenericArguments();
            genArgsInnerType[3] = typeof(bool);
            var closedInnerType = openInnerType.MakeGenericType(genArgsInnerType);

            string expected = "Eshopworld.Web.Tests.TypeNameHelperTest+OuterGeneric<>+InnerNonGeneric+InnerGeneric<,>+InnerGenericLeafNode<bool>";
            Assert.Equal(expected, TypeNameHelper.GetTypeDisplayName(closedInnerType, true));
        }

        public static TheoryData GetTypeDisplayName_IncludesGenericParameterNamesWhenOptionIsSetData =>
            new TheoryData<Type, string>
            {
                {  typeof(B<>),"Eshopworld.Web.Tests.TypeNameHelperTest+B<T>" },
                {  typeof(C<,>),"Eshopworld.Web.Tests.TypeNameHelperTest+C<T1, T2>" },
                {  typeof(Level1<>.Level2<>),"Eshopworld.Web.Tests.TypeNameHelperTest+Level1<T1>+Level2<T2>" },
            };

        [Theory, IsLayer0]
        [MemberData(nameof(GetTypeDisplayName_IncludesGenericParameterNamesWhenOptionIsSetData))]
        public void GetTypeDisplayName_IncludesGenericParameterNamesWhenOptionIsSet(Type type, string expected)
        {
            // Arrange & Act
            var actual = TypeNameHelper.GetTypeDisplayName(type, fullName: true, includeGenericParameterNames: true);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact, IsLayer0]
        public void GetTypeDisplayName_IncludesGenericParameterNamesWhenOptionIsSet_PartiallyClosedGeneric()
        {
            var type = typeof(PartiallyClosedGeneric<>).BaseType;
            var actual = TypeNameHelper.GetTypeDisplayName(type, fullName: true, includeGenericParameterNames: true);

            Assert.Equal("Eshopworld.Web.Tests.TypeNameHelperTest+C<T, int>", actual);
        }

        public static TheoryData GetTypeDisplayName_WithoutFullName_IncludesGenericParameterNamesWhenOptionIsSetData =>
            new TheoryData<Type, string>
            {
                {  typeof(B<>),"B<T>" },
                {  typeof(C<,>),"C<T1, T2>" },
                {  typeof(Level1<>.Level2<>),"Level2<T2>" },
            };

        [Theory, IsLayer0]
        [MemberData(nameof(GetTypeDisplayName_WithoutFullName_IncludesGenericParameterNamesWhenOptionIsSetData))]
        public void GetTypeDisplayName_WithoutFullName_IncludesGenericParameterNamesWhenOptionIsSet(Type type, string expected)
        {
            // Arrange & Act
            var actual = TypeNameHelper.GetTypeDisplayName(type, fullName: false, includeGenericParameterNames: true);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact, IsLayer0]
        public void GetTypeDisplayName_WithoutFullName_IncludesGenericParameterNamesWhenOptionIsSet_PartiallyClosedGeneric()
        {
            var type = typeof(PartiallyClosedGeneric<>).BaseType;
            var actual = TypeNameHelper.GetTypeDisplayName(type, fullName: false, includeGenericParameterNames: true);

            Assert.Equal("C<T, int>", actual);
        }

        public static TheoryData<Type, string> FullTypeNameData
        {
            get
            {
                return new TheoryData<Type, string>
                {
                    // Predefined Types
                    { typeof(int), "int" },
                    { typeof(List<int>), "System.Collections.Generic.List" },
                    { typeof(Dictionary<int, string>), "System.Collections.Generic.Dictionary" },
                    { typeof(Dictionary<int, List<string>>), "System.Collections.Generic.Dictionary" },
                    { typeof(List<List<string>>), "System.Collections.Generic.List" },

                    // Classes inside NonGeneric class
                    { typeof(A), "Eshopworld.Web.Tests.TypeNameHelperTest.A" },
                    { typeof(B<int>), "Eshopworld.Web.Tests.TypeNameHelperTest.B" },
                    { typeof(C<int, string>), "Eshopworld.Web.Tests.TypeNameHelperTest.C" },
                    { typeof(C<int, B<string>>), "Eshopworld.Web.Tests.TypeNameHelperTest.C" },
                    { typeof(B<B<string>>), "Eshopworld.Web.Tests.TypeNameHelperTest.B" },

                    // Classes inside Generic class
                    { typeof(Outer<int>.D), "Eshopworld.Web.Tests.TypeNameHelperTest.Outer.D" },
                    { typeof(Outer<int>.E<int>), "Eshopworld.Web.Tests.TypeNameHelperTest.Outer.E" },
                    { typeof(Outer<int>.F<int, string>), "Eshopworld.Web.Tests.TypeNameHelperTest.Outer.F" },
                    { typeof(Outer<int>.F<int, Outer<int>.E<string>>),"Eshopworld.Web.Tests.TypeNameHelperTest.Outer.F" },
                    { typeof(Outer<int>.E<Outer<int>.E<string>>), "Eshopworld.Web.Tests.TypeNameHelperTest.Outer.E" }
                };
            }
        }

        [Theory, IsLayer0]
        [MemberData(nameof(FullTypeNameData))]
        public void Can_PrettyPrint_FullTypeName_WithoutGenericParametersAndNestedTypeDelimiter(Type type, string expectedTypeName)
        {
            // Arrange & Act
            var displayName = TypeNameHelper.GetTypeDisplayName(type, fullName: true, includeGenericParameters: false, nestedTypeDelimiter: '.');

            // Assert
            Assert.Equal(expectedTypeName, displayName);
        }

        private class A { }

        private class B<T> { }

        private class C<T1, T2> { }

        private class PartiallyClosedGeneric<T> : C<T, int> { }

        private class Outer<T>
        {
            public class D { }

            public class E<T1> { }

            public class F<T1, T2> { }
        }

        private class OuterGeneric<T1>
        {
            public class InnerNonGeneric
            {
                public class InnerGeneric<T2, T3>
                {
                    public class InnerGenericLeafNode<T4> { }

                    public class InnerLeafNode { }
                }
            }
        }

        private class Level1<T1>
        {
            public class Level2<T2>
            {
                public class Level3<T3>
                {
                }
            }
        }
    }
}

internal class ClassInGlobalNamespace<T>
{
}