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
        private readonly GridDimensionSet<GridColumn> columns;
        private readonly GridDimensionSet<GridRow> rows;
        private bool initialized;

        /// <summary>
        /// Setup the grid container.
        /// </summary>
        public GridContainer()
        {
            this.columns = new GridDimensionSet<GridColumn>(DimensionChangedHandler);
            this.rows = new GridDimensionSet<GridRow>(DimensionChangedHandler);
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

        ///<inheritdoc/>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            this.initialized = true;
        }

        private void DimensionChangedHandler()
        {
            if (this.initialized)
            {
                this.StateHasChanged();
            }
        }

        internal void Add(GridColumn column) =>
            this.columns.Add(column);

        internal void Add(GridRow row) =>
            this.rows.Add(row);

        internal void Remove(GridColumn column) =>
            this.columns.Remove(column);

        internal void Remove(GridRow row) =>
            this.rows.Remove(row);

        internal int GetColumnIndex(string nameOrIndex) =>
            this.columns.GetDimensionIndex(nameOrIndex);

        internal int GetRowIndex(string nameOrIndex) =>
            this.rows.GetDimensionIndex(nameOrIndex);
        private string ColumnsStyle =>
            this.columns.ComputeDimensionsStyle(ColumnSizing);

        private string RowsStyle =>
            this.rows.ComputeDimensionsStyle(RowSizing);

        private string ComputeStyle()
        {
            return $"grid-template-columns: {ColumnsStyle}; grid-template-rows: {RowsStyle}; {Style}";
        }

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
