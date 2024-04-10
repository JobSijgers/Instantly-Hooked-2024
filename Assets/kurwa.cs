using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kurwa : MonoBehaviour
{
    [SerializeField] private GameObject Hook;
    void Update()
    {
        transform.position = Hook.transform.position;
    }
}
