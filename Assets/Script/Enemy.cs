using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float hp;
    public float maxHp;
    public int giveEx;
    public float speed = 4;
    public float attackDamage;
    public float attackRange;
    public float attackSpeed;
    public GameObject[] itmes;
    public bool isStun;
    public float currentStunTime;
    public ParticleSystem hitParticle;
    public ParticleSystem prozenParticle;
    private bool speedDown;
    private float orizinSpeed;
    private float currentAttackSpeed;
    public Transform target;
    private Rigidbody rb;
    private Animator animator;
    public bool hit;
    public Image hpImage;
    private float hitCurrentTime;
    public bool isDie = false;
    //public bool isAttack;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        hp = maxHp;
        currentAttackSpeed = attackSpeed;
    }

    private void Update()
    {
        
        

        if (!isDie)
        {
            if (hit)
            {
                hitCurrentTime += Time.deltaTime;
                if (hitCurrentTime > 1)
                {
                    hitCurrentTime = 0;
                    hit = false;
                }
            }
            hpImage.fillAmount = hp / maxHp;
            if (isStun)
            {
                //2초간 움직일수없고, 공격불가
                currentStunTime += Time.deltaTime;
                if (currentStunTime > 2)
                {
                    currentStunTime = 0;
                    isStun = false;
                }
            }

            if (hp <= 0)
            {
                isDie = true;
                hpImage.fillAmount = 0;
                currentAttackSpeed = -9999999;
                animator.StopPlayback();
                animator.SetTrigger("Die");
                StartCoroutine(Die());
            }
        }
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(3);
        var item = Instantiate(itmes[Random.Range(0, itmes.Length)]);
        item.transform.position = new Vector3(transform.position.x, item.transform.position.y, transform.position.z);
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (isDie) return;
        if(rb.velocity == Vector3.zero)
        {
            animator.SetBool("Move", false);
        }
        else
        {
            animator.SetBool("Move", true);
        }


        if (target != null)
        {

            float dis = Vector3.Distance(transform.position, target.position);
            if (dis < attackRange)
            {
                Attack();
            }
            else
            {
                if (!hit)
                {
                    transform.LookAt(target);
                    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                    Vector3 dir = transform.forward;
                    dir.Normalize();
                    rb.velocity = dir * speed;
                }
            }
               
        }
    }

    private void Attack()
    {
        if(target != null)
        {
            if (isDie) return;
            currentAttackSpeed += Time.deltaTime;
            float dis = Vector3.Distance(transform.position, target.position);
            if (dis > attackRange)
            {
                currentAttackSpeed = 0;
            }
            if (currentAttackSpeed > attackSpeed)
            {
                currentAttackSpeed = 0;
                if(Random.Range(0, 2) == 1)
                {
                    animator.SetTrigger("RightAttack");
                }
                else
                {
                    animator.SetTrigger("LaftAttack");
                }
                    
                Player player;
                if (target.TryGetComponent<Player>(out player))
                {
                    player.TakeDamage(attackDamage);
                }
            }
        }
    }



    public Vector2 TakeDamage(float damage)
    {
        if (isDie) return Vector2.zero;
        hp -= damage;
        animator.SetTrigger("Hit");
        if (!hit)
        {
            hit = true;
        }
        else
        {
            hitCurrentTime = 0;
        }
        if (speedDown)
        {
            if (prozenParticle != null)
            {
                prozenParticle.Play();
            }
        }
        else
        {
            if (hitParticle != null)
            {
                hitParticle.Play();
            }
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
