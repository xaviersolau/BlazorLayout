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
    public abstract class AGridDimension : ComponentBase
    {
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

        /// <summary>
        /// Add the current Grid Dimension to its parent Grid container.
        /// </summary>
        /// <param name="gridContainer">The Grid container the current Dimension must be added.</param>
        protected abstract void AddToGrid(GridContainer gridContainer);
    }
}
