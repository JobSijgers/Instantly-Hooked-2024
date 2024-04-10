using System;
using Enums;

namespace Economy.ShopScripts
{
    [Serializable]
    public struct SellListItem
    {
        public string name;
        public FishSize size;
        public int amount;
        public int singleCost;

        public SellListItem(ShopItem item, int startAmount)
        {
            name = item.GetFishData().fishName;
            size = item.GetFishSize();
            singleCost = item.GetFishData().fishSellAmount[(int)item.GetFishSize()];
            amount = startAmount;
        }
    }
}