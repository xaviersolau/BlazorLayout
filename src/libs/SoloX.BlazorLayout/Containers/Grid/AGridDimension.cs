// ----------------------------------------------------------------------
// <copyright file="AGridDimension.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using SoloX.BlazorLayout.Core;
using System;

namespace SoloX.BlazorLayout.Containers.Grid
{
    /// <summary>
    /// Abstract Grid Dimension.
    /// </summary>
    public abstract class AGridDimension : ComponentBase, IDisposable
    {
        private bool disposedValue;

        /// <summary>
        /// Gets/Sets the parent Grid container.
        /// </summary>
        [CascadingParameter]
        private GridContainer? Parent { get; set; }

        /// <summary>
        /// Gets/Sets the optional Grid dimension name.
        /// </summary>
        [Parameter]
        public string? Name { get; set; }

        /// <summary>
        /// Gets/Sets the Grid dimension size.
        /// </summary>
        [Parameter]
        public int Size { get; set; }

        /// <summary>
        /// Gets/Sets the Grid dimension sizing mode.
        /// </summary>
        [Parameter]
        public Sizing? Sizing { get; set; }

        /// <summary>
        /// Gets/Sets the Grid dimension Repeat number.
        /// </summary>
        [Parameter]
        public int Repeat { get; set; } = 1;

        ///<inheritdoc/>
        protected override void OnInitialized()
        {
            if (Parent == null)
            {
                throw new ArgumentNullException(nameof(Parent), "Column and Row must exist within a GridContainer");
            }

            AddToGrid(Parent);

            base.OnInitialized();
        }

        ///<inheritdoc/>
        protected override void OnParametersSet()
        {
            if (Parent == null)
            {
                throw new ArgumentNullException(nameof(Parent), "Column and Row must exist within a GridContainer");
            }

            UpdateGrid(Parent);

            base.OnParametersSet();
        }

        /// <summary>
        /// Add the current Grid Dimension to its parent Grid container.
        /// </summary>
        /// <param name="gridContainer">The Grid container the current Dimension must be added.</param>
        protected abstract void AddToGrid(GridContainer gridContainer);

        /// <summary>
        /// Update the current Grid Dimension in its parent Grid container.
        /// </summary>
        /// <param name="gridContainer">The Grid container the current Dimension must be updated.</param>
        protected abstract void UpdateGrid(GridContainer gridContainer);

        /// <summary>
        /// Remove from the current Grid Dimension in its parent Grid container.
        /// </summary>
        /// <param name="gridContainer">The Grid container the current Dimension must be removed.</param>
        protected abstract void RemoveFromGrid(GridContainer gridContainer);

        /// <summary>
        /// Dispose method for inherited classes.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    if (Parent != null)
                    {
                        RemoveFromGrid(Parent);
                    }
                }

                this.disposedValue = true;
            }
        }

        /// <summary>
        /// Dispose object resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
