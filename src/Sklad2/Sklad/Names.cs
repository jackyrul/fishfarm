using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sklad
{
    public static class Names
    {
        public static string[] Users = new[] { "System", "Admin", "Katya" };
        public static string[] Workers = new[] { "KIYEMBA", "OPYENE", "YASIN", "MICHEAL" };
        public static string[] Materials =
            new[]
            {
                "BONES",
                "SEASHELLS",
                "BLOOD",
                "MUKENE",
                "YEAST",
                "SUNFLOWER CAKE",
                "BARLEY ROOTLETS",
                "PREMIX",
                "CALCIUM MONOPHOSPHATE",
                "SALT",
                "RECYCLE OF FEED 30%",
                "RECYCLE OF FEED 35%",
                "SOYA CAKE",
                "WHEAT",
                "MAIZE",
                "SOYA OIL"
            };
        public static string[] Stages =
            new[]
            {
                "Supply",
                "Raw",
                "Milling",
                "Milled",
                "Extruder",
                "Finish",
                "Inventory"
            };
    }
}
