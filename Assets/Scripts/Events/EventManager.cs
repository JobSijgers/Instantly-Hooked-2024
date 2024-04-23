using Economy.ShopScripts;
using Enums;
using Fish;
using UnityEngine.Events;
using Upgrades;

namespace Events
{
    public static class EventManager
    {
        public static event UnityAction<float> TimeUpdate;
        public static event UnityAction<int> NewDay;
        public static event UnityAction<int> MoneyUpdate;
        public static event UnityAction Dock;
        public static event UnityAction UnDock;
        public static event UnityAction ShopOpen;
        public static event UnityAction ShopClose;
        public static event UnityAction<int> ShopSell;
        public static event UnityAction<SellListItem[]> SellSelectedButton;
        public static event UnityAction<FishData, FishSize> FishCaught;
        public static event UnityAction<float> DepthUpdate;
        public static event UnityAction<LineLengthUpgrade> LineLengthUpgradeBought;
        public static event UnityAction<ReelSpeedUpgrade> ReelSpeedUpgradeBought;
        public static event UnityAction<ShipSpeedUpgrade> ShipSpeedUpgradeBought;
        public static event UnityAction<SonarUpgrade> SonarUpgradeBought;

        public static void OnTimeUpdate(float value) => TimeUpdate?.Invoke(value);
        public static void OnNewDay(int value) => NewDay?.Invoke(value);
        public static void OnMoneyUpdate(int newMoney) => MoneyUpdate?.Invoke(newMoney);
        public static void OnDock() => Dock?.Invoke();
        public static void OnUndock() => UnDock?.Invoke();
        public static void OnShopOpen() => ShopOpen?.Invoke();
        public static void OnShopClose() => ShopClose?.Invoke();
        public static void OnShopSell(int sellAmount) => ShopSell?.Invoke(sellAmount);
        public static void OnSellSelectedButton(SellListItem[] items) => SellSelectedButton?.Invoke(items);
        public static void OnFishCaught(FishData data, FishSize size) => FishCaught?.Invoke(data, size);
        public static void OnDepthUpdate(float newDepth) => DepthUpdate?.Invoke(newDepth);
        public static void OnLineLengthUpgradeBought(LineLengthUpgrade upgrade) => LineLengthUpgradeBought?.Invoke(upgrade);
        public static void OnReelSpeedUpgradeBought(ReelSpeedUpgrade upgrade) => ReelSpeedUpgradeBought?.Invoke(upgrade);
        public static void OnShipSpeedUpgradeBought(ShipSpeedUpgrade upgrade) => ShipSpeedUpgradeBought?.Invoke(upgrade);
        public static void OnSonarUpgradeBought(SonarUpgrade upgrade) => SonarUpgradeBought?.Invoke(upgrade);
    }
}