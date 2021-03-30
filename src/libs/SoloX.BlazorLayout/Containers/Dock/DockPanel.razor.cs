// ----------------------------------------------------------------------
// <copyright file="DockPanel.razor.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using SoloX.BlazorLayout.Core;
using System;

namespace SoloX.BlazorLayout.Containers.Dock
{
    /// <summary>
    /// Dock Panel to be docked in the DockContainer.
    /// </summary>
    public partial class DockPanel : IDisposable
    {
        [CascadingParameter]
        private DockContainer? Parent { get; set; }

        /// <summary>
        /// Gets/Sets witch side to dock the panel in the parent DockContainer.
        /// </summary>
        [Parameter]
        public Side Side { get; set; }

        private string ColumnStyle =>
            Parent?.GetColumnStyle(this) ?? throw new ArgumentNullException(nameof(Parent), "Dock must exist within a DockContainer");

        private string RowStyle =>
            Parent?.GetRowStyle(this) ?? throw new ArgumentNullException(nameof(Parent), "Dock must exist within a DockContainer");

        ///<inheritdoc/>
        protected override void OnInitialized()
        {
            if (Parent == null)
            {
                throw new ArgumentNullException(nameof(Parent), "Dock must exist within a DockContainer");
            }

            Parent.Add(this);

            base.OnInitialized();
        }

        ///<inheritdoc/>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Virtual dispose base method.
        /// </summary>
        /// <param name="dispose">true from Dispose call.</param>
        protected virtual void Dispose(bool dispose)
        {
            if (this.Parent != null)
            {
                try
                {
                    this.Parent.Remove(this);
                }
                catch (ObjectDisposedException)
                {
                    // Looks like the parent is also disposed.
                }
            }
        }
    }
}
