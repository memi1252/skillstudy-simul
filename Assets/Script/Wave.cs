using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public float damage;

    private void Start()
    {
        Destroy(gameObject, 2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponent<BoxCollider>().enabled = false;
            if (collision.gameObject.TryGetComponent(out Player player))
            {
                player.TakeDamage(damage);
            }
        }
    }


}
