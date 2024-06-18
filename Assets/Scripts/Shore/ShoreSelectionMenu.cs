using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using ShopScripts;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Upgrades;
using Views;

namespace Shore
{
    public class ShoreSelectionMenu : ViewComponent
    {
        [SerializeField] private PlayableDirector director;
        [SerializeField] private TimelineAsset arriveTimeline;
        [SerializeField] private TimelineAsset leaveTimeline;
        private Type activeViewType;
        
        private void Start()
        {
            ViewManager.instance.ViewHide += CheckActiveView;
        }
        
        private void OnDestroy()
        {
            ViewManager.instance.ViewHide -= CheckActiveView;
        }
        public void OpenUpgradeShop()
        {
            StartCoroutine(OpenMenu<UpgradeUI>());
        }

        public void OpenSellShop()
        {
            StartCoroutine(OpenMenu<SellShopUI>());
        }
        
        public void GoToSea()
        {
            EventManager.OnLeftShore();
        }
        
        private IEnumerator OpenMenu<T>() where T : View
        {
            director.playableAsset = arriveTimeline;
            director.Play();
            yield return new WaitForSeconds((float)director.duration + 0.4f);
            ViewManager.ShowView<T>();
            activeViewType = typeof(T);
        }
        
        private void CheckActiveView(View closedView)
        {
            if (closedView.GetType() != activeViewType) return;
            director.playableAsset = leaveTimeline;
            director.Play();
            activeViewType = null;
        }
    }
}