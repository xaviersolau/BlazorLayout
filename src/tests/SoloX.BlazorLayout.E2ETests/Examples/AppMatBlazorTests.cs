// ----------------------------------------------------------------------
// <copyright file="AppMatBlazorTests.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using FluentAssertions;
using Microsoft.Playwright;
using SoloX.BlazorLayout.E2ETests.Utils;
using SoloX.CodeQuality.Playwright;
using Program = SoloX.BlazorLayout.Examples.WithMatBlazor.Program;

namespace SoloX.BlazorLayout.E2ETests.Examples
{
    public class AppMatBlazorTests
    {
        [Fact]
        public async Task IsShouldDisplayComponentExample()
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
                    opt.ViewportSize = new ViewportSize() { Width = 800, Height = 600 };
                });


            var playwrightTest = await builder
                .BuildAsync(Browser.Chromium)
                .ConfigureAwait(true);

            await using var _ = playwrightTest.ConfigureAwait(false);

            byte[]? pngBuffer = null;

            await playwrightTest
                .GotoPageAsync(string.Empty, async page =>
                {
                    pngBuffer = await page.ScreenshotAsync().ConfigureAwait(false);
                });

            pngBuffer.Should().NotBeNull();

            var isDifferences = ImageComparison.CompareAndGenerateDifferenceImage(
                $"../../../Examples/Screenshots/AppMatBlazorTests.IsShouldDisplayComponentExample{Browser.Chromium}.png",
                pngBuffer!);

            isDifferences.Should().BeFalse();
        }
    }
}
