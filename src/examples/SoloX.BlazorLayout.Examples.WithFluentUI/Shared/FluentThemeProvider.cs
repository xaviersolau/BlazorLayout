using Microsoft.FluentUI.AspNetCore.Components;

namespace SoloX.BlazorLayout.Examples.WithFluentUI.Shared
{
    public class FluentThemeProvider
    {
        internal FluentThemeProviderComponent Component { get; set; }

        public Task SetupAsync(Theme theme)
        {
            return Component?.SetupAsync(theme) ?? Task.CompletedTask;
        }

        public Task<Theme> LoadThemeAsync()
        {
            return Component?.LoadThemeAsync() ?? throw new NullReferenceException();
        }
    }

    public class Theme
    {
        public float Luminance { get; set; } = 0.75f;

        public string StrokeWidth { get; set; } = "0.75";

        public string NeutralColor { get; set; } = "#000088";
        public string Color { get; set; } = "#008800";
        public string CornerRadius { get; set; } = "10";

    }
}
