using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore
{
    public static class GrillMenuItemHelper
    {
        /// <summary>
        /// It generates <see cref="GrillMenuItemModelWQuantity.Quantity"/>  elements of <see cref="GrillMenuItemModel"/> with the same height and width 
        /// </summary>
        /// <param name="menuItemModelWQuantity"></param>
        /// <returns></returns>
        public static List<GrillMenuItemModel> GenerateItemsWithoutQuantity(GrillMenuItemModelWQuantity menuItemModelWQuantity)
        {
            List<GrillMenuItemModel> result = new List<GrillMenuItemModel>();
            if (menuItemModelWQuantity == null || menuItemModelWQuantity.Quantity <= 0 || menuItemModelWQuantity.Quantity == int.MaxValue)
                return result;


            for (int i = 0; i < menuItemModelWQuantity.Quantity; i++)
            {
                result.Add(new GrillMenuItemModel()
                {
                    Height = menuItemModelWQuantity.Height,
                    Width = menuItemModelWQuantity.Width
                });
            }
            return result;
        }
    }
}
