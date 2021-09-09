using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class GrillMenuMockHelper
    {
        public static List<GrillMenu> MockMenus()
        {
            GrillMenu menu;
            GrillMenuItem item;
            Random rnd = new Random();
            List<GrillMenu> grillMenus = new List<GrillMenu>();

            int itemNumberMenus = rnd.Next(1, 10);
            int itemNumberItems = rnd.Next(1, 10);
            for (int i = 0; i < itemNumberMenus; i++)
            {
                menu = new GrillMenu
                {
                    Guid = Guid.NewGuid(),
                    Id = rnd.Next().ToString(),
                    Menu = rnd.Next().ToString(),
                };


                for (int j = 0; j < itemNumberItems; j++)
                {
                    item = new GrillMenuItem()
                    {
                        Duration = rnd.Next().ToString(),
                        Guid = Guid.NewGuid(),
                        Id = rnd.Next().ToString(),
                        Length = rnd.Next(1, 30),
                        Quantity = rnd.Next(1, 10),
                        Width = rnd.Next(1, 20),
                        Name = rnd.Next().ToString()
                    };
                    menu.Items.Add(item);
                }
                grillMenus.Add(menu);
            }
            return grillMenus;
        }
    }
}
