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

            // Return empty singlton array if there are no members to avoid unnecessary allocations
            if (source.Members.Count == 0) return Array.Empty<KeyValuePair<int, CsMember>>();

            // Use CreateBuilder to avoid intermediate List<T> allocation
            var result = ImmutableArray.CreateBuilder<KeyValuePair<int, CsMember>>(source.Members.Count);

            foreach (var m in source.Members)
                result.Add(new KeyValuePair<int, CsMember>(m.GetMemberComparisonHashCode(comparisonType), m));

            switch (source.ContainerType)
            {
                case CsContainerType.Interface:

                    var interfaceContainer = source as ICsInterface;

                    if (interfaceContainer?.InheritedInterfaces != null)
                    {
                        foreach (var inheritedInterface in interfaceContainer.InheritedInterfaces)
                        {
                            var interfaceMembers = inheritedInterface.GetComparisonMembers(comparisonType);
                            if (interfaceMembers.Count > 0) result.AddRange(interfaceMembers);
                        }
                    }

                    break;
                case CsContainerType.Class:

                    var classContainer = source as ICsClass;

                    if (classContainer?.BaseClass != null)
                    {
                        var baseMembers = classContainer.BaseClass.GetComparisonMembers(comparisonType);

                        if (baseMembers.Count > 0) result.AddRange(baseMembers);
                    }

                    break;
            }

            return result.Count > 0 ? result.ToImmutable() : Array.Empty<KeyValuePair<int, CsMember>>();
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
        /// Gets the <see cref="CsSource"/> model from the container.
        /// </summary>
        /// <param name="source">The container to retrieve the source from.</param>
        /// <returns>The <see cref="CsSource"/> model.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static CsSource GetSource(this CsContainer source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var result = source.GetModel<CsSource>(PathBuilderConstants.Source);

            return result;
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

            // ImmutableArray<T>.Empty — struct, no heap allocation
            if (source.ContainerType == CsContainerType.Interface) return ImmutableArray<CsMember>.Empty;
            if (source.InheritedInterfaces == null) return ImmutableArray<CsMember>.Empty;

            var sourceMembers = source.GetComparisonMembers(MemberComparisonType.Security);

            // Build a HashSet for O(1) lookup instead of O(n) per element
            var sourceMemberKeys = new HashSet<int>(sourceMembers.Select(sm => sm.Key));
            
            var interfaceMembers = new Dictionary<int, CsMember>();

            foreach (var inheritedInterface in source.InheritedInterfaces)
            {
                var compareMembers = inheritedInterface.GetComparisonMembers(MemberComparisonType.Security);
                if (compareMembers.Count == 0) continue;

                foreach (var compareMember in compareMembers)
                {
                    if (!interfaceMembers.ContainsKey(compareMember.Key))
                        interfaceMembers.Add(compareMember.Key, compareMember.Value);
                }
            }

            if (interfaceMembers.Count == 0) return Array.Empty<CsMember>();

            // O(1) HashSet lookup replaces O(n) Any() — overall O(n) instead of O(n²)
            var missing = ImmutableArray.CreateBuilder<CsMember>(interfaceMembers.Count);
            foreach (var interfaceMember in interfaceMembers)
            {
                if (!sourceMemberKeys.Contains(interfaceMember.Key))
                    missing.Add(interfaceMember.Value);
            }

            return missing.Count > 0 ? missing.ToImmutable() : Array.Empty<CsMember>();
        }
    }
}