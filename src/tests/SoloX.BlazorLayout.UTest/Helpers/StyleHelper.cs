// ----------------------------------------------------------------------
// <copyright file="StyleHelper.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SoloX.BlazorLayout.UTest.Helpers
{
    /// <summary>
    /// Helper to read style attribute from Html tree element.
    /// </summary>
    public static class StyleHelper
    {
        internal class CssProperty
        {
            public CssProperty(string name, string value)
            {
                Name = name;
                Value = value;
            }

            public string Name { get; }
            public string Value { get; }
        }

        internal static IEnumerable<CssProperty> LoadStyleAttribute(IElement element)
        {
            var style = element.GetAttribute("style");

            return style.Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Where(e => !string.IsNullOrWhiteSpace(e))
                .Select(e =>
                {
                    var words = e.Split(':');

                    return new CssProperty(words[0].Trim(), words[1].Trim());
                });
        }
    }
}
