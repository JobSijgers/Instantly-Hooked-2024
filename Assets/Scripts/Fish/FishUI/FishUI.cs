using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private void Awake()
    {
        brain = GetComponent<FishBrain>();
    }

    private void Update()
    {
        ActiveState(brain.states.Biting.IsInWater());
        if (!uiHolder.activeSelf)
            return;
        bool fishOnHook = Hook.instance.FishOnHook == brain;
        ActiveState(fishOnHook);
        
        brain.states.Biting.GetStaminaStats(out float stamina, out float maxstamina);
        staminaMeter.fillAmount = stamina / maxstamina;

        float angle = Mathf.Lerp(minAngle, maxAngle, brain.states.Biting.GetTension());

        time += Time.deltaTime * 5;
        float noise = Mathf.PerlinNoise(time, 0);
        float jiggle = Mathf.Lerp(-7, 7, noise);
        angle += jiggle;

        tensionHandle.transform.localEulerAngles = new Vector3(0, 0, angle);
    }

    public void ActiveState(bool state)
    {
        switch (state)
        {
            case true:
                uiHolder.SetActive(true);
                break;

            case false:
                uiHolder.SetActive(false);
                time = 0;
                break;
        }
    }

    private void OnDisable()
    {
        ActiveState(false);
    }
}