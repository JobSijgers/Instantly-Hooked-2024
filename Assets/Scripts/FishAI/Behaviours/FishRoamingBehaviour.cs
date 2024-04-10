using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FishRoamingBehaviour : MonoBehaviour, IFishAI   

{
    private float speed;
    [SerializeField] private Vector2 moveBounds = new Vector2(10, 10);
    private Coroutine moveCoroutine;
    private FishBrain brain;

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
        if (Vector3.Distance(transform.position, brain.bobber.transform.position) < 6f && brain.bobber.state == BobberState.Fishing) 
            return (brain.states.trackBobber, true);
        return (this, false);
    }

    public void UpdateState(FishManager FM)
    {
        if (moveCoroutine == null) moveCoroutine = StartCoroutine(MoveFishAsync(FM.waterMesh));
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
}
