using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackBobberBehaviour : MonoBehaviour, IFishAI
{
    private float speed;
    private Coroutine moveCoroutine;
    private FishBrain brain;
    private void Start()
    {
        brain = GetComponent<FishBrain>();
    }
    public void Initialize(FishData data)
    {
        speed = data.moveSpeed;
        brain.bobber.state = BobberState.Caught;
    }

    public (IFishAI, bool) switchState()
    {
        return (this, false);
    }

    public void UpdateState(FishManager FM)
    {
        if (moveCoroutine == null) moveCoroutine = StartCoroutine(Move());
    }
    private IEnumerator Move()
    {
        Vector3 startPos = transform.position;
        float distance = Vector3.Distance(startPos, brain.bobber.transform.position);
        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * speed / distance;
            float prc = Mathf.Clamp01(t);

            transform.position = Vector3.Lerp(startPos, brain.bobber.transform.position, prc);
            transform.LookAt(brain.bobber.transform.position);

            yield return null;
        }
        if (distance < 0.1f)
        {
            transform.position = brain.bobber.transform.position;
            transform.parent = brain.bobber.transform;
        }
        moveCoroutine = null;
    }
}
