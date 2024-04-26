using System;
using Enums;
using Fish;

namespace Economy.ShopScripts
{
    [Serializable]
    public struct SellListItem
    {
        public FishData data;
        public string name;
        public FishSize size;
        public int amount;
        public int singleCost;

        public SellListItem(SellShopItem item, int startAmount)
        {
            data = item.GetFishData();
            name = data.fishName;
            size = item.GetFishSize();
            singleCost = data.fishSellAmount[(int)item.GetFishSize()];
            amount = startAmount;
        }
    }
}