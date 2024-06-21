using System;
using System.Collections.Generic;
using Catalogue;
using Events;
using Quests;
using Quests.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;
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
        
        [Serializable]
        public class CatchFailedTutorialEvent
        {
            public TutorialData tutorialData;
            public View viewOnClose;
        }
        
        [Serializable]
        public class QuestCompletedTutorialEvent
        {
            public Quest quest;
            public TutorialData tutorialData;
            public View viewOnClose;
        }
        
        [Serializable]
        public class StartTutorialEvent
        {
            public TutorialData tutorialData;
            public View viewOnClose;
        }
        
        [SerializeField] private List<ViewEventTutorialEvent> tutorialEvents;
        [SerializeField] private List<FishCaughtTutorialEvent> fishCaughtTutorialEvents;
        [SerializeField] private List<TimeTutorialEvent> timeTutorialEvents;
        [SerializeField] private List<CatchFailedTutorialEvent> catchFailedTutorialEvents;
        [SerializeField] private List<QuestCompletedTutorialEvent> questCompletedTutorialEvents;
        [FormerlySerializedAs("startTutorialEvents")] [SerializeField] private StartTutorialEvent startTutorialEvent;
        [SerializeField] private TutorialPopup popup;
        
        private void Start()
        {
            ViewManager.instance.ViewShow += OnViewShow;
            CatalogueTracker.Instance.catalogueUpdated += OnFishCaught;
            EventManager.TimeUpdate += OnTimeUpdate;
            EventManager.QuestCompleted += OnQuestCompleted;
            EventManager.ReelFailed += OnReelFailed;
            if (startTutorialEvent.tutorialData != null)
            {
                ShowTutorial(startTutorialEvent.tutorialData, startTutorialEvent.viewOnClose, false);
            }
        }
        

        private void OnDestroy()
        {
            ViewManager.instance.ViewShow -= OnViewShow;
            CatalogueTracker.Instance.catalogueUpdated -= OnFishCaught;
            EventManager.TimeUpdate -= OnTimeUpdate;
            EventManager.QuestCompleted -= OnQuestCompleted;
            EventManager.ReelFailed -= OnReelFailed;
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
        
        private void OnReelFailed()
        {
            foreach (CatchFailedTutorialEvent catchFailedTutorialEvent in catchFailedTutorialEvents)
            {
                ShowTutorial(catchFailedTutorialEvent.tutorialData, catchFailedTutorialEvent.viewOnClose, false);
                catchFailedTutorialEvents.Remove(catchFailedTutorialEvent);
                break;
            }
        }
        
        private void OnQuestCompleted(QuestProgress questProgress)
        {
            foreach (QuestCompletedTutorialEvent questCompletedTutorialEvent in questCompletedTutorialEvents)
            {
                if (questCompletedTutorialEvent.quest != questProgress.quest) continue;
                
                ShowTutorial(questCompletedTutorialEvent.tutorialData, questCompletedTutorialEvent.viewOnClose, true);
                questCompletedTutorialEvents.Remove(questCompletedTutorialEvent);
                break;
            }
        }
        
        private void ShowTutorial(TutorialData tutorialData, View viewOnClose, bool saveHistory)
        {
            ViewManager.ShowView(popup, saveHistory);
            popup.Init(tutorialData, viewOnClose);
        }
    }
}