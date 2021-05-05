using Blazorise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoloX.BlazorLayout.Examples.WithBlazorise
{
    public partial class App
    {
        private Theme Theme { get; } = new Theme()
        {
            BackgroundOptions = new ThemeBackgroundOptions()
            {
                Primary = ThemeColors.Purple._700.Value,
                Secondary = ThemeColors.LightGreen._700.Value,
                Dark = ThemeColors.Gray._700.Value,
                Light = ThemeColors.LightBlue._200.Value,
                Body = ThemeColors.Lime._100.Value,
            },
            ColorOptions = new ThemeColorOptions()
            {
                Primary = ThemeColors.Purple._700.Value,
                Secondary = ThemeColors.LightGreen._700.Value,
                Dark = ThemeColors.Gray._700.Value,
                Light = ThemeColors.LightBlue._200.Value,
            },
            TextColorOptions = new ThemeTextColorOptions()
            {
                Primary = ThemeColors.Purple._700.Value,
                Secondary = ThemeColors.LightGreen._700.Value,
                Dark = ThemeColors.Gray._700.Value,
                Light = ThemeColors.LightBlue._200.Value,
            },
        };
    }
}
