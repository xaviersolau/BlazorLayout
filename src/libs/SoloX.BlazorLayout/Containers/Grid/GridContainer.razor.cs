// ----------------------------------------------------------------------
// <copyright file="GridContainer.razor.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using Microsoft.AspNetCore.Components;
using SoloX.BlazorLayout.Core;
using SoloX.BlazorLayout.Containers.Grid.Impl;

namespace SoloX.BlazorLayout.Containers.Grid
{
    /// <summary>
    /// Grid Container
    /// </summary>
    public partial class GridContainer : AContainer
    {
        private readonly DimensionSet<Column> columns;
        private readonly DimensionSet<Row> rows;

        /// <summary>
        /// Setup the grid container.
        /// </summary>
        public GridContainer()
        {
            this.columns = new DimensionSet<Column>(this.StateHasChanged);
            this.rows = new DimensionSet<Row>(this.StateHasChanged);
        }

        /// <summary>
        /// Setup the Grid Column sizing mode.
        /// </summary>
        [Parameter]
        public Sizing ColumnSizing { get; set; }

        /// <summary>
        /// Setup the Grid Row sizing mode.
        /// </summary>
        [Parameter]
        public Sizing RowSizing { get; set; }

        internal void Add(Column column) =>
            this.columns.Add(column);

        internal void Add(Row row) =>
            this.rows.Add(row);

        internal void Remove(Column column) =>
            this.columns.Remove(column);

        internal void Remove(Row row) =>
            this.rows.Remove(row);

        internal int GetColumnIndex(string nameOrIndex) =>
            this.columns.GetDimensionIndex(nameOrIndex);

        internal int GetRowIndex(string nameOrIndex) =>
            this.rows.GetDimensionIndex(nameOrIndex);
        private string ColumnsStyle =>
            this.columns.ComputeDimensionsStyle(ColumnSizing);

        private string RowsStyle =>
            this.rows.ComputeDimensionsStyle(RowSizing);

        private string FillClass =>
            "grid-container " +
            Fill switch
            {
                Fill.None => "container-size-content",
                Fill.Full => "container-size-100",
                Fill.Vertical => "container-height-100",
                Fill.Horizontal => "container-width-100",
                _ => throw new ArgumentOutOfRangeException($"unexpected argument {nameof(Fill)}"),
            };
    }
}
