using System;
using System.Collections;
using System.Collections.Generic;
using Catalogue;
using Enums;
using Events;
using Fish;
using UnityEngine;
using Views;

namespace Tutorial
{
    public class TutorialListener : MonoBehaviour
    {
        [Serializable]
        public class ViewEventTutorialEvent
        {
            public View view;
            public TutorialData tutorialData;
        }

        [Serializable]
        public class FishCaughtTutorialEvent
        {
            public int fishIndex;
            public TutorialData tutorialData;
            public View viewOnClose;
        }

        [Serializable]
        public class TimeTutorialEvent
        {
            public int timeInMinutes;
            public TutorialData tutorialData;
            public View viewOnClose;
        }
        
        [SerializeField] private List<ViewEventTutorialEvent> tutorialEvents;
        [SerializeField] private List<FishCaughtTutorialEvent> fishCaughtTutorialEvents;
        [SerializeField] private List<TimeTutorialEvent> timeTutorialEvents;
        [SerializeField] private TutorialPopup popup;
        
        private void Start()
        {
            ViewManager.instance.ViewShow += OnViewShow;
            CatalogueTracker.Instance.catalogueUpdated += OnFishCaught;
            EventManager.TimeUpdate += OnTimeUpdate;
        }
        

        private void OnDestroy()
        {
            ViewManager.instance.ViewShow -= OnViewShow;
            CatalogueTracker.Instance.catalogueUpdated -= OnFishCaught;
            EventManager.TimeUpdate -= OnTimeUpdate;
        }

        private void OnViewShow(View view)
        {
            foreach (ViewEventTutorialEvent tutorialEvent in tutorialEvents)
            {
                if (tutorialEvent.view != view) continue;
                
                ShowTutorial(tutorialEvent.tutorialData, view, false);
                tutorialEvents.Remove(tutorialEvent);
                break;
            }
        }

        private void OnFishCaught(int fishIndex)
        {
            foreach (FishCaughtTutorialEvent tutorialEvent in fishCaughtTutorialEvents)
            {
                if (tutorialEvent.fishIndex != fishIndex) continue;
                
                ShowTutorial(tutorialEvent.tutorialData, tutorialEvent.viewOnClose, true);
                fishCaughtTutorialEvents.Remove(tutorialEvent);
                break;
            }
        }
        
        private void OnTimeUpdate(TimeSpan time)
        {
            foreach (TimeTutorialEvent timeTutorialEvent in timeTutorialEvents)
            {
                if (time.TotalMinutes >= timeTutorialEvent.timeInMinutes)
                {
                    ShowTutorial(timeTutorialEvent.tutorialData, timeTutorialEvent.viewOnClose, false);
                    timeTutorialEvents.Remove(timeTutorialEvent);
                    break;
                }
            }
        }
        
        private void ShowTutorial(TutorialData tutorialData, View viewOnClose, bool saveHistory)
        {
            ViewManager.ShowView(popup, saveHistory);
            popup.Init(tutorialData, viewOnClose);
        }
    }
}