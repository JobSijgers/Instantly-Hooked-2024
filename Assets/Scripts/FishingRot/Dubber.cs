using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Kurwa : MonoBehaviour
{
    [SerializeField] private Rigidbody Hook;
    [SerializeField] private Bobber bobber;
    [SerializeField] private GameObject Originpoint;
    [SerializeField] private float MoveSpeed;
    [SerializeField] private Material material;
    private FisherMan Fisher;
    private float timer;
    private void Start()
    {
        Fisher = FindObjectOfType<FisherMan>();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.UpArrow))
        {
            if (bobber.state == BobberState.Caught)timer += Time.deltaTime * MoveSpeed;
            Hook.useGravity = false;

            float speed = MoveSpeed - timer;
            if (speed <= 0)
            {
                material = GetMaterial(true);
                TrackBobberBehaviour fish = bobber.GetComponentInChildren<TrackBobberBehaviour>();
                fish.InvokeFishCaught();
            }
            transform.position = Vector3.MoveTowards(transform.position, Originpoint.transform.position, speed * Time.deltaTime);
            Hook.transform.position = transform.position;
            if (Vector3.Distance(transform.position, Originpoint.transform.position) < Fisher.DistanceToRot)
            {
                Hook.useGravity = true;
                Hook.isKinematic = false;
                Fisher.LineState = FishLineState.linein;
            }
        }
        else
        {
            timer -= Time.deltaTime * MoveSpeed * 3;
            timer = Mathf.Clamp(timer, 0, 10);
            material = GetMaterial(false);
            transform.position = Hook.transform.position;
        }
    }
    private Material GetMaterial(bool state)
    {
        Material mat = material;

        if (state)
        {
            mat.color = Color.red;
        }
        else mat.color = Color.black;
        return mat;
    }
}
