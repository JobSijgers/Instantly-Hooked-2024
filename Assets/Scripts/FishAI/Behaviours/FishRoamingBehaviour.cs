using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FishRoamingBehaviour : MonoBehaviour, IFishAI

{
    private float speed;
    [SerializeField] private float spotDistance;
    [SerializeField] private Vector2 moveBounds = new Vector2(10, 10);
    private FishBrain brain;

    private Coroutine timerCoroutine;
    private Coroutine moveCoroutine;
    private void Start()
    {
        brain = GetComponent<FishBrain>();
    }
    public void Initialize(FishData data)
    {
        speed = data.moveSpeed;
    }

    public (IFishAI, bool) switchState()
    {
        return (this, false);
    }

    public void UpdateState(FishManager FM)
    {
        if (moveCoroutine == null) moveCoroutine = StartCoroutine(MoveFishAsync(FM.waterMesh));

        if (CheckDistance())
        {
            float time = Random.Range(brain.FM.minFishInterestTime, brain.FM.maxFishInterestTime);
            if (timerCoroutine == null) timerCoroutine = StartCoroutine(SpotTimer(time));
        }
    }
    private bool CheckDistance()
    {
        return Vector3.Distance(transform.position, brain.bobber.transform.position) < spotDistance;
    }
    private IEnumerator MoveFishAsync(MeshRenderer waterMesh)
    {
        Vector3 waterBounds = waterMesh.bounds.max;
        Vector3 newPos = new Vector3
        {
            x = Mathf.Clamp(waterMesh.bounds.center.x + Random.Range(-waterBounds.x, waterBounds.x), transform.position.x - moveBounds.x, transform.position.x + moveBounds.x),
            y = Mathf.Clamp(waterMesh.bounds.center.y + Random.Range(-waterBounds.y, waterBounds.y), transform.position.y - moveBounds.y, transform.position.y + moveBounds.y),
            z = transform.position.z
        };

        Vector3 startPos = transform.position;
        float distance = Vector3.Distance(transform.position, newPos);
        float t = 0.0f;

        while (t < 1.0f)
        {
            t += Time.deltaTime * speed / distance;
            float prc = Mathf.Clamp01(t);

            transform.position = Vector3.Lerp(startPos, newPos, prc);
            transform.LookAt(newPos);

            yield return null;
        }
        moveCoroutine = null;
        yield return null;
    }
    private IEnumerator SpotTimer(float time)
    {
        yield return new WaitForSeconds(time);
        if (CheckDistance() && brain.bobber.state == BobberState.Fishing)
        {
            brain.currentState = brain.states.trackBobber;
            brain.currentState.Initialize(brain.data);
            StopCoroutine(moveCoroutine);
        }
        timerCoroutine = null;
        yield return null;
    }
}
