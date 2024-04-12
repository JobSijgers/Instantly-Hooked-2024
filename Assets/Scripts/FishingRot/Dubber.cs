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
    [SerializeField] private Vector3 offset;
    private FisherMan Fisher;
    private float timer;
    float speed;
    private void Start()
    {
        Fisher = FindObjectOfType<FisherMan>();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.UpArrow))
        {
            if (bobber.state == BobberState.Caught)
            {
                timer += Time.deltaTime * MoveSpeed;
                speed = MoveSpeed - timer;
            }
            else
            {
                speed = MoveSpeed;
                timer = 0;
            }

            if (speed <= 0 && speed >= -1)
            {
                material = GetMaterial(true);
                TrackBobberBehaviour fish = bobber.GetComponentInChildren<TrackBobberBehaviour>();
                fish.InvokeFishCaught();
            }
            Hook.useGravity = false;
            
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
            transform.position = Hook.transform.position + offset;
        }
    }
    private Material GetMaterial(bool state)
    {
        Material mat = material;

        if (state)
        {
            mat.color = new Color(255, 0, 0, 0.6f);
        }
        else mat.color = new Color(0, 0, 0, 0.6f);
        return mat;
    }
}
