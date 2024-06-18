using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Views;

namespace Tutorial
{
    public class TutorialListener : MonoBehaviour
    {
        [Serializable]
        public class TutorialEvent
        {
            public View view;
            public TutorialData tutorialData;
        }
        
        [SerializeField] private List<TutorialEvent> tutorialEvents;
        [SerializeField] private TutorialPopup popup;

        private void Start()
        {
            ViewManager.instance.ViewShow += OnViewShow;
        }

        private void OnDestroy()
        {
            ViewManager.instance.ViewShow -= OnViewShow;
        }

        private void OnViewShow(View view)
        {
            foreach (TutorialEvent tutorialEvent in tutorialEvents)
            {
                if (tutorialEvent.view != view) continue;
                
                ViewManager.ShowView(popup, false);
                popup.Init(tutorialEvent.tutorialData, view);
                tutorialEvents.Remove(tutorialEvent);
                break;
            }
        }
    }
}