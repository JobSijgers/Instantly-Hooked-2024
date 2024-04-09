using System.Collections.Generic;

namespace Interfaces
{
    public interface IShop
    {
        public int SellFish(List<FishData> fishToSell);
    }
}