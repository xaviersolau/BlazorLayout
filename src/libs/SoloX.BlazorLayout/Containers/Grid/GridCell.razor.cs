// ----------------------------------------------------------------------
// <copyright file="GridCell.razor.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using System;

namespace SoloX.BlazorLayout.Containers.Grid
{
    /// <summary>
    /// A Grid Cell panel.
    /// </summary>
    public partial class GridCell
    {
        [CascadingParameter]
        private GridContainer? Parent { get; set; }

        /// <summary>
        /// Cell column location.
        /// </summary>
        [Parameter]
        public object? Column { get; set; }

        /// <summary>
        /// Optional Cell column location end.
        /// </summary>
        [Parameter]
        public object? ColumnEnd { get; set; }

        /// <summary>
        /// Cell row location.
        /// </summary>
        [Parameter]
        public object? Row { get; set; }

        /// <summary>
        /// Optional Cell row location end.
        /// </summary>
        [Parameter]
        public object? RowEnd { get; set; }

        private string ColumnStyle
        {
            get
            {
                if (Parent == null)
                {
                    throw new ArgumentNullException(nameof(Parent), "Cell must exist within a GridContainer");
                }
                if (Column == null)
                {
                    return string.Empty;
                }

                return ColumnEnd == null
                    ? $"{Parent.GetColumnIndex(Column) + 1}"
                    : $"{Parent.GetColumnIndex(Column) + 1}/{Parent.GetColumnIndex(ColumnEnd) + 2}";
            }
        }

        private string RowStyle
        {
            get
            {
                if (Parent == null)
                {
                    throw new ArgumentNullException(nameof(Parent), "Cell must exist within a GridContainer");
                }
                if (Row == null)
                {
                    return string.Empty;
                }

                return RowEnd == null
                    ? $"{Parent.GetRowIndex(Row) + 1}"
                    : $"{Parent.GetRowIndex(Row) + 1}/{Parent.GetRowIndex(RowEnd) + 2}";
            }
        }

        private string ComputeStyle()
        {
            return $"grid-column: {ColumnStyle}; grid-row: {RowStyle}; {Style}";
        }

        ///<inheritdoc/>
        protected override void OnInitialized()
        {
            if (Parent == null)
            {
                throw new ArgumentNullException(nameof(Parent), "Cell must exist within a GridContainer");
            }

            base.OnInitialized();
        }
    }
}
