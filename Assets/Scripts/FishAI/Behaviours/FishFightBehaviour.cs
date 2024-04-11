using System.Collections;
using System.Collections.Generic;
using Fish;
using UnityEngine;

public class FishFightBehaviour : MonoBehaviour, IFishAI
{
    private FishBrain brain;
    private float speed;
    private float hookDistance;
    [SerializeField] private GameObject particle;
    private void Start()
    {
        brain = GetComponent<FishBrain>();
        brain.states.trackBobber.OnFishCaught += SetHookStartPos;
    }
    public void Initialize(FishData data)
    {
        speed = data.moveSpeed;
        GameObject go = Instantiate(particle);
        Destroy(go, 1f);
    }
    private void SetHookStartPos()
    {
        hookDistance = Vector3.Distance(brain.bobber.transform.parent.position, brain.bobber.transform.position);
    }
    public (IFishAI, bool) switchState()
    {
        float newDistance = Vector3.Distance(brain.bobber.transform.parent.position, brain.bobber.transform.position);
        Debug.Log(brain.states.roaming.spotDistance);
        if (newDistance - hookDistance > brain.states.roaming.spotDistance)
        {
            transform.parent = null;
            return (brain.states.roaming, true);
        }
        return (this, false);
    }

    public void UpdateState(FishManager FM)
    {
        brain.bobber.transform.Translate(Vector3.right * speed * 2 * Time.deltaTime, Space.World);
    }
}
