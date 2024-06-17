using ShopScripts;
using Upgrades;
using Views;

namespace Shore
{
    public class ShoreUI : View
    {
        public void ShowUpgradeShop()
        {
            ViewManager.ShowView<UpgradeUI>();
        }

        public void ShowSellShop()
        {
            ViewManager.ShowView<SellShopUI>();
        }
    }
}