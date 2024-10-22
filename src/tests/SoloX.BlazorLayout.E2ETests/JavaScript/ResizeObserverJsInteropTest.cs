// ----------------------------------------------------------------------
// <copyright file="ResizeObserverJsInteropTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using FluentAssertions;
using Xunit.Abstractions;
using SoloX.CodeQuality.Playwright;
using Microsoft.Playwright;

namespace SoloX.BlazorLayout.E2ETests.JavaScript
{
    public class ResizeObserverJsInteropTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public ResizeObserverJsInteropTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData(Browser.Chromium)]
        [InlineData(Browser.Firefox)]
        [InlineData(Browser.Webkit)]
        public async Task ItShouldBeNotifyed(Browser browser)
        {
            var path = Path.Combine(Path.GetFullPath(Path.GetDirectoryName(this.GetType().Assembly.Location)!), "JavaScript/Resources");

            var builder = PlaywrightTestBuilder.Create()
                .WithLocalHost(localHostBuilder =>
                {
                    localHostBuilder
                        .UseWebHostWithWwwRoot(path, "resizeIndex.html");
                })
                .WithPlaywrightOptions(opt =>
                {
                    //opt.Headless = false;
                    //opt.SlowMo = 1000;
                    //opt.Timeout = 60000;
                })
                .WithPlaywrightNewContextOptions(opt =>
                {
                    // Set up the browser view port size.
                    opt.ViewportSize = new ViewportSize() { Width = 1000, Height = 100 };
                });

            var playwrightTest = await builder
                .BuildAsync(browser)
                .ConfigureAwait(true);

            await using var _ = playwrightTest.ConfigureAwait(false);

            await playwrightTest
                .GotoPageAsync(string.Empty, async page =>
                {
                    var res = await page.EvaluateAsync(@"

                                    window.resizeManager.setupModule(true, null, false);

                                ").ConfigureAwait(false);

                    var res1 = await page.EvaluateAsync(@"() => {

                                    window.size = {
                                        count: 0,
                                    };

                                    const resizeCallback = { };

                                    resizeCallback.invokeMethodAsync = (name, width, height) => {
                                        console.log('Callback on resize');
                                        window.size.width = width;
                                        window.size.height = height;
                                        window.size.count++;
                                    };

                                    window.resizeManager.registerResizeCallback(resizeCallback, 'id1', window.document.body);

                                    return window.size;
                                }").ConfigureAwait(false);

                    res1.Should().NotBeNull();

                    var width1 = res1.Value.GetProperty("width").GetInt32();

                    width1.Should().Be(1000);

                    await page.SetViewportSizeAsync(1200, 100).ConfigureAwait(false);

                    // Wait a few in order to give time to the scroll listener to be processed.
                    await Task.Delay(100).ConfigureAwait(false);

                    var res2 = await page.EvaluateAsync(@"() => {

                                    console.log(window.resizeManager);

                                    return window.size;
                                }").ConfigureAwait(false);

                    res2.Should().NotBeNull();

                    var width2 = res2.Value.GetProperty("width").GetInt32();

                    width2.Should().Be(1200);
                }).ConfigureAwait(true);
        }
    }
}
