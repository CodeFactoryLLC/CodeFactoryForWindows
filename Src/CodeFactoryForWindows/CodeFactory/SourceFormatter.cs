//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2023-2025 CodeFactory, LLC
//*****************************************************************************
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory
{
    /// <summary>
    /// Base implementation of source code formatting. This allows for control of formatted output from CodeFactory.
    /// </summary>
    public class SourceFormatter : ISourceFormatter
    {
        /// <summary>
        /// Field that holds the indent statement to be used in code blocks
        /// </summary>
        private readonly string _indentStatement;

        /// <summary>
        /// The string builder used to format the source code.
        /// </summary>
        readonly StringBuilder _codeFormatter = new StringBuilder();

        /// <summary>
        /// Creates a new instance of the <see cref="SourceFormatter"/>
        /// </summary>
        /// <param name="indentStatement">Optional parameter that allows you to set the target type of indent that will occur with each code statement.</param>
        public SourceFormatter(string indentStatement = "\t")
        {
            _indentStatement = indentStatement;
        }

        /// <summary>
        /// Appends code to the end of the current line in the formatter.
        /// </summary>
        /// <param name="code">The code to append.</param>
        public void AppendCode(string code)
        {
            if (string.IsNullOrEmpty(code)) return;
            _codeFormatter.Append(code);
        }

        /// <summary>
        /// Appends the specified code to the internal formatter, preceded by the specified level of indentation.
        /// </summary>
        /// <remarks>Each indentation level corresponds to a predefined indentation statement. If
        /// <paramref name="code"/> is <see langword="null"/> or empty, the method performs no action.</remarks>
        /// <param name="indentLevel">The number of indentation levels to prepend before appending the code. Must be non-negative.</param>
        /// <param name="code">The code to append. Cannot be <see langword="null"/> or empty.</param>
        public void AppendCode(int indentLevel, string code)
        {
            if (string.IsNullOrEmpty(code)) return;

            // Append the indent level
            for (int i = 0; i < indentLevel; i++)
            {
                _codeFormatter.Append(_indentStatement);
            }
            // Append the code
            _codeFormatter.Append(code);
        }

        /// <summary>
        /// Appends the specified code to the internal formatter, preceded by the specified indentation level,  and adds
        /// a new line at the end.
        /// </summary>
        /// <remarks>This method formats the code by adding the specified number of indentation levels
        /// before appending it,  followed by a new line. If <paramref name="code"/> is null or empty, the method
        /// performs no action.</remarks>
        /// <param name="indentLevel">The number of indentation levels to prepend before the code. Must be non-negative.</param>
        /// <param name="code">The code to append. Cannot be null or empty.</param>
        public void AppendCodeEndWithNewLine(int indentLevel, string code)
        {
            if (string.IsNullOrEmpty(code)) return;

            // Append the indent level
            for (int i = 0; i < indentLevel; i++)
            {
                _codeFormatter.Append(_indentStatement);
            }
            // Append the code
            _codeFormatter.Append(code);

            _codeFormatter.AppendLine();
        }

        /// <summary>
        /// Appends a new line of code to the formatter.
        /// </summary>
        /// <param name="indentLevel">The number of indent levels to add to the source code.</param>
        /// <param name="code">The code to add to the formatter.</param>
        public void AppendCodeLine(int indentLevel, string code)
        {
            AppendCodeLine(indentLevel);

            //Bug fix update for Issue #13
            if (!string.IsNullOrEmpty(code)) _codeFormatter.Append(code);
        }

        /// <summary>
        /// Appends a new line of code to the formatter.
        /// </summary>
        /// <param name="indentLevel">The number of indent levels to add to the source code.</param>
        public void AppendCodeLine(int indentLevel)
        {
            _codeFormatter.AppendLine();
            if (indentLevel > 0)
            {
                int currentLevel = 0;
                while (currentLevel < indentLevel)
                {
                    _codeFormatter.Append(_indentStatement);
                    currentLevel++;
                }
            }
        }

        /// <summary>
        /// Appends a target indent level to a already formatted block of code.
        /// </summary>
        /// <param name="indentLevel">The target indent level to be added to the existing code block.</param>
        /// <param name="codeBlock">The block of code to append to.</param>
        public void AppendCodeBlock(int indentLevel, string codeBlock)
        {
            if (string.IsNullOrEmpty(codeBlock)) return;

            //Split the existing documentation into individual lines to be processed.
            var codeLines = codeBlock.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            //iterate over each document line and confirm it can be formatted for C# xml documentation.
            foreach (string codeLine in codeLines)
            {
                AppendCodeLine(indentLevel, codeLine);
            }
        }

        /// <summary>
        /// Appends a target indent level to a already formatted block of code.
        /// </summary>
        /// <param name="indentLevel">The target indent level to be added to the existing code block.</param>
        /// <param name="codeBlock">The block of code to append to.</param>
        public void AppendCodeBlock(int indentLevel, IEnumerable<string> codeBlock)
        {
            if (codeBlock == null) return;

            //iterate over each document line and confirm it can be formatted for C# xml documentation.
            foreach (string codeLine in codeBlock)
            {
                AppendCodeLine(indentLevel, codeLine);
            }
        }

        /// <summary>
        /// Returns the formatted source code.
        /// </summary>
        /// <returns>Formatted SourceCode.</returns>
        public string ReturnSource()
        {
            return _codeFormatter.ToString();
        }

        /// <summary>
        /// Clears the formatter to be reused.
        /// </summary>
        public void ResetFormatter()
        {
            _codeFormatter.Clear();
        }
    }
}
