//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2020-2023 CodeFactory, LLC
//*****************************************************************************
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Extension methods that support model that implement the <see cref="CsMember"/> interface.
    /// </summary>
    public static class CsMemberExtensions
    {
        /// <summary>
        /// Gets the hash code for a formatted model signature using the C# format.
        /// </summary>
        /// <param name="source">The sources <see cref="ICsModel"/> model.</param>
        /// <param name="comparisonType">The type of comparision format to use when generating the hashcode. Default is set to the base comparision type.</param>
        /// <param name="mappedNamespaces">Optional parameter that provides namespaces to be mapped to.</param>
        /// <returns>The has code of the formatted model.</returns>
        /// <exception cref="ArgumentNullException">This is thrown if the model is null.</exception>
        public static int GetMemberComparisonHashCode(this CsMember source,
            MemberComparisonType comparisonType = MemberComparisonType.Base, List<MapNamespace> mappedNamespaces = null)
        {
            if (source == null) throw new ArgumentNullException("source");

            int result = 0;

            switch (source.MemberType)
            { 
                case CsMemberType.Field:

                    var field = source as CsField;

                    switch (comparisonType)
                    {
                        case MemberComparisonType.Base:
                            result = field.GetComparisonHashCode(CsSecurity.Public, false, mappedNamespaces: mappedNamespaces);
                            break;

                        case MemberComparisonType.Security:
                            result = field.GetComparisonHashCode(includeKeywords:false, mappedNamespaces: mappedNamespaces);
                            break;

                        case MemberComparisonType.Full:
                            result = field.GetComparisonHashCode( mappedNamespaces: mappedNamespaces);
                            break;
                    }

                    break;

                case CsMemberType.Property:

                    var property = source as CsProperty;
                    switch (comparisonType)
                    {
                        case MemberComparisonType.Base:
                            result = property.GetComparisonHashCode(includeKeywords:false,mappedNamespaces: mappedNamespaces,propertySecurity:CsSecurity.Public,getSecurity:CsSecurity.Public,setSecurity:CsSecurity.Public);
                            break;

                        case MemberComparisonType.Security:
                            result = property.GetComparisonHashCode(includeKeywords: false, mappedNamespaces: mappedNamespaces);
                            break;

                        case MemberComparisonType.Full:
                            result = property.GetComparisonHashCode(includeKeywords: true, mappedNamespaces: mappedNamespaces);
                            break;
                    }
                    break;

                case CsMemberType.Method:
                    var method = source as CsMethod;
                    switch (comparisonType)
                    {
                        case MemberComparisonType.Base:
                            result = method.GetComparisonHashCode(includeSecurity: false, includeKeywords: false, mappedNamespaces: mappedNamespaces);
                            break;

                        case MemberComparisonType.Security:
                            result = method.GetComparisonHashCode(includeSecurity: true, includeKeywords: false, mappedNamespaces: mappedNamespaces);
                            break;

                        case MemberComparisonType.Full:
                            result = method.GetComparisonHashCode(includeSecurity: true, includeKeywords: true, mappedNamespaces: mappedNamespaces);
                            break;
                    }
                    break;

                case CsMemberType.Event:
                    var eventModel = source as CsEvent;
                    switch (comparisonType)
                    {
                        case MemberComparisonType.Base:
                            result = eventModel.GetComparisonHashCode(includeSecurity:false, includeKeywords: false, mappedNamespaces:mappedNamespaces);
                            break;
                        case MemberComparisonType.Security:
                            result = eventModel.GetComparisonHashCode(includeSecurity: true, includeKeywords: false, mappedNamespaces: mappedNamespaces);
                            break;
                        case MemberComparisonType.Full:
                            result = eventModel.GetComparisonHashCode(includeSecurity: true, includeKeywords: true, mappedNamespaces: mappedNamespaces);
                            break;
                    }
                    break;

            }
            
            return result;
        }


       
    }
}
