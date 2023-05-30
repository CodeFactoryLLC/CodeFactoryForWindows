using CodeFactory.WinVs.Models.CSharp.FormattedSyntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.WinVs.Models.CSharp
{
    /// <summary>
    /// Extension methods that support the generation of source in the C# language from the <see cref="CsSecurity"/> model.
    /// </summary>
    public static class GenerateCSharpCsSecurityExtensions
    {
        /// <summary>
        /// Gets the security keyword for the C# language.
        /// </summary>
        /// <param name="source">The source security object to get the keyword from.</param>
        /// <returns>The name of the security keyword or null.</returns>
        public static string GenerateCSharpKeyword(this CsSecurity source)
        {
            string result;

            switch (source)
            {
                case CsSecurity.Public:
                    result = Security.Public;
                    break;

                case CsSecurity.Protected:
                    result = Security.Protected;
                    break;

                case CsSecurity.Internal:
                    result = Security.Internal;
                    break;

                case CsSecurity.Private:
                    result = Security.Private;
                    break;
                case CsSecurity.ProtectedInternal:
                    result = Security.ProtectedInternal;
                    break;

                case CsSecurity.ProtectedOrInternal:
                    result = Security.PrivateProtected;
                    break;

                case CsSecurity.Unknown:
                    result = null;
                    break;

                default:
                    result = null;
                    break;
            }

            return result;
        }
    }
}
