using Events;
using ShopScripts;
using Upgrades;
using Views;

namespace Shore
{
    public class ShoreSelectionMenu : ViewComponent
    {
        public void OpenUpgradeShop()
        {
            ViewManager.ShowView<UpgradeUI>();
        }

        public void OpenSellShop()
        {
            ViewManager.ShowView<SellShopUI>();
        }
        
        public void GoToSea()
        {
            EventManager.OnLeftShore();
        }
    }
}