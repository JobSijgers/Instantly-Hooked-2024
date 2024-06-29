using System;
using System.Collections;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Views;

namespace ShopManager
{
    public class ShopManager : MonoBehaviour
    {
        [Serializable]
        public class ShopType
        {
            public string shopName;
            public CinemachineVirtualCamera camera;
            public PlayableDirector truck;
        }

        [SerializeField] private ShopType[] shopTypes;
        [SerializeField] private TimelineAsset arriveTimeline;
        [SerializeField] private TimelineAsset leaveTimeline;
        private PlayableDirector director;
        private View activeView;
        private Coroutine openMenuCoroutine;

        private void Start()
        {
            ViewManager.instance.ViewHide += CheckActiveView;
        }

        private void OnDestroy()
        {
            ViewManager.instance.ViewHide -= CheckActiveView;
        }

        public void OpenShop(View shop)
        {
            if (openMenuCoroutine != null)
                return;
            openMenuCoroutine = StartCoroutine(OpenMenu(shop));
        }

        private IEnumerator OpenMenu(View view)
        {
            ShopType type = GetShopType(view.GetType());
            if (type.camera != null)
            {
                type.camera.Priority = 3;
            }
            ViewManager.HideActiveView(false);
            director = Instantiate(type.truck, Vector3.zero, quaternion.identity);
            PlayTimeline(director, arriveTimeline);
            yield return new WaitForSeconds((float)director.duration + 0.4f);
            ViewManager.ShowView(view);
            activeView = view;
            openMenuCoroutine = null;
        }

        private void CheckActiveView(View closedView)
        {
            if (closedView != activeView) return;
            ShopType type = GetShopType(activeView.GetType());
            if (type.camera != null)
            {
                type.camera.Priority = -10;
            }

            ParticleSystem[] particles = director.gameObject.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem particle in particles)
            {
                particle.Play();
            }
            PlayTimeline(director, leaveTimeline);
            Destroy(director.gameObject, (float)director.duration);
            activeView = null;
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

        private void PlayTimeline(PlayableDirector dir, TimelineAsset timelineAsset)
        {
            dir.playableAsset = timelineAsset;
            dir.Play();
        }
    }
}