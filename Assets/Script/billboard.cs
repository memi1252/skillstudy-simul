using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class billboard : MonoBehaviour
{
    public Transform target;
    void Start()
    {
        if(target == null)
        {
            target = Camera.main.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            transform.rotation = Quaternion.LookRotation(target.forward, target.up);
        }
    }
}
