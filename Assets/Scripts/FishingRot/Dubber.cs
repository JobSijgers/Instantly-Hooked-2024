using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kurwa : MonoBehaviour
{
    [SerializeField] private Rigidbody Hook;
    [SerializeField] private GameObject Originpoint;
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float distanceToRot;
    private FisherMan Fisher;
    private void Start()
    {
        Fisher = FindObjectOfType<FisherMan>();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            Hook.useGravity = false;
            transform.position = Vector3.MoveTowards(transform.position, Originpoint.transform.position, MoveSpeed * Time.deltaTime);
            Hook.transform.position = transform.position;
        }
        else transform.position = Hook.transform.position;
        if (Vector3.Distance(transform.position, Originpoint.transform.position) < distanceToRot)
        {
            Hook.useGravity = true;
            Hook.isKinematic = false;
            Fisher.LineState = FishLineState.linein;
        }
    }
}
