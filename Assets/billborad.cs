using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class billborad : MonoBehaviour
{
    public Transform Target;

    void Start()
    {

        Target = Camera.main.gameObject.transform;
    }

    void Update()
    {

        transform.rotation = Quaternion.LookRotation(Target.forward, Target.up);
    }
}
