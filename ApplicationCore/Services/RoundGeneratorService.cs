using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApplicationCore
{
    public class RoundGeneratorService : IAsyncRoundGenerator<GrillMenuModel, GrillRoundModel>
    {
        /// <summary>
        /// For each Menu we have x items <br/>
        /// Each item has a width and a height <br/>
        /// First, we will order all the items by height, then we will place the first item starting from the bottom left in the grill; this item will be the highest one in the menu. <br/>
        /// Then we will keep putting items into the grill in a decreasing height order from left to right until the Grill width is filled or is unable to fit the current item. <br/>
        /// In that case, we will create another strip in the same Round putting first the highest item of what is left that fits. <br/>
        /// If no more items fit, then we will create another Round and repeat this process until there is no more items left <br/>
        /// </summary>
        /// <param name="menu">menu we will generate the rounds</param>
        /// <returns>list of <see cref="GrillRoundModel"/>  that this menu has generated</returns>
        public async Task<List<GrillRoundModel>> GenerateRounds(GrillMenuModel menu)
        {
            if (menu == null)
                return new List<GrillRoundModel>();
            return await GenerateRoundsMenu(menu);
        }

        
        private static async Task<List<GrillRoundModel>> GenerateRoundsMenu(GrillMenuModel menu)
        {
            List<GrillRoundModel> result = new List<GrillRoundModel>();
            List<GrillMenuItemModel> allItems = new List<GrillMenuItemModel>();
            foreach (var item in menu.Items)
            {
                allItems.AddRange(GrillMenuItemHelper.GenerateItemsWithoutQuantity(item));
            }

            List<GrillMenuItemModel> grillMenuItemsOrderedList = allItems.OrderByDescending(x => x.Height).ToList();

            GrillRoundModel currenRound = new GrillRoundModel();
            GrillRoundStripModel currenRoundStrip = new GrillRoundStripModel();
            GrillMenuItemModel currentItem;
            int? index;

            for (int i = 0; i < grillMenuItemsOrderedList.Count; i++)
            {
                currentItem = grillMenuItemsOrderedList[i];
                //If the item is higher or wider than the grill, we can't grill it
                if (currentItem.Height > GrillModel.HEIGHT || currentItem.Width > GrillModel.WIDTH)
                {
                    grillMenuItemsOrderedList.RemoveAt(i);
                    continue;
                }

                //if the width of the current item is greater than the width left of the grill
                //we create a new strip
                if (currentItem.Width > GrillModel.WIDTH - currenRoundStrip.Width)
                {
                    currenRound.GrillRoundStrips.Add(currenRoundStrip);
                    currenRoundStrip = new GrillRoundStripModel();
                    index = GetItemIndexThatFitsInHeight(grillMenuItemsOrderedList, GrillModel.HEIGHT - currenRound.CurrentMaxHeightAllStrips);
                    //In case is new Round, we set index to 0 due to we need to put the heighest item
                    //otherwise, we set as current item the heighest item that fits
                    i = index ?? 0;
                    currentItem = grillMenuItemsOrderedList[i];
                }

                // if there we cant fit another item or the item we need to save this strip in the current round and create a new one
                if (currentItem.Height > GrillModel.HEIGHT - currenRound.CurrentMaxHeightAllStrips)
                {
                    SaveCurrentStripAndCurrentOrder(result, ref currenRound, ref currenRoundStrip);
                }

                currenRoundStrip.GrillMenuItems.Add(currentItem);
                grillMenuItemsOrderedList.RemoveAt(i);

                //In case this is the last item, we save the current strip in the current Order
                if (grillMenuItemsOrderedList.Count == 0)
                {
                    SaveCurrentStripAndCurrentOrder(result, ref currenRound, ref currenRoundStrip);
                }

                //If we wasted all the smallest items in the current strip but we still have to fit other items that are taller,
                //we reset the index to 0. 
                if (i + 1 >= grillMenuItemsOrderedList.Count && grillMenuItemsOrderedList.Count > 0)
                {
                    // the index will be 0 in the next iteration
                    i = -1;
                }
            }
            return result;
        }
        /// <summary>
        /// It saves the current strip in the current round and generates another round and strip
        /// </summary>
        /// <param name="result"></param>
        /// <param name="currenRound"></param>
        /// <param name="currenRoundStrip"></param>
        private static void SaveCurrentStripAndCurrentOrder(List<GrillRoundModel> result, ref GrillRoundModel currenRound, ref GrillRoundStripModel currenRoundStrip)
        {
            if (!currenRoundStrip.GrillMenuItems.IsNullOrEmpty())
                currenRound.GrillRoundStrips.Add(currenRoundStrip);
            if (!currenRound.GrillRoundStrips.IsNullOrEmpty())
                result.Add(currenRound);

            currenRound = new GrillRoundModel();
            currenRoundStrip = new GrillRoundStripModel();
        }

        /// <summary>
        /// It checks if there is an item in the Menu that fits height wise in the Grill
        /// </summary>
        /// <param name="grillMenuItemsOrderedList">the list with all the items</param>
        /// <param name="heightNeedsToFit">height that the item needs to fit</param>
        /// <returns>returns the index of the item in the list or null in case there are no items with that height</returns>
        private static int? GetItemIndexThatFitsInHeight(List<GrillMenuItemModel> grillMenuItemsOrderedList, int heightNeedsToFit)
        {
            for (int i = 0; i < grillMenuItemsOrderedList.Count; i++)
            {
                if (grillMenuItemsOrderedList[i].Height <= heightNeedsToFit)
                    return i;
            }
            return null;
        }
    }
}
