using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishUI : MonoBehaviour
{
    private FishBrain Brain;

    [Header("UI")]
    [SerializeField] private Image StaminaMeter;
    private GameObject StaminaMeterObject;

    private void Awake()
    {
        Brain = GetComponent<FishBrain>();
        StaminaMeterObject = StaminaMeter.gameObject;
    }

    void Update()
    {
        Brain.states.Biting.GetStaminaStats(out float stamina, out float maxstamina);
        StaminaMeter.fillAmount = stamina / maxstamina;
    }
    public void ActiceState(bool state)
    {
        switch (state)
        {
            case true:
                StaminaMeterObject.SetActive(true);
                break;

            case false:
                StaminaMeterObject.SetActive(false);
                break;
        }
    }
    private void OnDisable()
    {
        ActiceState(false);
    }
}
