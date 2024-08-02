// ----------------------------------------------------------------------
// <copyright file="AsyncDisposable.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace SoloX.BlazorLayout.Services.Impl
{
    internal class AsyncDisposable : IAsyncDisposable
    {
        private readonly Func<ValueTask> disposeHandler;
        private readonly string id;
        private bool isDisposed;
        public AsyncDisposable(string id, Func<ValueTask> disposeHandler)
        {
            this.id = id;
            this.disposeHandler = disposeHandler;
        }
#pragma warning disable CA1513
        public ValueTask DisposeAsync()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException($"Object {this.id} already disposed.");
            }

            this.isDisposed = true;
            return this.disposeHandler();
        }
#pragma warning restore CA1513
    }
}
