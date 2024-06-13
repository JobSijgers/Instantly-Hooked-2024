using Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Upgrades.Scriptable_Objects;

public class FishUpgradeCheck : MonoBehaviour
{
    public static FishUpgradeCheck instance;
    [Header("hoeft niet veranderd te worden")]
    public float staminaDrainUpgradePower;
    public float BiteMultiply;
    void Awake()
    {
        instance = this;
        EventManager.UpgradeBought += OnBaitBought;
    }
    public void OnBaitBought(Upgrade upgrade)
    {
        switch (upgrade)
        {
            case HookUpgrade hookupgrade:
                BiteMultiply = hookupgrade.BiteMultiply;
                staminaDrainUpgradePower = hookupgrade.StaminaDrain;
                break;
        }
    }
    private void OnDestroy()
    {
            EventManager.UpgradeBought -= OnBaitBought;
    }
}
