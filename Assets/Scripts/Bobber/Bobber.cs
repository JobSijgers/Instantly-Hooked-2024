using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BobberState
{
    Docked,
    Fishing,
    Caught
}
public class Bobber : MonoBehaviour
{
    private BoatFish boat;
    public BobberState state;
    private void Start()
    {
        boat = BoatFish.Instance;

        boat.OnFishCaught += FishCaught;
    }
    private void FishCaught()
    {
        state = BobberState.Docked;
    }
    private void OnTriggerExit(Collider other) {  if (other.GetComponent<WaterTag>()) { if (state != BobberState.Caught) state = BobberState.Docked; } }
    private void OnTriggerEnter(Collider other) {  if (other.GetComponent<WaterTag>()) { if (state != BobberState.Caught) state = BobberState.Fishing; } }
}
