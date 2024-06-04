using Economy.ShopScripts;
using Enums;
using Fish;
using PauseMenu;
using Quests;
using UnityEngine.Events;
using Upgrades.Scriptable_Objects;

namespace Events
{
    public static class EventManager
    {
        public static event UnityAction<float> TimeUpdate;
        public static event UnityAction<int> NewDay;
        public static event UnityAction<int> MoneyUpdate;
        public static event UnityAction Dock;
        public static event UnityAction DockSuccess;
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
        public static event UnityAction<bool> BoatControlsChange;
        public static event UnityAction BoatAutoDock;
        public static event UnityAction<QuestProgress> QuestCompleted;
        public static event UnityAction<QuestProgress> QuestUpdated;
        public static event UnityAction<QuestProgress> QuestHighlighted;
        public static event UnityAction<QuestProgress> QuestUnHighlighted; 
        
        public static void OnTimeUpdate(float value) => TimeUpdate?.Invoke(value);
        public static void OnNewDay(int value) => NewDay?.Invoke(value);
        public static void OnMoneyUpdate(int newMoney) => MoneyUpdate?.Invoke(newMoney);
        public static void OnDock() => Dock?.Invoke();
        public static void OnDockSuccess() => DockSuccess?.Invoke();
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
        public static void OnBoatControlsChanged(bool state) => BoatControlsChange?.Invoke(state);
        public static void OnBoatAutoDock() => BoatAutoDock?.Invoke();
        public static void OnQuestCompleted(QuestProgress quest) => QuestCompleted?.Invoke(quest);
        public static void OnQuestUpdated(QuestProgress quest) => QuestUpdated?.Invoke(quest);
        public static void OnQuestHighlight(QuestProgress quest) => QuestHighlighted?.Invoke(quest);
        public static void OnQuestUnHighlight(QuestProgress quest) => QuestUnHighlighted?.Invoke(quest);
    }
}