// ----------------------------------------------------------------------
// <copyright file="Row.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;

namespace SoloX.BlazorLayout.Containers.Grid
{
    /// <summary>
    /// A Row grid dimension.
    /// </summary>
    public class Row : ADimension
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
    }
}
