using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum playerStats
{
    near,
    far,
    magic
}

public class Player : MonoBehaviour
{
    public playerStats stats;
    public bool root;
    public int level;
    public float ex;
    public float maxEx;
    public float hp;
    public float maxHp;
    public float mp;
    public float maxMp;
    public int attackDamage;
    public float attackSpeed;
    public int criticalDamagePersent;
    public float attackRange;
    public int inventoryCount;
    public int maxInventoryCount;
    public int speed;
    public Transform target;
    public Image hpSlider;
    public Image mpSlider;
    public Image exSlider;
    public Transform pos1;
    public Transform pos2;
    public GameObject selectUI;
    public float enemyGamegiRange;
    public Vector3 dir;
    public GameObject bulletPrefab;
    public Transform firePose;

    private Rigidbody rb;
    private Animator anim;

    private float currentAttackSpeed;

    private bool attack;
    private bool follow = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }


    private void Start()
    {
        switch (stats)
        {
            case playerStats.near:
                maxHp = 100;
                maxMp = 50;
                attackDamage = 20;
                attackRange = 3;
                maxInventoryCount = 4;
                criticalDamagePersent = 10;
                break;
            case playerStats.far:
                maxHp = 80;
                maxMp = 70;
                attackDamage = 15;
                attackRange = 6;
                maxInventoryCount = 4;
                criticalDamagePersent = 20;
                break;
            case playerStats.magic:
                maxHp = 70;
                maxMp = 150;
                attackDamage = 10;
                attackRange = 6;
                maxInventoryCount = 4;
                criticalDamagePersent = 0;
                break;
        }
        hp = maxHp;
        mp = maxMp;
    }

    private void Update()
    {
        exSlider.fillAmount = ex / maxEx;
        hpSlider.fillAmount = hp / maxHp;
        mpSlider.fillAmount = mp / maxMp;
        if (root)
        {
            selectUI.SetActive(true);
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            dir = new Vector3(h, 0, v);
            if(rb.velocity == Vector3.zero)
            {
                anim.SetBool("Move", false);
                anim.SetBool("Run", false);
            }
            else
            {
                anim.SetBool("Move", true);
            }
            Vector3 dirLook = transform.forward * dir.z + transform.right * dir.x;
            dirLook.Normalize();


            float currentSpeed = rb.velocity.magnitude;
            float currentSpeedMax = 8;


            if (Input.GetKey(KeyCode.LeftShift))
            {
                anim.SetBool("Run", true);
            }
            else
            {
                anim.SetBool("Run", false);
            }

            if (currentSpeed < currentSpeedMax)
            {
                rb.AddForce(dirLook * speed * 100f * Time.fixedDeltaTime, ForceMode.Force);
            }

        }
        else
        {
            selectUI.SetActive(false);
            if (follow)
            {
                switch (stats)
                {
                    case playerStats.near:
                        foreach (var playerObj in GameObject.FindGameObjectsWithTag("Player"))
                        {

                            Player player;
                            if (playerObj.TryGetComponent<Player>(out player))
                            {
                                if (!player.root) continue;
                                if (player.stats == playerStats.far)
                                {
                                    if (Vector3.Distance(transform.position, player.pos1.position) > 1.5f)
                                    {
                                        transform.LookAt(player.pos1);
                                        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                                        transform.Translate(Vector3.forward * speed * Time.deltaTime);
                                        anim.SetBool("Move", true);
                                    }
                                    else
                                    {
                                        anim.SetBool("Move", false);
                                    }
                                    if (Vector3.Distance(transform.position, player.pos1.position) > 12)
                                    {
                                        transform.position = player.pos1.position;
                                    }

                                }
                                if (player.stats == playerStats.magic)
                                {
                                    if (Vector3.Distance(transform.position, player.pos2.position) > 1.5f)
                                    {
                                        transform.LookAt(player.pos2);
                                        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                                        transform.Translate(Vector3.forward * speed * Time.deltaTime);
                                        anim.SetBool("Move", true);
                                    }
                                    else
                                    {
                                        anim.SetBool("Move", false);
                                    }

                                    if (Vector3.Distance(transform.position, player.pos2.position) > 12)
                                    {
                                        transform.position = player.pos2.position;
                                    }
                                }

                            }
                        }
                        break;
                    case playerStats.far:
                        foreach (var playerObj in GameObject.FindGameObjectsWithTag("Player"))
                        {
                            Player player;
                            if (playerObj.TryGetComponent<Player>(out player))
                            {
                                if (!player.root) continue;
                                if (player.stats == playerStats.near)
                                {
                                    if (Vector3.Distance(transform.position, player.pos1.position) > 1.5f)
                                    {
                                        transform.LookAt(player.pos1);
                                        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                                        transform.Translate(Vector3.forward * speed * Time.deltaTime);
                                        anim.SetBool("Move", true);
                                    }
                                    else
                                    {
                                        anim.SetBool("Move", false);
                                    }

                                    if (Vector3.Distance(transform.position, player.pos1.position) > 12)
                                    {
                                        transform.position = player.pos1.position;
                                    }
                                }
                                if (player.stats == playerStats.magic)
                                {
                                    if (Vector3.Distance(transform.position, player.pos1.position) > 1.5f)
                                    {
                                        transform.LookAt(player.pos1);
                                        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                                        transform.Translate(Vector3.forward * speed * Time.deltaTime);
                                        anim.SetBool("Move", true);
                                    }
                                    else
                                    {
                                        anim.SetBool("Move", false);
                                    }

                                    if (Vector3.Distance(transform.position, player.pos1.position) > 12)
                                    {
                                        transform.position = player.pos1.position;
                                    }
                                }
                            }
                        }
                        break;
                    case playerStats.magic:
                        foreach (var playerObj in GameObject.FindGameObjectsWithTag("Player"))
                        {
                            Player player;
                            if (playerObj.TryGetComponent<Player>(out player))
                            {
                                if (!player.root) continue;
                                if (player.stats == playerStats.far)
                                {
                                    if (Vector3.Distance(transform.position, player.pos2.position) > 1.5f)
                                    {
                                        transform.LookAt(player.pos2);
                                        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                                        transform.Translate(Vector3.forward * speed * Time.deltaTime);
                                        anim.SetBool("Move", true);
                                    }
                                    else
                                    {
                                        anim.SetBool("Move", false);
                                    }

                                    if (Vector3.Distance(transform.position, player.pos2.position) > 12)
                                    {
                                        transform.position = player.pos2.position;
                                    }
                                }
                                if (player.stats == playerStats.near)
                                {
                                    if (Vector3.Distance(transform.position, player.pos2.position) > 1.5f)
                                    {
                                        transform.LookAt(player.pos2);
                                        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                                        transform.Translate(Vector3.forward * speed * Time.deltaTime);
                                        anim.SetBool("Move", true);
                                    }
                                    else
                                    {
                                        anim.SetBool("Move", false);
                                    }
                                    if (Vector3.Distance(transform.position, player.pos2.position) > 12)
                                    {
                                        transform.position = player.pos2.position;
                                    }
                                }
                            }
                        }
                        break;
                }
            }
        }
        Attack();
        if (attack)
        {
            currentAttackSpeed += Time.deltaTime;
            if(currentAttackSpeed >= attackSpeed)
            {
                currentAttackSpeed = 0;
                attack = false;
            }
        }

        if(ex >= maxEx)
        {
            ex -= maxEx;
            level ++;
            maxEx += 20;
        }
    }

    private void Attack()
    {
        if (root)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack2();
            }
        }
        else
        {
            if (target!=null)
            {
                //ai¸ðµå
                float dis = Vector3.Distance(transform.position, target.position);
                if (dis <= enemyGamegiRange)
                {
                    float dis2 = Vector3.Distance(transform.position, target.position);
                    if(dis <= attackRange)
                    {
                        Attack2();
                        transform.LookAt(target.position);
                        anim.SetBool("Move", false);
                        foreach (var playerObj in GameObject.FindGameObjectsWithTag("Player"))
                        {
                            Player player;
                            if (playerObj.TryGetComponent<Player>(out player))
                            {
                                if (!player.root) continue;
                                if(Vector3.Distance(transform.position, playerObj.transform.position) > 12)
                                {
                                    follow = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        follow = false;
                        anim.SetBool("Move", true);
                        transform.LookAt(target.position);
                        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                        transform.Translate(Vector3.forward * speed * Time.deltaTime);
                    }
                        
                    
                }
                else
                {
                    follow = true;
                }
            }
            else
            {
                follow = true;
            }
            
        }
    }

    public void Attack2()
    {
        if (attack) return;
        attack = true;
        anim.SetTrigger("Attack");
        switch (stats)
        {
            case playerStats.near:
                transform.GetComponentInChildren<TrailRenderer>().time = 0.8f;
                StartCoroutine(TrailReset());
                if (target != null)
                {
                    Enemy enemy;
                    if (target.TryGetComponent<Enemy>(out enemy))
                    {
                        float dis = Vector3.Distance(transform.position, target.position);
                        if (dis <= attackRange)
                        {
                            Vector2 exs = enemy.TakeDamage(attackDamage);
                            if (exs.y <= 0)
                            {
                                ex += exs.x / 10;
                            }
                            else if (attackDamage >= maxHp)
                            {
                                ex += exs.x * hp / maxHp;
                            }
                            else
                            {
                                ex += exs.x * attackDamage / maxHp;
                            }
                        }
                    }
                }
                break;
            case playerStats.far:
            case playerStats.magic:
                var bullet = Instantiate(bulletPrefab);
                bullet.transform.position = firePose.position;
                bullet.transform.eulerAngles = firePose.eulerAngles;
                bullet.GetComponent<Bullet>().Set(this);
                break;
        }
    }

    IEnumerator TrailReset()
    {
        yield return new WaitForSeconds(1);
        transform.GetComponentInChildren<TrailRenderer>().time = 0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyGamegiRange);
    }
}
