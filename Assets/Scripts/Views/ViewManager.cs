using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Views
{
    public class ViewManager : MonoBehaviour
    {
        public static ViewManager instance;

        public event UnityAction<View> ViewShow;
        public void OnViewShow(View view) => ViewShow?.Invoke(view);

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

        public static void HideActiveView()
        {
            if (instance.activeView == null)
                return;
            instance.activeView.Hide();
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
            ShowView(instance.viewHistory.Pop());
        }
        
        public static void ClearHistory()
        {
            instance.viewHistory.Clear();
        }
    }
}