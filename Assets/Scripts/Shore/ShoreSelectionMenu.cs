using System;
using System.Collections;
using Cinemachine;
using Events;
using ShopScripts;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
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
            public GameObject truckMaterial;
        }

        [SerializeField] private ShopType[] shopTypes;
        [SerializeField] private TimelineAsset arriveTimeline;
        [SerializeField] private TimelineAsset leaveTimeline;
        private PlayableDirector director;
        private Type activeViewType;
        private Coroutine openMenuCoroutine;

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
            if (openMenuCoroutine != null)
                return;

            ShopType shopType = GetShopType(typeof(UpgradeUI));
            openMenuCoroutine = StartCoroutine(OpenMenu<UpgradeUI>(shopType));
        }

        public void OpenSellShop()
        {
            if (openMenuCoroutine != null)
                return;
            
            ShopType shopType = GetShopType(typeof(SellShopUI));
            openMenuCoroutine = StartCoroutine(OpenMenu<SellShopUI>(shopType));
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

            GameObject truck = Instantiate(type.truckMaterial, Vector3.zero, quaternion.identity);
            director = truck.GetComponent<PlayableDirector>();
            director.playableAsset = arriveTimeline;
            director.Play();
            yield return new WaitForSeconds((float)director.duration + 0.4f);
            ViewManager.ShowView<T>();
            activeViewType = typeof(T);

            openMenuCoroutine = null;
        }

        private void CheckActiveView(View closedView)
        {
            if (closedView.GetType() != activeViewType) return;
            ShopType type = GetShopType(activeViewType);
            if (type.camera != null)
            {
                type.camera.Priority = -10;
            }
            ParticleSystem[] particles = director.gameObject.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem particle in particles)
            {
                particle.Play();
            }
            director.playableAsset = leaveTimeline;
            director.Play();
            Destroy(director.gameObject, (float)director.duration);
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