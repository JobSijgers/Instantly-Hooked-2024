using System.Resources;
using Economy.ShopScripts;
using Enums;
using Fish;
using PauseMenu;
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
        public static event UnityAction SellShopOpen;
        public static event UnityAction SellShopClose;
        public static event UnityAction UpgradeShopOpen;
        public static event UnityAction UpgradeShopClose;
        public static event UnityAction<int> ShopSell;
        public static event UnityAction<SellListItem[]> SellSelectedButton;
        public static event UnityAction<FishData, FishSize> FishCaught;
        public static event UnityAction<float> DepthUpdate;
        public static event UnityAction<Upgrade> UpgradeBought;
        public static event UnityAction NotEnoughMoney;
        public static event UnityAction<PauseState> PauseStateChange;
        public static event UnityAction ArrivedAtShore;
        public static event UnityAction LeftShore;
        public static event UnityAction PlayerDied;
        
        public static void OnTimeUpdate(float value) => TimeUpdate?.Invoke(value);
        public static void OnNewDay(int value) => NewDay?.Invoke(value);
        public static void OnMoneyUpdate(int newMoney) => MoneyUpdate?.Invoke(newMoney);
        public static void OnDock() => Dock?.Invoke();
        public static void OnUndock() => UnDock?.Invoke();
        public static void OnSellShopOpen() => SellShopOpen?.Invoke();
        public static void OnSellShopClose() => SellShopClose?.Invoke();
        public static void OnUpgradeShopOpen() => UpgradeShopOpen?.Invoke();
        public static void OnUpgradeShopClose() => UpgradeShopClose?.Invoke();
        public static void OnShopSell(int sellAmount) => ShopSell?.Invoke(sellAmount);
        public static void OnSellSelectedButton(SellListItem[] items) => SellSelectedButton?.Invoke(items);
        public static void OnFishCaught(FishData data, FishSize size) => FishCaught?.Invoke(data, size);
        public static void OnDepthUpdate(float newDepth) => DepthUpdate?.Invoke(newDepth);
        public static void OnUpgradeBought(Upgrade upgrade) => UpgradeBought?.Invoke(upgrade);
        public static void OnNotEnoughMoney() => NotEnoughMoney?.Invoke();
        public static void OnPauseSateChange(PauseState state) => PauseStateChange?.Invoke(state);
        public static void OnArrivedAtShore() => ArrivedAtShore?.Invoke();
        public static void OnLeftShore() => LeftShore?.Invoke();
        public static void OnPlayerDied() => PlayerDied?.Invoke();
    }
}