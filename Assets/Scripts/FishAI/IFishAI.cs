using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFishAI
{
    public void UpdateState(FishManager FM);
    public (IFishAI, bool) switchState();
    public void Initialize(FishData data);
}
