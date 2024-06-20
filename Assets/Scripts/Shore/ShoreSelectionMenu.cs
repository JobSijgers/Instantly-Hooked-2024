using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Events;
using ShopScripts;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;
using UnityEngine.Timeline;
using UnityEngine.UI;
using Upgrades;
using Views;

namespace Shore
{
    public class ShoreSelectionMenu : ViewComponent
    {
        [Serializable]
        public class ShopType
        {
            public string shopName;
            public CinemachineVirtualCamera camera;
            public Material truckMaterial;
        }

        [SerializeField] private PlayableDirector director;
        [SerializeField] private ShopType[] shopTypes;
        [SerializeField] private MeshRenderer truckRenderer;
        [SerializeField] private TimelineAsset arriveTimeline;
        [SerializeField] private TimelineAsset leaveTimeline;
        [SerializeField] private Image fadeImage;


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
            ShopType shopType = GetShopType(typeof(UpgradeUI));
            StartCoroutine(OpenMenu<UpgradeUI>(shopType));
        }

        public void OpenSellShop()
        {
            ShopType shopType = GetShopType(typeof(SellShopUI));
            StartCoroutine(OpenMenu<SellShopUI>(shopType));
        }

        public void GoToSea()
        {
            EventManager.OnLeftShore();
        }
        
        private IEnumerator OpenMenu<T>(ShopType type) where T : View
        {
            if (type.camera != null)
            {
                type.camera.Priority = 3;
            }

            truckRenderer.material = type.truckMaterial;
            director.playableAsset = arriveTimeline;
            director.Play();
            yield return new WaitForSeconds((float)director.duration + 0.4f);
            ViewManager.ShowView<T>();
            activeViewType = typeof(T);
        }

        private void CheckActiveView(View closedView)
        {
            if (closedView.GetType() != activeViewType) return;
            ShopType type = GetShopType(activeViewType);
            if (type.camera != null)
            {
                type.camera.Priority = -10;
            }

            director.playableAsset = leaveTimeline;
            director.Play();
            activeViewType = null;
        }

        private ShopType GetShopType(Type shopType)
        {
            foreach (ShopType type in shopTypes)
            {
                if (type.shopName == shopType.Name)
                {
                    return type;
                }
            }

            return null;
        }
    }
}