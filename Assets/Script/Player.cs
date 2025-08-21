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

    private SkillManager SM;

    public Collider[] nearCollision;






    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }


    private void Start()
    {
        SM = SkillManager.instance;
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
        StartCoroutine(MpAddOneSecond());
    }

    private void Update()
    {
        nearCollision = Physics.OverlapSphere(transform.position, attackRange, LayerMask.GetMask("Enemy"));
        exSlider.fillAmount = ex / maxEx;
        hpSlider.fillAmount = hp / maxHp;
        mpSlider.fillAmount = mp / maxMp;
        Move();
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
            maxEx += 15;
            switch (stats)
            {
                case playerStats.near:
                    maxHp += 20;
                    hp = maxHp;
                    attackDamage += 10;
                    SkillManager.instance.nearSkillUpgrade++;
                    break;
                case playerStats.far:
                    maxHp += 15;
                    hp = maxHp;
                    attackDamage += 8;
                    SkillManager.instance.farSkillUpgrade++;
                    break;
                case playerStats.magic:
                    maxHp += 10;
                    hp = maxHp;
                    attackDamage += 10;
                    SkillManager.instance.magicSkillUpgrade++;
                    break;
            }
        }
    }

    public void NearSkill()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if(SM.nearSkillUpgrade > 0)
            {
                SM.nearSkillLevel[0]++;
                SM.rockNearSkill[0] = false;
                SM.nearSkillUpgrade--;
                return;
            }
            if (!SM.useNearSkill[0] && !SM.rockNearSkill[0])
            {
                if (SM.nearSkill[0].mp[SM.nearSkillLevel[0]] <= mp)
                {
                    mp -= SM.nearSkill[0].mp[SM.nearSkillLevel[0]];
                }
                else
                {
                    GameManager.Instance.messageUI.Add("마나 부족");
                    return;
                }
                if (SM.nearSkillLevel[0] == 0) return; 
                SM.useNearSkill[0] = true;
                anim.SetTrigger("Attack");
                transform.GetComponentInChildren<TrailRenderer>().time = 0.8f;
                StartCoroutine(TrailReset());
                if (nearCollision.Length != 0)
                {
                    Enemy enemy;
                    if (nearCollision[0].TryGetComponent<Enemy>(out enemy))
                    {
                        float dis = Vector3.Distance(transform.position, nearCollision[0].transform.position);
                        if (dis <= attackRange)
                        {
                            Vector2 exs = new Vector2();
                            switch (SM.nearSkillLevel[0])
                            {
                                case 1:
                                    exs = enemy.TakeDamage(attackDamage * 1.2f);
                                    break;
                                case 2:
                                    exs = enemy.TakeDamage(attackDamage * 1.4f);
                                    break;
                                case 3:
                                    exs = enemy.TakeDamage(attackDamage * 1.6f);
                                    break;
                                case 4:
                                    exs = enemy.TakeDamage(attackDamage * 1.8f);
                                    break;
                                case 5:
                                    exs = enemy.TakeDamage(attackDamage * 2f);
                                    break;
                            }
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
                GameManager.Instance.messageUI.Add("단일공격");
            }
        }else if (Input.GetKeyDown(KeyCode.Q))
        {
            if (SM.nearSkillUpgrade > 0)
            {
                SM.rockNearSkill[1] = false;
                SM.nearSkillLevel[1]++;
                SM.nearSkillUpgrade--;
                return;
            }
            if (!SM.useNearSkill[1] && !SM.rockNearSkill[1])
            {
                if (SM.nearSkill[1].mp[SM.nearSkillLevel[1]] <= mp)
                {
                    mp -= SM.nearSkill[1].mp[SM.nearSkillLevel[1]];
                }
                else
                {
                    GameManager.Instance.messageUI.Add("마나 부족");
                    return;
                }
                if (SM.nearSkillLevel[1] == 0) return;
                SM.useNearSkill[1] = true;
                anim.SetTrigger("Attack");
                transform.GetComponentInChildren<TrailRenderer>().time = 0.8f;
                StartCoroutine(TrailReset());
                if (nearCollision.Length != 0)
                {
                    Enemy enemy;
                    if (nearCollision[0].TryGetComponent<Enemy>(out enemy))
                    {
                        float dis = Vector3.Distance(transform.position, nearCollision[0].transform.position);
                        if (dis <= attackRange)
                        {
                            Vector2 exs = new Vector2();
                            switch (SM.nearSkillLevel[0])
                            {
                                case 1:
                                    exs = enemy.TakeDamage(attackDamage);
                                    break;
                                case 2:
                                    exs = enemy.TakeDamage(attackDamage * 1.2f);
                                    break;
                                case 3:
                                    exs = enemy.TakeDamage(attackDamage * 1.2f);
                                    break;
                                case 4:
                                    exs = enemy.TakeDamage(attackDamage * 1.4f);
                                    break;
                                case 5:
                                    exs = enemy.TakeDamage(attackDamage * 1.4f);
                                    break;
                            }
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
                GameManager.Instance.messageUI.Add("멀티공격");
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (SM.nearSkillUpgrade > 0)
            {
                SM.nearSkillLevel[2]++;
                SM.nearSkillUpgrade--;
                SM.rockNearSkill[2] = false;
                return;
            }
            if (!SM.useNearSkill[2] && !SM.rockNearSkill[2])
            {
                if (SM.nearSkill[2].mp[SM.nearSkillLevel[2]] <= mp)
                {
                    mp -= SM.nearSkill[2].mp[SM.nearSkillLevel[2]];
                }
                else
                {
                    GameManager.Instance.messageUI.Add("마나 부족");
                    return;
                }
                if (SM.nearSkillLevel[2] == 0) return;
                SM.useNearSkill[2] = true;
                GameManager.Instance.messageUI.Add("관통공격");
            }
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            if (SM.nearSkillUpgrade > 0)
            {
                SM.nearSkillLevel[3]++;
                SM.rockNearSkill[3] = false;
                SM.nearSkillUpgrade--;
                return;
            }
            if (!SM.useNearSkill[3] && !SM.rockNearSkill[3])
            {
                if (SM.nearSkill[3].mp[SM.nearSkillLevel[3]] <= mp)
                {
                    mp -= SM.nearSkill[3].mp[SM.nearSkillLevel[3]];
                }
                else
                {
                    GameManager.Instance.messageUI.Add("마나 부족");
                    return;
                }
                if (SM.nearSkillLevel[3] == 0) return;
                SM.useNearSkill[3] = true;
                GameManager.Instance.messageUI.Add("관통공격");
            }
        }
    }


    public void Move()
    {
        if (root)
        {
            selectUI.SetActive(true);
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            dir = new Vector3(h, 0, v);
            if (rb.velocity == Vector3.zero)
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
            if (nearCollision.Length != 0)
            {
                //ai모드
                float dis = Vector3.Distance(transform.position, nearCollision[0].transform.position);
                if (dis <= enemyGamegiRange)
                {
                    float dis2 = Vector3.Distance(transform.position, nearCollision[0].transform.position);
                    if(dis <= attackRange)
                    {
                        Attack2();
                        transform.LookAt(nearCollision[0].transform.position);
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
                        transform.LookAt(nearCollision[0].transform.position);
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
                if (nearCollision.Length != 0)
                {
                    Enemy enemy;
                    if (nearCollision[0].TryGetComponent<Enemy>(out enemy))
                    {
                        float dis = Vector3.Distance(transform.position, nearCollision[0].transform.position);
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

    IEnumerator MpAddOneSecond()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            float amount = maxMp * 0.01f;
            mp += amount;
            if(mp > maxMp)
            {
                mp = maxMp;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyGamegiRange);
    }
}
