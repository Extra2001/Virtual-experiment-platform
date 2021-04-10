using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class CustomSettings
{
    public Resolution resolution { get; set; }

    public class Resolution
    {
        public int width { get; set; } = Screen.width;
        public int height { get; set; } = Screen.height;
        public int refreshRate { get; set; }
    }
}
