// ----------------------------------------------------------------------
// <copyright file="GridColumn.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;

namespace SoloX.BlazorLayout.Containers.Grid
{
    /// <summary>
    /// A Column grid dimension.
    /// </summary>
    public class GridColumn : AGridDimension
    {
        ///<inheritdoc/>
        protected override void AddToGrid(GridContainer gridContainer)
        {
            if (gridContainer == null)
            {
                throw new ArgumentNullException(nameof(gridContainer));
            }

            gridContainer.Add(this);
        }

        ///<inheritdoc/>
        protected override void UpdateGrid(GridContainer gridContainer)
        {
            if (gridContainer == null)
            {
                throw new ArgumentNullException(nameof(gridContainer));
            }

            gridContainer.Update(this);
        }

        ///<inheritdoc/>
        protected override void RemoveFromGrid(GridContainer gridContainer)
        {
            if (gridContainer == null)
            {
                throw new ArgumentNullException(nameof(gridContainer));
            }

            gridContainer.Remove(this);
        }
    }
}
