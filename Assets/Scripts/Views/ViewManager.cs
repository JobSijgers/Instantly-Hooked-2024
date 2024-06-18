using System;
using System.Collections.Generic;
using Catalogue;
using PauseMenu;
using Player.Inventory;
using Quests;
using ShopScripts;
using Shore;
using UnityEngine;
using UnityEngine.Events;
using Upgrades;

namespace Views
{
    public class ViewManager : MonoBehaviour
    {
        public static ViewManager instance;

        public event UnityAction<View> ViewShow;
        private void OnViewShow(View view) => ViewShow?.Invoke(view);
        public event UnityAction<View> ViewHide;
        private void OnViewHide(View view) => ViewHide?.Invoke(view);

        [SerializeField] private View startingView;
        [SerializeField] private View[] allViews;

        private readonly Stack<View> viewHistory = new();
        private View activeView;
        
        private void Awake()
        {
            instance = this;
            foreach (View view in allViews)
            {
                view.Initialize();
                view.Hide();
            }

            if (startingView == null)
                return;
            ShowView(startingView);
        }

        private void Update()
        {
            Type[] ignoredViews = { typeof(PauseUI), typeof(SellShopUI), typeof(UpgradeUI) };
            CheckKey<Inventory>(KeyCode.I, ignoredViews);
            CheckKey<CatalogueUI>(KeyCode.J, ignoredViews);
            CheckKey<QuestBookUI>(KeyCode.Q, ignoredViews);
        }

        public static void HideActiveView()
        {
            if (instance.activeView == null)
                return;
            instance.OnViewHide(instance.activeView);
            instance.activeView.Hide();
            instance.activeView = null;
        }
        
        public static void ShowView<T>(bool saveInHistory = true)
        {
            foreach (View view in instance.allViews)
            {
                if (view is not T)
                    continue;
                if (instance.activeView != null)
                {
                    if (saveInHistory)
                    {
                        instance.viewHistory.Push(instance.activeView);
                    }

                    instance.activeView.Hide();
                }

                view.Show();
                instance.OnViewShow(view);
                instance.activeView = view;
            }
        }

        public static void ShowView(View view, bool saveInHistory = true)
        {
            if (instance.activeView != null)
            {
                if (saveInHistory)
                {
                    instance.viewHistory.Push(instance.activeView);
                }

                instance.activeView.Hide();
            }

            view.Show();
            instance.OnViewShow(view);
            instance.activeView = view;
        }

        public static void ShowLastView()
        {
            if (instance.viewHistory.Count <= 0)
                return;
            instance.OnViewHide(instance.activeView);
            ShowView(instance.viewHistory.Pop());
        }
        
        public static void ClearHistory()
        {
            instance.viewHistory.Clear();
        }

        public static Type GetActiveView()
        {
            return instance.activeView.GetType();
        }

        private void CheckKey<T>(KeyCode key, Type[] ignoredViews) where T : View
        {
            if (!Input.GetKeyDown(key)) return;
            if (activeView == null) return;
            if (Array.Exists(ignoredViews, ignoredView => activeView.GetType() == ignoredView)) return;
            if (activeView is not T)
            {
                ShowView<T>();
            }
            else
            {
                ShowLastView();
            }
        }
    }
}