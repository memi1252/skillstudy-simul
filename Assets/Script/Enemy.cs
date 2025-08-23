using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp;
    public float maxHp;
    public int giveEx;
    public float speed = 4;
    public GameObject[] itmes;
    public bool isStun;
    public float currentStunTime;
    private ParticleSystem ParticleSystem;
    private bool speedDown;
    private float orizinSpeed;

    private void Awake()
    {
        ParticleSystem = GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {
        hp = maxHp;
    }

    private void Update()
    {
        if (isStun)
        {
            //2초간 움직일수없고, 공격불가
            currentStunTime += Time.deltaTime;
            if(currentStunTime > 2)
            {
                currentStunTime = 0;
                isStun = false;
            }
        }
        if(hp <= 0)
        {
            var item = Instantiate(itmes[Random.Range(0, itmes.Length)]);
            item.transform.position = new Vector3(transform.position.x, item.transform.position.y, transform.position.z);
            Destroy(gameObject);
        }
    }

    public Vector2 TakeDamage(float damage)
    {
        hp -= damage;
        if(ParticleSystem != null)
        {
            ParticleSystem.Play();
        }
        return new Vector2(giveEx, hp);
    }

    public void SpeedDown(float persent)
    {
        if (!speedDown)
        {
            speedDown = true;
            orizinSpeed = speed;
            speed -= speed * persent;
        }
        
    }

    public void OrizinSpeed()
    {
        speedDown = false;
        speed = orizinSpeed;
    }
}
