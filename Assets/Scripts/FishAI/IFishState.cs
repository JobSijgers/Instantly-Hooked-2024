using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFishState
{
    public void UpdateState();
    public IFishState switchState();
}
