using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoloX.BlazorLayout.Examples.Shared
{
    public partial class NavMenu
    {
        [Parameter]
        public bool SmallNav { get; set; }
    }
}
