using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] private float FollowSpeed;
    [SerializeField] private float yOffset;
    [SerializeField] private float ZOffset;
    [SerializeField] private Transform target;

    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffset, -transform.position.z + ZOffset);
        transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
    }
}
