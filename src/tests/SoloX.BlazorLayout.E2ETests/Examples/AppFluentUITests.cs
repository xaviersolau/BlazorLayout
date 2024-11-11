// ----------------------------------------------------------------------
// <copyright file="AppFluentUITests.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using FluentAssertions;
using Microsoft.Playwright;
using SoloX.BlazorLayout.E2ETests.Utils;
using SoloX.CodeQuality.Playwright;
using Program = SoloX.BlazorLayout.Examples.WithFluentUI.App;

namespace SoloX.BlazorLayout.E2ETests.Examples
{
    public class AppFluentUITests
    {
        [Theory]
        [InlineData(Browser.Chromium, "")]
        [InlineData(Browser.Chromium, "Dock")]
        [InlineData(Browser.Chromium, "GridAuto")]
        [InlineData(Browser.Chromium, "GridManual")]
        [InlineData(Browser.Chromium, "DisplayOnWidth")]
        [InlineData(Browser.Firefox, "")]
        [InlineData(Browser.Firefox, "Dock")]
        [InlineData(Browser.Firefox, "GridAuto")]
        [InlineData(Browser.Firefox, "GridManual")]
        [InlineData(Browser.Firefox, "DisplayOnWidth")]
        [InlineData(Browser.Webkit, "")]
        [InlineData(Browser.Webkit, "Dock")]
        [InlineData(Browser.Webkit, "GridAuto")]
        [InlineData(Browser.Webkit, "GridManual")]
        [InlineData(Browser.Webkit, "DisplayOnWidth")]
        public async Task IsShouldDisplayComponentExample(Browser browser, string route)
        {
            var builder = PlaywrightTestBuilder.Create()
                .WithLocalHost(configuration =>
                {
                    configuration
                        .UseApplication<Program>()
                        .UseWebHostBuilder(builder =>
                        {
                            //builder.ConfigureServices(services =>
                            //{
                            //    services.AddTransient<IService, ServiceMock>();
                            //})
                            //.ConfigureAppConfiguration((app, conf) =>
                            //{
                            //    conf.AddJsonFile("appsettings.Test.json");
                            //})
                            //.UseSetting("SomeKey", "SomeValue");
                        });
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
                    opt.ViewportSize = new ViewportSize() { Width = 1300, Height = 800 };
                });


            var playwrightTest = await builder
                .BuildAsync(browser)
                .ConfigureAwait(true);

            await using var _ = playwrightTest.ConfigureAwait(false);

            byte[]? pngBuffer = null;

            await playwrightTest
                .GotoPageAsync(route, async page =>
                {
                    pngBuffer = await page.ScreenshotAsync().ConfigureAwait(false);
                });

            pngBuffer.Should().NotBeNull();

            var differences = ImageComparison.CompareAndGenerateImageDifference(
                $"../../../Examples/Screenshots/AppFluentUITests.IsShouldDisplayComponentExample{browser}{route}.png",
                pngBuffer!);

            differences.Should().BeFalse();
        }
    }
}
