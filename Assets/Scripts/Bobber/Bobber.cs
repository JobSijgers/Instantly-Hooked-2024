using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BobberState
{
    Fishing,
    Caught
}
public class Bobber : MonoBehaviour
{
    public BobberState state;
}
