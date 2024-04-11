using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kurwa : MonoBehaviour
{
    [SerializeField] private Rigidbody Hook;
    [SerializeField] private GameObject Originpoint;
    [SerializeField] private float MoveSpeed;
    private FisherMan Fisher;
    private void Start()
    {
        Fisher = FindObjectOfType<FisherMan>();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.UpArrow))
        {
            Hook.useGravity = false;
            transform.position = Vector3.MoveTowards(transform.position, Originpoint.transform.position, MoveSpeed * Time.deltaTime);
            Hook.transform.position = transform.position;
            if (Vector3.Distance(transform.position, Originpoint.transform.position) < Fisher.DistanceToRot)
            {
                Hook.useGravity = true;
                Hook.isKinematic = false;
                Fisher.LineState = FishLineState.linein;
            }
        }
        else transform.position = Hook.transform.position;
    }
}
