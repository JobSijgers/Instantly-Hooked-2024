﻿using System;
using Events;
using UnityEngine;
using UnityEngine.Video;

namespace Generic
{
    public class ToggleAtDay : MonoBehaviour
    {
        [SerializeField] private int dayToChangeState;
        [SerializeField] private bool defaultState;
        [SerializeField] private bool destroyOnToggle;
        private bool isEnabled;
        
        private void Start()
        {
            isEnabled = defaultState;
            gameObject.SetActive(isEnabled);
        }

        private void OnEnable()
        {
            EventManager.NewDay += ToggleActive;
            EventManager.LeftShore += CheckState;
        }

        private void OnDisable()
        {
            EventManager.NewDay -= ToggleActive;
            EventManager.LeftShore -= CheckState;
        }
        
        private void CheckState()
        {
            gameObject.SetActive(isEnabled);
        }
        
        private void ToggleActive(int newDay)
        {
            if (newDay != dayToChangeState) return;
            if (destroyOnToggle)
            {
                Destroy(gameObject);
                return;
            }
            Destroy(this);
            isEnabled = !isEnabled;
            gameObject.SetActive(isEnabled);
        }
    }
}