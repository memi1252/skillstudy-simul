using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour
{
    public float damage;
    public float speed;
    public Player player;
    public bool penetration;
    public float maxDis;

    private void Start()
    {
        Destroy(gameObject, 7);
    }

    public void Set(Player player, float damage)
    {
        this.player = player;
        this.damage = damage;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if(penetration)
        {
            if (Vector3.Distance(transform.position, player.transform.position) > maxDis)
            {
                Destroy(gameObject);
            }
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy;
            if (collision.gameObject.TryGetComponent<Enemy>(out enemy))
            {
                Vector2 exs = enemy.TakeDamage(damage);
                if (exs.y <= 0)
                {
                    player.ex += exs.x / 10;
                }
                else if (player.attackDamage >= player.maxHp)
                {
                    player.ex += exs.x * player.hp / player.maxHp;
                }
                else
                {
                    player.ex += exs.x * player.attackDamage / player.maxHp;
                }
            }
        }
        if (!penetration)
        {
            Destroy(gameObject);
        }
    }
}
