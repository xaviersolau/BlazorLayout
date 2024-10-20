// ----------------------------------------------------------------------
// <copyright file="ScrollObserverJsInteropTest.cs" company="Xavier Solau">
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
    public class ScrollObserverJsInteropTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public ScrollObserverJsInteropTest(ITestOutputHelper testOutputHelper)
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
                        .UseWebHostWithWwwRoot(path, "scrollIndex.html");
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
                    opt.ViewportSize = new ViewportSize() { Width = 800, Height = 600 };
                });

            var playwrightTest = await builder
                .BuildAsync(browser)
                .ConfigureAwait(true);

            await using var _ = playwrightTest.ConfigureAwait(false);

            await playwrightTest
                .GotoPageAsync(string.Empty, async page =>
                {
                    var res = await page.EvaluateAsync(@"

                                    window.scrollManager.setupModule(true, null, false);

                                ").ConfigureAwait(false);

                    var res1 = await page.EvaluateAsync(@"() => {

                                    window.scroll = {
                                        count: 0,
                                    };

                                    const scrollCallback = { };

                                    scrollCallback.invokeMethodAsync = (name, width, left, clientWidth, height, top, clientHeight) => {
                                        console.log('Callback on scroll');
                                        window.scroll.width = width;
                                        window.scroll.left = left;
                                        window.scroll.clientWidth = clientWidth;
                                        window.scroll.height = height;
                                        window.scroll.top = top;
                                        window.scroll.clientHeight = clientHeight;
                                        window.scroll.count++;
                                    };

                                    window.scrollManager.registerScrollCallback(scrollCallback, 'id1', window.document.body);

                                    return window.scroll;
                                }").ConfigureAwait(false);

                    res1.Should().NotBeNull();

                    var left1 = res1.Value.GetProperty("left").GetInt32();

                    left1.Should().Be(0);

                    var top1 = res1.Value.GetProperty("top").GetInt32();

                    top1.Should().Be(0);

                    await page.EvaluateAsync(@"() => {

                                    console.log(window.scrollManager);

                                    window.document.body.scrollLeft = 150;
                                    window.document.body.scrollTop = 80;

                                }").ConfigureAwait(false);

                    // Wait a few in order to give time to the scroll listener to be processed.
                    await Task.Delay(100).ConfigureAwait(false);

                    var res2 = await page.EvaluateAsync(@"() => {

                                    return window.scroll;
                                }").ConfigureAwait(false);

                    res2.Should().NotBeNull();

                    var left2 = res2.Value.GetProperty("left").GetInt32();

                    left2.Should().Be(150);

                    var top2 = res2.Value.GetProperty("top").GetInt32();

                    top2.Should().Be(80);


                }).ConfigureAwait(true);
        }
    }
}
