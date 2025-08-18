using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp;
    public float maxHp;
    public int giveEx;
    public GameObject[] itmes;
    private ParticleSystem ParticleSystem;

    private void Awake()
    {
        ParticleSystem = GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {
        hp = maxHp;
    }

    public Vector2 TakeDamage(float damage)
    {
        hp -= damage;
        if(ParticleSystem != null)
        {
            ParticleSystem.Play();
        }
        if(hp < 0)
        {
            var item = Instantiate(itmes[Random.Range(0, itmes.Length)]);
            item.transform.position = new Vector3(transform.position.x, item.transform.position.y, transform.position.z);
            Destroy(gameObject);
            
        }
        return new Vector2(giveEx, hp);
    }
}
