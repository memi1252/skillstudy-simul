using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleDestroy : MonoBehaviour
{
    public bool destory = false;
    public ParticleSystem ParticleSystem;

    private void Awake()
    {
        ParticleSystem = GetComponentInChildren<ParticleSystem>();
    }

    void Start()
    {
        if (destory)
        {
            Destroy(gameObject, 2);
            ParticleSystem.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
