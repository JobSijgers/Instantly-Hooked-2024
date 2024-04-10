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
    public BobberState state;
    private void OnTriggerExit(Collider other) {  if (other.GetComponent<WaterTag>()) { if (state != BobberState.Caught) state = BobberState.Docked; } }
    private void OnTriggerEnter(Collider other) {  if (other.GetComponent<WaterTag>()) { if (state != BobberState.Caught) state = BobberState.Fishing; } }
}
