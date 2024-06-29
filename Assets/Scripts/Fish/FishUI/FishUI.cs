using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FishUI : MonoBehaviour
{
    private FishBrain brain;

    [Header("UI")] [SerializeField] private GameObject uiHolder;
    [SerializeField] private Image staminaMeter;
    [SerializeField] private GameObject tensionHandle;
    [SerializeField] private int minAngle;
    [SerializeField] private int maxAngle;
    private float time;
    
    private void Start()
    {
        brain = GetComponent<FishBrain>();
        brain.states.Biting.FishStruggleStart += EnableUI;
        brain.states.Biting.FishStruggleEnd += DisableUI;
        uiHolder.SetActive(false);
    }

    private void EnableUI()
    {
        uiHolder.SetActive(true);
    }

    private void DisableUI()
    {
        uiHolder.SetActive(false);
    }

    private void Update()
    {
        if (uiHolder.activeSelf == false)
            return;
        uiHolder.SetActive(brain.states.Biting.IsInWater());

        brain.states.Biting.GetStaminaStats(out float stamina, out float maxstamina);
        staminaMeter.fillAmount = stamina / maxstamina;

        float angle = Mathf.Lerp(minAngle, maxAngle, brain.states.Biting.GetTension());

        time += Time.deltaTime * 5;
        float noise = Mathf.PerlinNoise(time, 0);
        float jiggle = Mathf.Lerp(-7, 7, noise);
        angle += jiggle;

        tensionHandle.transform.localEulerAngles = new Vector3(0, 0, angle);
    }

    private void OnDisable()
    {
        uiHolder.SetActive(false);
    }
}