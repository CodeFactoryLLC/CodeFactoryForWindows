//*****************************************************************************
//* CodeFactory SDK
//* Copyright (c) 2020-2023 CodeFactory, LLC
//*****************************************************************************
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Extension management class that manages  models that implement <see cref="CsContainer"/>.
    /// </summary>
    public static class CsContainerExtensions
    {
        /// <summary>
        /// Loads all members from a target model that implements <see cref="CsContainer"/> and returns all members and the comparison hash code for each member.
        /// </summary>
        /// <param name="source">The target container to load members from.</param>
        /// <param name="comparisonType">The type of hash code to build for comparision. Default comparison type is set to the base comparison. </param>
        /// <param name="mappedNamespaces">Optional parameter that provides namespaces to be mapped to.</param>
        /// <returns>List of all the hash codes and the members for each hashcode.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the source container is null.</exception>
        public static IReadOnlyList<KeyValuePair<int, CsMember>> GetComparisonMembers(this CsContainer source, MemberComparisonType comparisonType = MemberComparisonType.Base,
            List<MapNamespace> mappedNamespaces = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (!source.Members.Any()) return ImmutableList<KeyValuePair<int, CsMember>>.Empty;

            List<KeyValuePair<int, CsMember>> result = new List<KeyValuePair<int, CsMember>>();

            var members = source.Members.Select(m =>
                new KeyValuePair<int, CsMember>(m.GetMemberComparisonHashCode(comparisonType), m));

            result.AddRange(members);

            switch (source.ContainerType)
            {
                case CsContainerType.Interface:

                    var interfaceContainer = source as ICsInterface;

                    if (interfaceContainer?.InheritedInterfaces != null)
                    {
                        foreach (var inheritedInterface in interfaceContainer.InheritedInterfaces)
                        {
                            var interfaceMembers = inheritedInterface.GetComparisonMembers(comparisonType);
                            if (interfaceMembers.Any()) result.AddRange(interfaceMembers);
                        }
                    }

                    break;
                case CsContainerType.Class:

                    var classContainer = source as ICsClass;

                    if (classContainer?.BaseClass != null)
                    {
                        var baseMembers = classContainer.BaseClass.GetComparisonMembers(comparisonType);

                        if (baseMembers.Any()) result.AddRange(baseMembers);
                    }

                    break;

            }

            return result.ToImmutableArray();
        }

        /// <summary>
        /// Creates a list of the interface members that are not implemented in the <see cref="CsClass"/> model.
        /// </summary>
        /// <param name="source">The source model to check.</param>
        /// <param name="mappedNamespaces">Optional parameter that provides namespaces to be mapped to.</param>
        /// <returns>List of models that are missing or an empty list if there are no missing members.</returns>
        /// <exception cref="ArgumentNullException">Throws an argument null exception if the model does not exist.</exception>
        public static IReadOnlyList<CsMember> GetMissingInterfaceMembers(this CsClass source, List<MapNamespace> mappedNamespaces = null)
        {
            return GetMissingContainerInterfaceMembers(source);
        }

        /// <summary>
        /// Creates a list of the interface members that are not implemented in the <see cref="CsStructure"/> model.
        /// </summary>
        /// <param name="source">The source model to check.</param>
        /// <param name="mappedNamespaces">Optional parameter that provides namespaces to be mapped to.</param>
        /// <returns>List of models that are missing or an empty list if there are no missing members.</returns>
        /// <exception cref="ArgumentNullException">Throws an argument null exception if the model does not exist.</exception>
        public static IReadOnlyList<CsMember> GetMissingInterfaceMembers(this CsStructure source, List<MapNamespace> mappedNamespaces = null)
        {
            return GetMissingContainerInterfaceMembers(source);
        }

        /// <summary>
        /// Creates a list of the interface members that are not implemented in the <see cref="ICsContainer"/> model.
        /// </summary>
        /// <param name="source">The source model to check.</param>
        /// <param name="mappedNamespaces">Optional parameter that provides namespaces to be mapped to.</param>
        /// <returns>List of models that are missing or an empty list if there are no missing members.</returns>
        /// <exception cref="ArgumentNullException">Throws an argument null exception if the model does not exist.</exception>
        private static IReadOnlyList<CsMember> GetMissingContainerInterfaceMembers(CsContainer source, List<MapNamespace> mappedNamespaces = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.ContainerType == CsContainerType.Interface) return ImmutableList<CsMember>.Empty;
            if (source.InheritedInterfaces == null) return ImmutableList<CsMember>.Empty;

            var sourceMembers = source.GetComparisonMembers(MemberComparisonType.Security);

            var interfaceMembers = new Dictionary<int, CsMember>();

            foreach (var inheritedInterface in source.InheritedInterfaces)
            {
                var compareMembers = inheritedInterface.GetComparisonMembers(MemberComparisonType.Security);
                if (!compareMembers.Any()) continue;

                foreach (var compareMember in compareMembers)
                {
                    if (!interfaceMembers.ContainsKey(compareMember.Key))
                        interfaceMembers.Add(compareMember.Key, compareMember.Value);
                }
            }

            if (!interfaceMembers.Any()) return ImmutableList<CsMember>.Empty;

            return (from interfaceMember in interfaceMembers
                        // ReSharper disable once SimplifyLinqExpression
                    where !sourceMembers.Any(m => m.Key == interfaceMember.Key)
                    select interfaceMember.Value).ToImmutableList();
        }
    }
}