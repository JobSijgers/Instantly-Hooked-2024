using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FishRoamingBehaviour : MonoBehaviour, IFishAI   

{
    private float moveTime;
    [SerializeField] private Vector2 moveBounds;
    private Coroutine moveCoroutine;

    public void Initialzie(FishData data)
    {
        moveTime = data.moveSpeed;
    }

    public IFishAI switchState()
    {
        return this;
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

        float t = 0;
        Vector3 startPos = transform.position;
        while (t < moveTime)
        {
            t += Time.deltaTime * moveTime;
            float prc = t / moveTime;   
            transform.position = Vector3.Lerp(startPos, newPos, prc);
            Quaternion targetRot = Quaternion.LookRotation(transform.position - newPos);
            targetRot.x = 0;
            targetRot.y = 0;
            transform.rotation = targetRot;
            yield return null;
        }
        moveCoroutine = null;
        yield return null;
    }
}
