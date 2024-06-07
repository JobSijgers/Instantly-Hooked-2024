using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishUI : MonoBehaviour
{
    private FishBrain Brain;

    [Header("UI")]
    [SerializeField] private Image StaminaMeter;
 
    void Start()
    {
        Brain = GetComponent<FishBrain>();
    }

    void Update()
    {
        Brain.states.Biting.GetStaminaStats(out float stamina, out float maxstamina);
        StaminaMeter.fillAmount = stamina / maxstamina;
    }
}
