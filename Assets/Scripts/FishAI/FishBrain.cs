using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public struct States
{
    public FishRoamingBehaviour roaming;
}
public class FishBrain : MonoBehaviour
{
    [SerializeField] float speed;

    public IFishState currentState;
    public States states;

    private void Start()
    {
        currentState = states.roaming;
    }
    private void Update()
    {
        currentState.UpdateState();
        currentState = currentState.switchState();
    }
}
