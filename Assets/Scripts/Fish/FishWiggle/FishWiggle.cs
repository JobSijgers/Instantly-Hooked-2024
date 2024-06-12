using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishWiggle : MonoBehaviour
{
    private FishBrain brain;

    [Header("wiggle")]
    private Vector3[] wiggleRot;
    [SerializeField] private float wiggelAngle;
    [SerializeField] private float fastWiggelSpeed;
    [SerializeField] private float wiggleSpeed;
    [SerializeField] private float widerWiggleAngle;

    void Start()
    {
        brain = GetComponent<FishBrain>();
        wiggleRot = new Vector3[4];
        wiggleRot[0].y = brain.innerVisual.transform.eulerAngles.y - wiggelAngle / 2;
        wiggleRot[1].y = brain.innerVisual.transform.eulerAngles.y + wiggelAngle / 2;

        wiggleRot[2].y = brain.innerVisual.transform.eulerAngles.y - widerWiggleAngle / 2;
        wiggleRot[3].y = brain.innerVisual.transform.eulerAngles.y + widerWiggleAngle / 2;
    }
    void Update()
    {
        Wiggle();
    }
    private void Wiggle()
    {
        float time = Time.time * wiggleSpeed;
        Vector3 startrotation;
        Vector3 endrotation;

        if (!brain.states.Biting.IsInWater() || brain.states.Biting.IsFishStruggeling())
        {
            time *= fastWiggelSpeed;
            startrotation = wiggleRot[2];
            endrotation = wiggleRot[3];
        }
        else
        {
            startrotation = wiggleRot[0];
            endrotation = wiggleRot[1];
        }

        float p = Mathf.PingPong(time, 1);
        Vector3 newrot = Vector3.Lerp(startrotation,endrotation, p);
        brain.innerVisual.transform.localEulerAngles = newrot;
    }
}