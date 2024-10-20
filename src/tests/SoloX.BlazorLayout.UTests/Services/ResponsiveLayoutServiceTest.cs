// ----------------------------------------------------------------------
// <copyright file="ResponsiveLayoutServiceTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using Xunit.Abstractions;
using Xunit;
using SoloX.BlazorLayout.Services.Impl;
using SoloX.BlazorLayout.Services;
using FluentAssertions;

namespace SoloX.BlazorLayout.UTests.Services
{
    public class ResponsiveLayoutServiceTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public ResponsiveLayoutServiceTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ItShouldTriggerHideFooterEvent(bool hide)
        {
            var triggerCount = 0;
            var service = new ResponsiveLayoutService();

            using var handler = new EventHandler(service, e =>
            {
                triggerCount++;

                e.Request.Should().Be(ResponsiveLayoutRequestEventArgs.RequestType.HideFooter);

                e.BoolEventData.Should().Be(hide);
            });

            service.HideFooter(hide);

            triggerCount.Should().Be(1);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ItShouldTriggerHideHeaderEvent(bool hide)
        {
            var triggerCount = 0;
            var service = new ResponsiveLayoutService();

            using var handler = new EventHandler(service, e =>
            {
                triggerCount++;

                e.Request.Should().Be(ResponsiveLayoutRequestEventArgs.RequestType.HideHeader);

                e.BoolEventData.Should().Be(hide);
            });

            service.HideHeader(hide);

            triggerCount.Should().Be(1);
        }

        private sealed class EventHandler : IDisposable
        {
            private readonly ResponsiveLayoutService service;
            private readonly Action<ResponsiveLayoutRequestEventArgs> action;

            public EventHandler(ResponsiveLayoutService service, Action<ResponsiveLayoutRequestEventArgs> action)
            {
                this.service = service;
                this.action = action;
                this.service.RequestReceivedEvent += Service_RequestReceivedEvent;
            }

            private void Service_RequestReceivedEvent(object? sender, BlazorLayout.Services.ResponsiveLayoutRequestEventArgs e)
            {
                this.action(e);
            }

            public void Dispose()
            {
                this.service.RequestReceivedEvent -= Service_RequestReceivedEvent;
            }
        }
    }
}
