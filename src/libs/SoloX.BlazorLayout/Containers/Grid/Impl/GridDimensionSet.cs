﻿// ----------------------------------------------------------------------
// <copyright file="GridDimensionSet.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.BlazorLayout.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SoloX.BlazorLayout.Containers.Grid.Impl
{
    /// <summary>
    /// Set of a Grid dimensions that can be Rows or Columns.
    /// </summary>
    public class GridDimensionSet<TDimension> where TDimension : AGridDimension
    {
        private record CacheData
        {
            public CacheData(int repeat, string? name)
            {
                Repeat = repeat;
                Name = name;
            }

            public string? Name { get; }

            public int Repeat { get; }
        }

        private readonly SortedDictionary<TDimension, CacheData> dimensions = new SortedDictionary<TDimension, CacheData>();

        private readonly List<TDimension> expendedDimensions = new List<TDimension>();
        private readonly Dictionary<string, int> dimensionMap = new Dictionary<string, int>();

        private readonly Action gridNotifyHandler;

        /// <summary>
        /// Setup the Dimension Set with a Grid notifier handler.
        /// </summary>
        /// <param name="gridNotifyHandler">The Grid notifier handler.</param>
        public GridDimensionSet(Action gridNotifyHandler)
        {
            this.gridNotifyHandler = gridNotifyHandler;
        }

        /// <summary>
        /// Add a new dimension entry in the Set.
        /// </summary>
        /// <param name="dimension">The dimension entry to add.</param>
        public void Add(TDimension dimension)
        {
            if (dimension == null)
            {
                throw new ArgumentNullException(nameof(dimension));
            }

            if (this.dimensions.ContainsKey(dimension))
            {
                throw new NotSupportedException($"Dimension already added.");
            }

            this.dimensions.Add(dimension, new CacheData(dimension.Repeat, dimension.Name));

            ExpendDimensionsAndBuildMap();

            this.gridNotifyHandler();
        }

        /// <summary>
        /// Update dimension entry in the Set.
        /// </summary>
        /// <param name="dimension">The dimension entry to update.</param>
        public void Update(TDimension dimension)
        {
            if (dimension == null)
            {
                throw new ArgumentNullException(nameof(dimension));
            }

            if (!this.dimensions.TryGetValue(dimension, out var cache))
            {
                throw new NotSupportedException($"Unknown dimension to be updated.");
            }

            if (cache.Name == dimension.Name && cache.Repeat == dimension.Repeat)
            {
                return;
            }

            this.dimensions[dimension] = new CacheData(dimension.Repeat, dimension.Name);

            ExpendDimensionsAndBuildMap();

            this.gridNotifyHandler();
        }

        /// <summary>
        /// Remove a dimension entry from the Set.
        /// </summary>
        /// <param name="dimension"></param>
        public void Remove(TDimension dimension)
        {
            if (!this.dimensions.Remove(dimension))
            {
                throw new NotSupportedException($"Unknown dimension to be removed.");
            }

            ExpendDimensionsAndBuildMap();

            this.gridNotifyHandler();
        }

        private void ExpendDimensionsAndBuildMap()
        {
            this.dimensionMap.Clear();
            this.expendedDimensions.Clear();

            foreach (var dimensionItem in this.dimensions)
            {
                var dimension = dimensionItem.Key;
                if (!string.IsNullOrEmpty(dimension.Name))
                {
                    this.dimensionMap.Add(dimension.Name, this.expendedDimensions.Count);
                }

                for (var i = 0; i < dimension.Repeat; i++)
                {
                    this.expendedDimensions.Add(dimension);
                }
            }
        }

        /// <summary>
        /// Get the matching dimension index.
        /// </summary>
        /// <param name="nameOrIndex">Name or index of the dimension to match.</param>
        /// <returns></returns>
        public int GetDimensionIndex(object nameOrIndex)
        {
            if (nameOrIndex == null)
            {
                throw new ArgumentNullException(nameof(nameOrIndex));
            }

            if (nameOrIndex is string name)
            {
                if (this.dimensionMap.TryGetValue(name, out var index))
                {
                    return index;
                }
                else if (int.TryParse(name, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))
                {
                    return value;
                }

                throw new ArgumentOutOfRangeException(nameof(nameOrIndex));
            }
            else if (nameOrIndex is int index)
            {
                return index;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(nameOrIndex));
            }
        }

        /// <summary>
        /// Compute Dimensions style given the default sizing mode.
        /// </summary>
        /// <param name="defaultSizingMode">Default sizing mode to apply on dimensions without sizing mode defined.</param>
        /// <returns>The computed style.</returns>
        public string ComputeDimensionsStyle(Sizing defaultSizingMode) =>
            string.Join(" ",
                this.expendedDimensions.Select(c => ComputeDimensionStyle(c, defaultSizingMode)));

        private static string ComputeDimensionStyle(AGridDimension frame, Sizing defaultSizing) =>
            frame.Sizing.HasValue
            ? ComputeDimensionStyle(frame.Sizing.Value, frame.Size)
            : ComputeDimensionStyle(defaultSizing, frame.Size);

        private static string ComputeDimensionStyle(Sizing sizingMode, int size) =>
            sizingMode switch
            {
                Sizing.Fill => "1fr",
                Sizing.Proportion => $"{size}%",
                Sizing.Content => "auto",
                _ => throw new NotImplementedException(),
            };
    }
}
