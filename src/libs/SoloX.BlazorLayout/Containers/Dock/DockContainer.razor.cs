// ----------------------------------------------------------------------
// <copyright file="DockContainer.razor.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using SoloX.BlazorLayout.Core;

namespace SoloX.BlazorLayout.Containers.Dock
{
    /// <summary>
    /// Dock Panel container.
    /// </summary>
    public partial class DockContainer : AContainer
    {
        private readonly List<DockPanel> docks = new List<DockPanel>();

        internal void Add(DockPanel dock)
        {
            this.docks.Add(dock);

            this.StateHasChanged();
        }

        internal void Remove(DockPanel dock)
        {
            this.docks.Remove(dock);

            this.StateHasChanged();
        }

        private string ColumnsStyle =>
            this.ComputeFramesStyle(Side.Left, Side.Right);

        private string RowsStyle =>
            this.ComputeFramesStyle(Side.Top, Side.Bottom);

        private string ComputeFramesStyle(Side start, Side end)
        {
            return $"{this.ComputeFramesPartialStyle(start)} 1fr {this.ComputeFramesPartialStyle(end)}";
        }

        private string ComputeFramesPartialStyle(Side side)
        {
            return string.Join(" ", this.docks.Where(d => d.Side == side).Select(d => "auto"));
        }

        internal string GetColumnStyle(DockPanel dock)
            => GetFrameStyle(dock, Side.Left, Side.Right);

        internal string GetRowStyle(DockPanel dock)
            => GetFrameStyle(dock, Side.Top, Side.Bottom);

        private string GetFrameStyle(DockPanel dock, Side start, Side end)
        {
            if (dock.Side == start)
            {
                // | L0 | L1 | ***** | R2 | R1 | R0 |
                // 1    2    3       4    5    6    7

                var index = GetIndexOf(dock, start);
                return $"{index + 1} / {index + 2}";
            }
            else if (dock.Side == end)
            {
                // | L0 | L1 | ***** | R2 | R1 | R0 |
                // 1    2    3       4    5    6    7
                // count = 5 

                // | ***** | R2 | R1 | R0 |
                // 1       2    3    4    5
                // count = 3 

                var count = this.docks.Count(d => d.Side == start || d.Side == end);

                var index = count - GetIndexOf(dock, end);
                return $"{index + 1} / {index + 2}";
            }
            else
            {
                // L0 L1 T3 T3 T3 T3 R2  1
                // L0 L1 L6 T7 T7 R4 R2  2
                // L0 L1 L6 ** R8 R4 R2  3
                // L0 L1 L6 B9 R8 R4 R2  4
                // L0 L1 B5 B5 B5 R4 R2  5
                //1  2  3  4  5  6  7  8|6

                var count = this.docks.Count(d => d.Side == start || d.Side == end);
                var countLeft = this.docks.TakeWhile(d => d != dock).Count(d => d.Side == start);
                var countRight = this.docks.TakeWhile(d => d != dock).Count(d => d.Side == end);

                return $"{countLeft + 1} / {count - countRight + 2}";
            }
        }

        private int GetIndexOf(DockPanel dock, Side side)
        {
            var index = 0;
            foreach (var dockItem in this.docks.Where(d => d.Side == side))
            {
                if (object.ReferenceEquals(dock, dockItem))
                {
                    return index;
                }
                index++;
            }
            throw new KeyNotFoundException();
        }

        private string FillClass =>
            "dock-container " +
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
