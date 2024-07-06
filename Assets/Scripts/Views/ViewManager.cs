using System;
using System.Collections.Generic;
using Catalogue;
using Inventory;
using PauseMenu;
using Quests;
using ShopScripts;
using Tutorial;
using UnityEngine;
using UnityEngine.Events;
using Upgrades;

namespace Views
{
    public class ViewManager : MonoBehaviour
    {
        public static ViewManager instance;

        [SerializeField] private View startingView;
        [SerializeField] private View[] allViews;

        public event UnityAction<View> ViewShow;
        private void OnViewShow(View view) => ViewShow?.Invoke(view);
        public event UnityAction<View> ViewHide;
        private void OnViewHide(View view) => ViewHide?.Invoke(view);

        private readonly Stack<View> viewHistory = new();
        private View activeView;

        // If these views are active the input will not go though
        private readonly Type[] ignoredViews = { typeof(PauseUI), typeof(SellShopUI), typeof(UpgradeUI), typeof(TutorialPopup) };

        // If these views are active the view will not be saved in the history
        private readonly Type[] dontSaveIfActive = { typeof(InventoryManager), typeof(CatalogueBookUI), typeof(QuestBookUI) };

        private void Awake()
        {
            instance = this;

            // Initialize all views and hide them
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
            // Check for key presses to show specific views
            CheckKey<InventoryBookUI>(KeyCode.I, ignoredViews, dontSaveIfActive);
            CheckKey<CatalogueBookUI>(KeyCode.J, ignoredViews, dontSaveIfActive);
            CheckKey<QuestBookUI>(KeyCode.Q, ignoredViews, dontSaveIfActive);
        }

        /// <summary>
        /// Hide the currently active view
        /// </summary>
        /// <param name="removeActiveView">if you want to remove the view as the active view</param>
        public static void HideActiveView(bool removeActiveView = true)
        {
            if (instance.activeView == null)
                return;
            instance.OnViewHide(instance.activeView);
            instance.activeView.Hide();
            if (removeActiveView)
                instance.activeView = null;
        }

        /// <summary>
        /// Show a view of a specific type
        /// </summary>
        /// <param name="saveInHistory">if the view is saved in history</param>
        /// <typeparam name="T">The type of view to show</typeparam>
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
                instance.activeView = view;
                instance.OnViewShow(view);
            }
        }

        /// <summary>
        /// Show a specific view with a reference
        /// </summary>
        /// <param name="view">view to show</param>
        /// <param name="saveInHistory">if the view is saved in history</param>
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
            instance.activeView = view;
            instance.OnViewShow(view);
        }

        // Show the last view in the history
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

        // Check for a key press to show a specific view
        private void CheckKey<T>(KeyCode key, Type[] ignoredViews, Type[] dontSaveIfActive) where T : View
        {
            if (!Input.GetKeyDown(key)) return;
            if (activeView == null) return;
            if (Array.Exists(ignoredViews, ignoredView => activeView.GetType() == ignoredView)) return;

            if (activeView is not T)
            {
                if (Array.Exists(dontSaveIfActive, dontSave => activeView.GetType() == dontSave))
                {
                    ShowView<T>(false);
                    return;
                }

                ShowView<T>();
            }
            else
            {
                ShowLastView();
            }
        }
    }
}