using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
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
    public float bastMaxHp;
    public float mp;
    public float maxMp;
    public float attackDamage;
    public float orizinAttackDamage;
    public float attackSpeed;
    public float orizinAttackSpeed;
    public float criticalDamagePersent;
    public float attackRange;
    public int inventoryCount;
    public int maxInventoryCount;
    public float speed;
    public float orizinSpeed;
    public Image hpSlider;
    public Image mpSlider;
    public Image exSlider;
    public Text hpText;
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
    private float aiSkillAttackTime = 3;
    private float currentAiSkillAttackTime;

    private bool attack;
    private bool follow = false;
    public bool move = true;
    public bool isDie = false;

    private SkillManager SM;
    private GameManager GM;

    public Collider[] nearCollision;


    public GameObject[] nearSkillPrefabs;
    public GameObject[] farSkillPrefabs;
    public GameObject[] magicSkillPrefabs;

    public Collider En = new Collider();
    public LineRenderer LR;


    private Transform arrowDropPos;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }


    private void Start()
    {
        SM = SkillManager.instance;
        GM = GameManager.Instance;
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
        bastMaxHp = maxHp;
        orizinAttackSpeed = attackSpeed;
        orizinSpeed = speed;
        orizinAttackDamage = attackDamage;
        arrowDropPos = new GameObject().transform;
        StartCoroutine(MpAddOneSecond());
    }

    private void Update()
    {
        nearCollision = Physics.OverlapSphere(transform.position, enemyGamegiRange, LayerMask.GetMask("Enemy"));
        if (nearCollision.Length == 1)
        {
            En = nearCollision[0];
        }
        else if (nearCollision.Length != 0)
        {
            float neardis = Mathf.Infinity;
            foreach (var en in nearCollision)
            {
                float dis2 = Vector3.Distance(transform.position, en.transform.position);

                if (dis2 < neardis)
                {
                    neardis = dis2;
                    En = en;
                }
            }
        }
        if (isDie)
        {
            hpSlider.color = Color.red;
            hpSlider.fillAmount = 1;
            hpText.text = "사망";
            hpText.color = Color.red;
            exSlider.fillAmount = 0;
            mpSlider.fillAmount = 0;
        }
        else
        {
            hpSlider.fillAmount = hp / maxHp;
            hpText.text = $"{(int)hp} / {(int)maxHp}";
            exSlider.fillAmount = ex / maxEx;

            mpSlider.fillAmount = mp / maxMp;
        }
            
       
        if(hp > maxHp)
        {
            hp = maxHp;
        }
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
            maxEx += 250;
            float rando = UnityEngine.Random.value;
            if(rando < 50 / 100)
            {
                SM.passiveSkillPoint++;
            }
            switch (stats)
            {
                case playerStats.near:
                    maxHp += 20;
                    hp += maxHp/4;
                    attackDamage += 10;
                    if(level /2  == 0)
                        SkillManager.instance.nearSkillUpgrade++;
                    break;
                case playerStats.far:
                    maxHp += 15;
                    hp += maxHp / 4;
                    attackDamage += 8;
                    if (level / 2 == 0)
                        SkillManager.instance.farSkillUpgrade++;
                    break;
                case playerStats.magic:
                    maxHp += 10;
                    hp += maxHp / 4;
                    attackDamage += 10;
                    if (level / 2 == 0)
                        SkillManager.instance.magicSkillUpgrade++;
                    break;
            }
        }
    }

    public void NearSkill()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            NearSkill1();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            NearSkill2();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            NearSkill3();
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            NearSkill4();
        }
    }
    public void NearSkill1()
    {
        if (SM.nearSkillUpgrade > 0 && Input.GetKey(KeyCode.LeftControl))
        {
            if (SM.nearSkillLevel[0] == 5) return;
            SM.nearSkillLevel[0]++;
            SM.rockNearSkill[0] = false;
            SM.nearSkillUpgrade--;
            return;
        }
        if (!SM.useNearSkill[0] && !SM.rockNearSkill[0])
        {
            if (SM.nearSkill[0].mp[SM.nearSkillLevel[0] - 1] <= mp)
            {
                mp -= SM.nearSkill[0].mp[SM.nearSkillLevel[0] - 1];
            }
            else
            {
                GameManager.Instance.messageUI.Add("마나 부족");
                return;
            }
            if (SM.nearSkillLevel[0] == 0) return;
            SM.useNearSkill[0] = true;
            var skill = Instantiate(nearSkillPrefabs[0]);
            skill.transform.SetParent(transform);
            skill.transform.position = transform.position;
            skill.transform.rotation = transform.rotation;
            anim.SetTrigger("Attack");
            if (nearCollision.Length != 0)
            {
                Enemy enemy;
                if (En.TryGetComponent<Enemy>(out enemy))
                {
                    float dis = Vector3.Distance(transform.position, En.transform.position);
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
    }
    public void NearSkill2()
    {
        if (SM.nearSkillUpgrade > 0 && Input.GetKey(KeyCode.LeftControl))
        {
            if (SM.nearSkillLevel[1] == 5) return;
            SM.rockNearSkill[1] = false;
            SM.nearSkillLevel[1]++;
            SM.nearSkillUpgrade--;
            return;
        }
        if (!SM.useNearSkill[1] && !SM.rockNearSkill[1])
        {
            if (SM.nearSkill[1].mp[SM.nearSkillLevel[1] - 1] <= mp)
            {
                mp -= SM.nearSkill[1].mp[SM.nearSkillLevel[1] - 1];
            }
            else
            {
                GameManager.Instance.messageUI.Add("마나 부족");
                return;
            }
            if (SM.nearSkillLevel[1] == 0) return;
            SM.useNearSkill[1] = true;
            var skill = Instantiate(nearSkillPrefabs[1]);
            skill.transform.SetParent(transform);
            skill.transform.position = transform.position;
            skill.transform.rotation = transform.rotation;
            anim.SetTrigger("Attack");
            if (nearCollision.Length != 0)
            {
                Enemy enemy;
                if (En.TryGetComponent<Enemy>(out enemy))
                {
                    float dis = Vector3.Distance(transform.position, En.transform.position);
                    if (dis <= attackRange)
                    {
                        Vector2 exs = new Vector2();
                        switch (SM.nearSkillLevel[1])
                        {
                            case 1:
                                exs = enemy.TakeDamage(attackDamage);
                                {
                                    for (int i = 0; i < nearCollision.Length; i++)
                                    {
                                        int max = 1;
                                        if (i == max) break;
                                        Enemy enemy2;
                                        if (i < nearCollision.Length)
                                        {
                                            if (nearCollision[i].TryGetComponent<Enemy>(out enemy2))
                                            {
                                                if (enemy2 != enemy)
                                                {
                                                    exs = enemy2.TakeDamage(attackDamage);

                                                }
                                                else
                                                {
                                                    max++;
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            case 2:
                                exs = enemy.TakeDamage(attackDamage * 1.2f);
                                for (int i = 0; i < nearCollision.Length; i++)
                                {
                                    int max = 2;
                                    if (i == max) break;
                                    Enemy enemy2;
                                    if (i < nearCollision.Length)
                                    {
                                        if (nearCollision[i].TryGetComponent<Enemy>(out enemy2))
                                        {
                                            if (enemy2 != enemy)
                                            {
                                                exs = enemy2.TakeDamage(attackDamage * 1.2f);

                                            }
                                            else
                                            {
                                                max++;
                                            }
                                        }
                                    }
                                }
                                break;
                            case 3:
                                exs = enemy.TakeDamage(attackDamage * 1.2f);
                                for (int i = 0; i < nearCollision.Length; i++)
                                {
                                    int max = 2;
                                    if (i == max) break;
                                    Enemy enemy2;
                                    if (i < nearCollision.Length)
                                    {
                                        if (nearCollision[i].TryGetComponent<Enemy>(out enemy2))
                                        {
                                            if (enemy2 != enemy)
                                            {
                                                exs = enemy2.TakeDamage(attackDamage * 1.2f);

                                            }
                                            else
                                            {
                                                max++;
                                            }
                                        }
                                    }
                                }
                                break;
                            case 4:
                                exs = enemy.TakeDamage(attackDamage * 1.4f);
                                for (int i = 0; i < nearCollision.Length; i++)
                                {
                                    int max = 3;
                                    if (i == max) break;
                                    Enemy enemy2;
                                    if (i < nearCollision.Length)
                                    {
                                        if (nearCollision[i].TryGetComponent<Enemy>(out enemy2))
                                        {
                                            if (enemy2 != enemy)
                                            {
                                                exs = enemy2.TakeDamage(attackDamage * 1.4f);

                                            }
                                            else
                                            {
                                                max++;
                                            }
                                        }
                                    }
                                }
                                break;
                            case 5:
                                exs = enemy.TakeDamage(attackDamage * 1.4f);
                                for (int i = 0; i < nearCollision.Length; i++)
                                {
                                    int max = 4;
                                    if (i == max) break;
                                    Enemy enemy2;
                                    if (i < nearCollision.Length)
                                    {
                                        if (nearCollision[i].TryGetComponent<Enemy>(out enemy2))
                                        {
                                            if (enemy2 != enemy)
                                            {
                                                exs = enemy2.TakeDamage(attackDamage * 1.4f);

                                            }
                                            else
                                            {
                                                max++;
                                            }
                                        }
                                    }
                                }
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
    public void NearSkill3()
    {
        if (SM.nearSkillUpgrade > 0 && Input.GetKey(KeyCode.LeftControl))
        {
            if (SM.nearSkillLevel[2] == 5) return;
            SM.nearSkillLevel[2]++;
            SM.nearSkillUpgrade--;
            SM.rockNearSkill[2] = false;
            return;
        }
        if (!SM.useNearSkill[2] && !SM.rockNearSkill[2])
        {
            if (SM.nearSkill[2].mp[SM.nearSkillLevel[2] - 1] <= mp)
            {
                mp -= SM.nearSkill[2].mp[SM.nearSkillLevel[2] - 1];
            }
            else
            {
                GameManager.Instance.messageUI.Add("마나 부족");
                return;
            }
            if (SM.nearSkillLevel[2] == 0) return;
            SM.useNearSkill[2] = true;
            var skill = Instantiate(nearSkillPrefabs[2]);
            skill.transform.SetParent(transform);
            skill.transform.position = transform.position;
            skill.transform.rotation = transform.rotation;
            anim.SetTrigger("Attack");
            if (nearCollision.Length != 0)
            {
                Enemy enemy;
                Collider En = new Collider();
                float neardis = Mathf.Infinity;
                foreach (var en in nearCollision)
                {
                    float dis2 = Vector3.Distance(transform.position, en.transform.position);

                    if (dis2 < neardis)
                    {
                        neardis = dis2;
                        En = en;
                    }
                }
                if (En.TryGetComponent<Enemy>(out enemy))
                {
                    float dis = Vector3.Distance(transform.position, En.transform.position);
                    if (dis <= attackRange)
                    {
                        Vector2 exs = new Vector2();
                        switch (SM.nearSkillLevel[2])
                        {
                            case 1:
                                exs = enemy.TakeDamage(attackDamage);
                                break;
                            case 2:
                                exs = enemy.TakeDamage(attackDamage);
                                break;
                            case 3:
                                exs = enemy.TakeDamage(attackDamage * 1.2f);
                                break;
                            case 4:
                                exs = enemy.TakeDamage(attackDamage * 1.2f);
                                break;
                            case 5:
                                exs = enemy.TakeDamage(attackDamage * 1.5f);
                                break;
                        }
                        ;
                        Debug.DrawRay(enemy.transform.position, transform.forward * 4, Color.red, 2f);
                        RaycastHit[] hit = Physics.RaycastAll(enemy.transform.position, transform.forward, 4, LayerMask.GetMask("Enemy"));
                        if (hit.Length > 0)
                        {
                            foreach (var ray in hit)
                            {
                                Enemy backEnemy;
                                Debug.Log(hit.ToString());
                                if (ray.transform.TryGetComponent<Enemy>(out backEnemy))
                                {
                                    switch (SM.nearSkillLevel[2])
                                    {
                                        case 1:
                                            exs = backEnemy.TakeDamage(attackDamage * 0.5f);
                                            break;
                                        case 2:
                                            exs = backEnemy.TakeDamage(attackDamage * 0.7f);
                                            break;
                                        case 3:
                                            exs = backEnemy.TakeDamage(attackDamage * 0.7f);
                                            break;
                                        case 4:
                                            exs = backEnemy.TakeDamage(attackDamage * 0.8f);
                                            break;
                                        case 5:
                                            exs = backEnemy.TakeDamage(attackDamage);
                                            break;
                                    }
                                }
                            }
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
            GameManager.Instance.messageUI.Add("관통공격");
        }
    }
    public void NearSkill4()
    {
        if (SM.nearSkillUpgrade > 0 && Input.GetKey(KeyCode.LeftControl))
        {
            if (SM.nearSkillLevel[3] == 5) return;
            SM.nearSkillLevel[3]++;
            SM.rockNearSkill[3] = false;
            SM.nearSkillUpgrade--;
            return;
        }
        if (!SM.useNearSkill[3] && !SM.rockNearSkill[3])
        {
            if (SM.nearSkill[3].mp[SM.nearSkillLevel[3] - 1] <= mp)
            {
                mp -= SM.nearSkill[3].mp[SM.nearSkillLevel[3] - 1];
            }
            else
            {
                GameManager.Instance.messageUI.Add("마나 부족");
                return;
            }
            if (SM.nearSkillLevel[3] == 0) return;
            SM.useNearSkill[3] = true;
            var skill = Instantiate(nearSkillPrefabs[3]);
            skill.transform.SetParent(transform);
            skill.transform.position = transform.position;
            skill.transform.rotation = transform.rotation;
            anim.SetTrigger("Attack");
            if (nearCollision.Length != 0)
            {
                Enemy enemy;
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, 4, LayerMask.GetMask("Enemy")))
                {
                    Debug.DrawRay(transform.position, transform.forward * 4, Color.red);
                    if (hit.transform.TryGetComponent<Enemy>(out enemy))
                    {
                        float dis = Vector3.Distance(transform.position, hit.transform.position);
                        if (dis <= attackRange)
                        {
                            Vector2 exs = new Vector2();
                            switch (SM.nearSkillLevel[3])
                            {
                                case 1:
                                    exs = enemy.TakeDamage(attackDamage);
                                    {
                                        int index = UnityEngine.Random.Range(0, 2);
                                        if (index == 1)
                                        {
                                            enemy.isStun = true;
                                        }
                                    }
                                    break;
                                case 2:
                                    exs = enemy.TakeDamage(attackDamage * 1.5f);
                                    {
                                        int index = UnityEngine.Random.Range(0, 2);
                                        if (index == 1)
                                        {
                                            enemy.isStun = true;
                                        }
                                    }
                                    break;
                                case 3:
                                    exs = enemy.TakeDamage(attackDamage * 1.5f);
                                    enemy.isStun = true;
                                    break;
                                case 4:
                                    exs = enemy.TakeDamage(attackDamage * 1.5f);
                                    enemy.isStun = true;
                                    for (int i = 0; i < nearCollision.Length; i++)
                                    {
                                        int max = 1;
                                        if (i == max) break;
                                        Enemy enemy2;
                                        if (i < nearCollision.Length)
                                        {
                                            if (nearCollision[i].TryGetComponent<Enemy>(out enemy2))
                                            {
                                                if (enemy2 != enemy)
                                                {
                                                    exs = enemy.TakeDamage(attackDamage * 1.5f);
                                                    enemy2.isStun = true;
                                                }
                                                else
                                                {
                                                    max++;
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case 5:
                                    exs = enemy.TakeDamage(attackDamage * 1.5f);
                                    enemy.isStun = true;
                                    for (int i = 0; i < nearCollision.Length; i++)
                                    {
                                        int max = 3;
                                        if (i == max) break;
                                        Enemy enemy2;
                                        if (i < nearCollision.Length)
                                        {
                                            if (nearCollision[i].TryGetComponent<Enemy>(out enemy2))
                                            {
                                                if (enemy2 != enemy)
                                                {
                                                    exs = enemy.TakeDamage(attackDamage * 1.5f);
                                                    enemy2.isStun = true;
                                                }
                                                else
                                                {
                                                    max++;
                                                }

                                            }
                                        }
                                    }
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
            }
            GameManager.Instance.messageUI.Add("관통공격");
        }
    }


    public void FarSkill()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            FarSkill1();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            FarSkill2();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            FarSkill4();
        }
    }
    public void FarSkill1()
    {
        if (SM.farSkillUpgrade > 0 && Input.GetKey(KeyCode.LeftControl))
        {
            if (SM.farSkillLevel[0] == 5) return;
            SM.farSkillLevel[0]++;
            SM.rockFarSkill[0] = false;
            SM.farSkillUpgrade--;
            return;
        }
        if (!SM.useFarSkill[0] && !SM.rockFarSkill[0])
        {
            if (SM.farSkill[0].mp[SM.farSkillLevel[0] - 1] <= mp)
            {
                mp -= SM.farSkill[0].mp[SM.farSkillLevel[0] - 1];
            }
            else
            {
                GameManager.Instance.messageUI.Add("마나 부족");
                return;
            }
            if (SM.farSkillLevel[0] == 0) return;
            SM.useFarSkill[0] = true;
            anim.SetTrigger("Attack");
            switch (SM.farSkillLevel[0])
            {
                case 1:
                    StartCoroutine(farSKill1(3, 0.8f));
                    break;
                case 2:
                    StartCoroutine(farSKill1(3, 1));
                    break;
                case 3:
                    StartCoroutine(farSKill1(4, 1.2f));
                    break;
                case 4:
                    StartCoroutine(farSKill1(4, 1.4f));
                    break;
                case 5:
                    StartCoroutine(farSKill1(5, 1.5f));
                    break;
            }
            GameManager.Instance.messageUI.Add("연속공격");
        }
    }
    public void FarSkill2()
    {
        if (SM.farSkillUpgrade > 0 && Input.GetKey(KeyCode.LeftControl))
        {
            if (SM.farSkillLevel[1] == 5) return;
            SM.rockFarSkill[1] = false;
            SM.farSkillLevel[1]++;
            SM.farSkillUpgrade--;
            return;
        }
        if (!SM.useFarSkill[1] && !SM.rockFarSkill[1])
        {
            if (SM.farSkill[1].mp[SM.farSkillLevel[1] - 1] <= mp)
            {
                mp -= SM.farSkill[1].mp[SM.farSkillLevel[1] - 1];
            }
            else
            {
                GameManager.Instance.messageUI.Add("마나 부족");
                return;
            }
            if (SM.farSkillLevel[1] == 0) return;
            SM.useFarSkill[1] = true;
            anim.SetTrigger("Attack");
            if (nearCollision.Length != 0)
            {
                switch (SM.farSkillLevel[1])
                {
                    case 1:
                        for (int i = 0; i < nearCollision.Length; i++)
                        {
                            int max = 2;
                            if (i == max) break;
                            Enemy enemy;
                            if (i < nearCollision.Length)
                            {
                                if (nearCollision[i].TryGetComponent<Enemy>(out enemy))
                                {
                                    FarAttack(enemy.transform, 0.8f);
                                }
                            }
                        }
                        break;
                    case 2:
                        for (int i = 0; i < nearCollision.Length; i++)
                        {
                            int max = 3;
                            if (i == max) break;
                            Enemy enemy;
                            if (i < nearCollision.Length)
                            {
                                if (nearCollision[i].TryGetComponent<Enemy>(out enemy))
                                {
                                    FarAttack(enemy.transform, 1);
                                }
                            }
                        }
                        break;
                    case 3:
                        for (int i = 0; i < nearCollision.Length; i++)
                        {
                            int max = 4;
                            if (i == max) break;
                            Enemy enemy;
                            if (i < nearCollision.Length)
                            {
                                if (nearCollision[i].TryGetComponent<Enemy>(out enemy))
                                {
                                    FarAttack(enemy.transform, 1.2f);
                                }
                            }
                        }
                        break;
                    case 4:
                        for (int i = 0; i < nearCollision.Length; i++)
                        {
                            int max = 4;
                            if (i == max) break;
                            Enemy enemy;
                            if (i < nearCollision.Length)
                            {
                                if (nearCollision[i].TryGetComponent<Enemy>(out enemy))
                                {
                                    FarAttack(enemy.transform, 1.4f);
                                }
                            }
                        }
                        break;
                    case 5:
                        for (int i = 0; i < nearCollision.Length; i++)
                        {
                            int max = 5;
                            if (i == max) break;
                            Enemy enemy;
                            if (i < nearCollision.Length)
                            {
                                if (nearCollision[i].TryGetComponent<Enemy>(out enemy))
                                {
                                    FarAttack(enemy.transform, 1.6f);
                                }
                            }
                        }
                        break;
                }
            }
            else
            {
                Attack2();
            }
            GameManager.Instance.messageUI.Add("멀티공격");
        }
    }
    public void FarSkill3()
    {
        if (SM.farSkillUpgrade > 0 && Input.GetKey(KeyCode.LeftControl))
        {
            if (SM.farSkillLevel[2] == 5) return;
            SM.farSkillLevel[2]++;
            SM.farSkillUpgrade--;
            SM.rockFarSkill[2] = false;
            return;
        }
        if (!SM.useFarSkill[2] && !SM.rockFarSkill[2])
        {
            if (SM.farSkill[2].mp[SM.farSkillLevel[2] - 1] <= mp)
            {
                mp -= SM.farSkill[2].mp[SM.farSkillLevel[2] - 1];
            }
            else
            {
                GameManager.Instance.messageUI.Add("마나 부족");
                return;
            }
            if (SM.farSkillLevel[2] == 0) return;
            SM.useFarSkill[2] = true;
            anim.SetTrigger("Attack");
            switch (SM.farSkillLevel[2])
            {
                case 1:
                    ShootFan(3, 1, 6);
                    break;
                case 2:
                    ShootFan(3, 1.2f, 7);
                    break;
                case 3:
                    ShootFan(5, 1.2f, 7);
                    break;
                case 4:
                    ShootFan(5, 1.5f, 7);
                    break;
                case 5:
                    ShootFan(7, 1.2f, 8);
                    break;
            }
            GameManager.Instance.messageUI.Add("방향관통공격");
        }
    }
    public void FarSkill4()
    {
        if (SM.farSkillUpgrade > 0 && Input.GetKey(KeyCode.LeftControl))
        {
            if (SM.farSkillLevel[3] == 5) return;
            SM.farSkillLevel[3]++;
            SM.rockFarSkill[3] = false;
            SM.farSkillUpgrade--;
            return;
        }
        if (!SM.useFarSkill[3] && !SM.rockFarSkill[3])
        {
            if (SM.farSkill[3].mp[SM.farSkillLevel[3] - 1] <= mp)
            {
                mp -= SM.farSkill[3].mp[SM.farSkillLevel[3] - 1];
            }
            else
            {
                GameManager.Instance.messageUI.Add("마나 부족");
                return;
            }
            if (SM.farSkillLevel[3] == 0) return;
            SM.useFarSkill[3] = true;

            anim.SetTrigger("Attack");
            if (nearCollision.Length != 0)
            {
                switch (SM.farSkillLevel[3])
                {
                    case 1:
                        StartCoroutine(PerformArrowRain(1, 4, 0.5f, true, false));
                        break;
                    case 2:
                        StartCoroutine(PerformArrowRain(2, 4, 0.5f, false, false));
                        break;
                    case 3:
                        StartCoroutine(PerformArrowRain(2, 6, 0.5f, false, false));
                        break;
                    case 4:
                        StartCoroutine(PerformArrowRain(3, 6, 0.25f, false, false));
                        break;
                    case 5:
                        StartCoroutine(PerformArrowRain(4, 6, 0.25f, true, false));
                        break;
                }
            }
            else
            {
                switch (SM.farSkillLevel[3])
                {
                    case 1:
                        StartCoroutine(PerformArrowRain(1, 4, 0.5f, true, true));
                        break;
                    case 2:
                        StartCoroutine(PerformArrowRain(2, 4, 0.5f, false, true));
                        break;
                    case 3:
                        StartCoroutine(PerformArrowRain(2, 6, 0.5f, false, true));
                        break;
                    case 4:
                        StartCoroutine(PerformArrowRain(3, 6, 0.25f, false, true));
                        break;
                    case 5:
                        StartCoroutine(PerformArrowRain(4, 6, 0.25f, true, true));
                        break;
                }
            }
            GameManager.Instance.messageUI.Add("광역공격");
        }
    }


    public void MagicSkill()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            MagicSkill1();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            MagicSkill2();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            MagicSkill3();
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            MagicSkill4();
        }
    }
    public void MagicSkill1()
    {
        if (SM.magicSkillUpgrade > 0 && Input.GetKey(KeyCode.LeftControl))
        {
            if (SM.magicSkillLevel[0] == 5) return;
            SM.magicSkillLevel[0]++;
            SM.rockMagicSkill[0] = false;
            SM.magicSkillUpgrade--;
            return;
        }
        if (!SM.useMagicSkill[0] && !SM.rockMagicSkill[0])
        {
            if (SM.magicSkill[0].mp[SM.magicSkillLevel[0] - 1] <= mp)
            {
                mp -= SM.magicSkill[0].mp[SM.magicSkillLevel[0] - 1];
            }
            else
            {
                GameManager.Instance.messageUI.Add("마나 부족");
                return;
            }
            if (SM.magicSkillLevel[0] == 0) return;
            SM.useMagicSkill[0] = true;
            anim.SetTrigger("Attack");
            if (nearCollision.Length != 0)
            {
                switch (SM.magicSkillLevel[0])
                {
                    case 1:
                        MagicAttack(En.transform, 4, 1);
                        break;
                    case 2:
                        MagicAttack(En.transform, 4, 1.2f);
                        break;
                    case 3:
                        MagicAttack(En.transform, 5, 1.5f);
                        break;
                    case 4:
                        MagicAttack(En.transform, 6, 1.8f);
                        break;
                    case 5:
                        MagicAttack(En.transform, 7, 2);
                        break;
                }
            }
            else
            {
                switch (SM.magicSkillLevel[0])
                {
                    case 1:
                        MagicAttack(transform, 4, 1);
                        break;
                    case 2:
                        MagicAttack(transform, 4, 1.2f);
                        break;
                    case 3:
                        MagicAttack(transform, 5, 1.5f);
                        break;
                    case 4:
                        MagicAttack(transform, 6, 1.8f);
                        break;
                    case 5:
                        MagicAttack(transform, 7, 2);
                        break;
                }
            }

            GameManager.Instance.messageUI.Add("연속공격");
        }
    }
    public void MagicSkill2()
    {
        if (SM.magicSkillUpgrade > 0 && Input.GetKey(KeyCode.LeftControl))
        {
            if (SM.magicSkillLevel[1] == 5) return;
            SM.rockMagicSkill[1] = false;
            SM.magicSkillLevel[1]++;
            SM.magicSkillUpgrade--;
            return;
        }
        if (!SM.useMagicSkill[1] && !SM.rockMagicSkill[1])
        {
            if (SM.magicSkill[1].mp[SM.magicSkillLevel[1] - 1] <= mp)
            {
                mp -= SM.magicSkill[1].mp[SM.magicSkillLevel[1] - 1];
            }
            else
            {
                GameManager.Instance.messageUI.Add("마나 부족");
                return;
            }
            if (SM.magicSkillLevel[1] == 0) return;
            SM.useMagicSkill[1] = true;
            anim.SetTrigger("Attack");
            if (nearCollision.Length != 0)
            {
                switch (SM.magicSkillLevel[1])
                {
                    case 1:
                        ChaingAttack(2, 1);
                        break;
                    case 2:
                        ChaingAttack(3, 1.1f);
                        break;
                    case 3:
                        ChaingAttack(3, 1.3f);
                        break;
                    case 4:
                        ChaingAttack(4, 1.5f);
                        break;
                    case 5:
                        ChaingAttack(5, 1.8f);
                        break;
                }
            }
            else
            {
                Attack2();
            }
            GameManager.Instance.messageUI.Add("체이닝공격");
        }
    }
    public void MagicSkill3()
    {
        if (SM.magicSkillUpgrade > 0 && Input.GetKey(KeyCode.LeftControl))
        {
            if (SM.magicSkillLevel[2] == 5) return;
            SM.magicSkillLevel[2]++;
            SM.magicSkillUpgrade--;
            SM.rockMagicSkill[2] = false;
            return;
        }
        if (!SM.useMagicSkill[2] && !SM.rockMagicSkill[2])
        {
            if (SM.magicSkill[2].mp[SM.magicSkillLevel[2] - 1] <= mp)
            {
                mp -= SM.magicSkill[2].mp[SM.magicSkillLevel[2] - 1];
            }
            else
            {
                GameManager.Instance.messageUI.Add("마나 부족");
                return;
            }
            if (SM.magicSkillLevel[2] == 0) return;
            SM.useMagicSkill[2] = true;
            anim.SetTrigger("Attack");
            switch (SM.magicSkillLevel[2])
            {
                case 1:
                    Heal(0.5f, false);
                    break;
                case 2:
                    Heal(1, false);
                    break;
                case 3:
                    Heal(1, true);
                    break;
                case 4:
                    Heal(1.5f, true);
                    break;
                case 5:
                    Heal(1.8f, true);
                    break;
            }
            GameManager.Instance.messageUI.Add("회복");
        }
    }
    public void MagicSkill4()
    {
        if (SM.magicSkillUpgrade > 0 && Input.GetKey(KeyCode.LeftControl))
        {
            if (SM.magicSkillLevel[3] == 5) return;
            SM.magicSkillLevel[3]++;
            SM.rockMagicSkill[3] = false;
            SM.magicSkillUpgrade--;
            return;
        }
        if (!SM.useMagicSkill[3] && !SM.rockMagicSkill[3])
        {
            if (SM.magicSkill[3].mp[SM.magicSkillLevel[3] - 1] <= mp)
            {
                mp -= SM.magicSkill[3].mp[SM.magicSkillLevel[3] - 1];
            }
            else
            {
                GameManager.Instance.messageUI.Add("마나 부족");
                return;
            }
            if (SM.magicSkillLevel[3] == 0) return;
            SM.useMagicSkill[3] = true;

            anim.SetTrigger("Attack");
            if (nearCollision.Length != 0)
            {
                switch (SM.magicSkillLevel[3])
                {
                    case 1:
                        StartCoroutine(forzen(2, En.GetComponent<Enemy>(), 10, .1f));
                        break;
                    case 2:
                        StartCoroutine(forzen(2, En.GetComponent<Enemy>(), 20, .1f));
                        break;
                    case 3:
                        StartCoroutine(forzen(3, En.GetComponent<Enemy>(), 20, .2f));
                        break;
                    case 4:
                        StartCoroutine(forzen(4, En.GetComponent<Enemy>(), 30, .5f));
                        break;
                    case 5:
                        StartCoroutine(forzen(5, En.GetComponent<Enemy>(), 50, .5f));
                        break;
                }
            }
            GameManager.Instance.messageUI.Add("포이즌");
        }
    }


    IEnumerator forzen(float time, Enemy enemy, float damage, float speedDownPersent)
    {
        enemy.SpeedDown(speedDownPersent);
        for(int i =0; i < time; i++)
        {
            Vector2 exs = new Vector2();
            exs = enemy.TakeDamage(damage);
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
            yield return new WaitForSeconds(1);
        }
        enemy.OrizinSpeed();
        
    }
    public void Heal(float persent, bool give)
    {
        if (give)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, 5, LayerMask.GetMask("Player"));
            foreach (Collider hit in hits)
            {
                Player player;
                if (hit.TryGetComponent<Player>(out player))
                {
                    player.hp += attackDamage * persent;
                }
            }
        }
        else
        {
            hp += attackDamage * persent;
        }
    }
    public void ChaingAttack(int count, float persent)
    {
        Collider[] hitCollider = Physics.OverlapSphere(En.transform.position, 4, LayerMask.GetMask("Enemy"));
        Vector2 exs = new Vector2();
        LR.positionCount+=2;
        LR.SetPosition(0, transform.position);
        LR.SetPosition(1, En.transform.position);
        int index = 2;
        for (int i = 0; i < hitCollider.Length; i++)
        { 
            if (i == count-1) break;
            Enemy enemy;
            if (i < hitCollider.Length)
            {
                if (hitCollider[i].TryGetComponent<Enemy>(out enemy))
                {
                    
                    if(enemy == En.GetComponent<Enemy>())
                    {
                        count++;
                    }
                    else
                    {
                        LR.positionCount++;
                        LR.SetPosition(index, enemy.transform.position);
                        index++;
                        exs = enemy.TakeDamage(attackDamage * persent);
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
        }

        
        StartCoroutine(LRRest());
    }

    IEnumerator LRRest()
    {
        yield return new WaitForSeconds(1.5f);
        LR.positionCount = 0;
    }

    public void MagicAttack(Transform target, float radiuse, float persent)
    {
        Collider[] hits = Physics.OverlapSphere(target.position, radiuse, LayerMask.GetMask("Enemy"));

        foreach (var hit in hits)
        {
            Enemy enemy;
            if(hit.TryGetComponent<Enemy>(out enemy)){
                var bullet = Instantiate(bulletPrefab);
                bullet.transform.position = firePose.position;
                bullet.transform.LookAt(hit.transform.position);
                bullet.GetComponent<Bullet>().Set(this, attackDamage * persent);
            }
        }

        if(hits.Length == 0)
        {
            var bullet = Instantiate(bulletPrefab);
            bullet.transform.position = firePose.position;
            bullet.transform.eulerAngles = firePose.eulerAngles;
            bullet.GetComponent<Bullet>().Set(this, attackDamage);
        }
    }

    //원거리 아처 스킬
    IEnumerator PerformArrowRain(int count, float radius, float time, bool move, bool not)
    {
        if (!move)
        {
            this.move = false;
        }
        for(int i = 0; i< count; i++)
        {
            if (not)
            {
                arrowDropPos.position = transform.position;
                arrowDropPos.position += transform.up * 10 + transform.forward * 8;
                DropArrows(arrowDropPos, radius);
            }
            else
            {
                DropArrows(En.transform, radius);
            }
            yield return new WaitForSeconds(time);
        }
        if (!move)
        {
            this.move = true;
        }
    }
    private void DropArrows(Transform center, float radius)
    {
        for (int i = 0; i < 50; i++)
        {
            Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * radius;
            Vector3 dropPosition = new Vector3(
                center.position.x + randomCircle.x,
                center.position.y + 10f, 
                center.position.z + randomCircle.y
            );


            GameObject bullet = Instantiate(
                bulletPrefab, dropPosition, Quaternion.identity);
            bullet.transform.localScale = Vector3.one * 1.5f;
            bullet.transform.eulerAngles = new Vector3(90, 0, 0);
            bullet.GetComponent<Bullet>().Set(this, attackDamage);
            bullet.GetComponent<BoxCollider>().size *= 3f;
            Rigidbody arrowRb = bullet.GetComponent<Rigidbody>();
            if (arrowRb != null)
            {
                arrowRb.velocity = Vector3.down * 5f;
            }
        }
    }
    public void ShootFan(int numberOfBullets, float damagePersent, float maxdis)
    {
        float startAngle = -45 / 2f;

        float angleStep = 45 / (numberOfBullets - 1);

        for (int i = 0; i < numberOfBullets; i++)
        {
            float angle = startAngle + i * angleStep;

            Quaternion bulletRotation = firePose.rotation * Quaternion.Euler(0, angle, 0);
            var bullet = Instantiate(bulletPrefab);
            bullet.transform.position = firePose.position;
            bullet.transform.rotation = bulletRotation;
            bullet.GetComponent<Bullet>().Set(this, attackDamage * damagePersent);
            bullet.GetComponent<Bullet>().penetration = true;
            bullet.GetComponent<Bullet>().maxDis = maxdis;
        }
    }
    public void FarAttack(Transform target, float damagePersent)
    {
        var bullet = Instantiate(bulletPrefab);
        bullet.transform.position = firePose.position;
        bullet.transform.LookAt(target);
        bullet.GetComponent<Bullet>().Set(this, attackDamage * damagePersent);
    }
    IEnumerator farSKill1(int count, float damagePersent)
    {
        for(int i = 0; i< count; i++)
        {
            yield return new WaitForSeconds(0.1f);
            var bullet = Instantiate(bulletPrefab);
            bullet.transform.position = firePose.position;
            bullet.transform.eulerAngles = firePose.eulerAngles;
            bullet.GetComponent<Bullet>().Set(this, attackDamage * damagePersent);
        }
    }


    public void Move()
    {
        if (root)
        {
            if (!move) return;
            if (isDie) return;
            selectUI.SetActive(true);
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            dir = new Vector3(h, 0, v);
            if (dir == Vector3.zero)
            {
                anim.SetBool("Move", false);
                anim.SetBool("Run", false);
            }
            else
            {
                anim.SetBool("Move", true);
            }
            Vector3 dirLook = transform.forward * dir.z + transform.right * dir.x;
            dirLook = new Vector3(dirLook.x, -0.1f, dirLook.z);
            dirLook.Normalize();
            


            float currentSpeed = rb.velocity.magnitude;
            float currentSpeedMax = 8;


            if (Input.GetKey(KeyCode.LeftShift) && dir != Vector3.zero)
            {
                if (currentSpeed < currentSpeedMax)
                {
                    rb.velocity = dirLook * (speed+2) * 100f * Time.fixedDeltaTime;
                }
                anim.SetBool("Run", true);
            }
            else
            {
                if (currentSpeed < currentSpeedMax)
                {
                    rb.velocity = dirLook * speed * 100f * Time.fixedDeltaTime;
                }
                anim.SetBool("Run", false);
            }

            

        }
        else
        {
            selectUI.SetActive(false);
            if (follow)
            {
                if (isDie) return;
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
                                        Vector3 dir = transform.forward + transform.right;
                                        rb.velocity =dir * speed * 50 * Time.fixedDeltaTime;
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
                                        transform.LookAt(player.pos2, Vector3.up);
                                        //transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                                        Vector3 dir = transform.forward + transform.right;
                                        rb.velocity = dir * speed * 50 * Time.fixedDeltaTime;
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
                                        Vector3 dir = transform.forward + transform.right;
                                        rb.velocity = dir * speed * 50 * Time.fixedDeltaTime;
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
                                        Vector3 dir = transform.forward + transform.right;
                                        rb.velocity = dir * speed * 50 * Time.fixedDeltaTime;
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
                                        Vector3 dir = transform.forward + transform.right;
                                        rb.velocity = dir * speed * 50 * Time.fixedDeltaTime;
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
                                        Vector3 dir = transform.forward + transform.right;
                                        rb.velocity = dir * speed * 50 * Time.fixedDeltaTime;
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
            if (isDie) return;
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
                if (isDie) return;
                float dis = Vector3.Distance(transform.position, En.transform.position);
                if (dis <= enemyGamegiRange)
                {
                    float dis2 = Vector3.Distance(transform.position, En.transform.position);
                    if(dis <= attackRange)
                    {
                        currentAiSkillAttackTime += Time.deltaTime;
                        if(currentAiSkillAttackTime > aiSkillAttackTime)
                        {
                            RandomSKill();
                            currentAiSkillAttackTime = 0;
                        }
                        else
                        {
                            Attack2();
                        }
                            
                        transform.LookAt(En.transform.position);
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

    public void RandomSKill()
    {
        bool[] rockSkills;
        bool[] useSkills;
        Action[] skills;
        // stats에 따라 적절한 배열을 할당
        switch (stats)
        {
            case playerStats.near:
                rockSkills = SM.rockNearSkill;
                useSkills = SM.useNearSkill;
                skills = new Action[] { NearSkill1, NearSkill2, NearSkill3, NearSkill4 };
                break;
            case playerStats.far:
                rockSkills = SM.rockFarSkill;
                useSkills = SM.useFarSkill;
                skills = new Action[] { FarSkill1, FarSkill2, FarSkill3, FarSkill4 };
                break;
            case playerStats.magic:
                rockSkills = SM.rockMagicSkill;
                useSkills = SM.useMagicSkill;
                skills = new Action[] { MagicSkill1, MagicSkill2, MagicSkill3, MagicSkill4 };
                break;
            default:
                return; // 유효하지 않은 stats
        }

        // 사용 가능한 스킬 인덱스를 저장할 리스트
        List<int> usableSkillIndices = new List<int>();

        for (int i = 0; i < skills.Length; i++)
        {
            if (!rockSkills[i] && !useSkills[i])
            {
                usableSkillIndices.Add(i);
            }
        }


        // 사용 가능한 스킬의 개수에 따라 로직 분기
        if (usableSkillIndices.Count == 0)
        {
            return;
        }
        else if (usableSkillIndices.Count == 1)
        {
            int index = usableSkillIndices[0];
            skills[index]?.Invoke();
        }
        else // usableSkillIndices.Count >= 2
        {
            // 리스트에서 무작위로 하나 선택
            int randomIndex = UnityEngine.Random.Range(0, usableSkillIndices.Count);
            int index = usableSkillIndices[randomIndex];
            skills[index]?.Invoke();
        }
    }

    public void Attack2()
    {
        if (attack) return;
        if (isDie) return;
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
                    if (En.TryGetComponent<Enemy>(out enemy))
                    {
                        float dis = Vector3.Distance(transform.position, En.transform.position);
                        if (dis <= attackRange)
                        {
                            float randomValue = UnityEngine.Random.value;
                            float chanceNor = criticalDamagePersent / 100;
                            Vector2 exs = Vector2.zero;
                            if (randomValue <= chanceNor)
                            {
                                exs = enemy.TakeDamage(attackDamage * 2);
                            }
                            else
                            {
                                exs = enemy.TakeDamage(attackDamage);
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
                break;
            case playerStats.far:
            case playerStats.magic:
                var bullet = Instantiate(bulletPrefab);
                bullet.transform.position = firePose.position;
                bullet.transform.eulerAngles = firePose.eulerAngles;
                bullet.GetComponent<Bullet>().Set(this, attackDamage);
                break;
        }
    }

    public  void TakeDamage(float damage)
    {
        hp -= damage;
        anim.SetTrigger("Hit");
        if (hp <= 0)
        {
            isDie = true;
            anim.StopPlayback();
            anim.SetTrigger("Die");
            StartCoroutine(Die());
            foreach (var playerObj in GameObject.FindGameObjectsWithTag("Player"))
            {
                Player player;
                if (playerObj.TryGetComponent<Player>(out player))
                {
                    if (this != player)
                    {
                        if (player.hp <= 0)
                        {
                            switch (player.stats)
                            {
                                case playerStats.near:
                                    GM.playerLife[0] = false;
                                    GM.players[0].root = true;
                                    GM.players[1].root = false;
                                    GM.players[2].root = false;
                                    GM.skillUI[0].SetActive(true);
                                    GM.skillUI[1].SetActive(false);
                                    GM.skillUI[2].SetActive(false);
                                    break;
                                case playerStats.far:
                                    GM.playerLife[1] = false;
                                    GM.players[0].root = false;
                                    GM.players[1].root = true;
                                    GM.players[2].root = false;
                                    GM.skillUI[0].SetActive(false);
                                    GM.skillUI[1].SetActive(true);
                                    GM.skillUI[2].SetActive(false);
                                    break;
                                case playerStats.magic:
                                    GM.playerLife[2] = false;
                                    GM.players[0].root = false;
                                    GM.players[1].root = false;
                                    GM.players[2].root = true;
                                    GM.skillUI[0].SetActive(false);
                                    GM.skillUI[1].SetActive(false);
                                    GM.skillUI[2].SetActive(true);
                                    break;
                            }
                        }
                    }
                }

            }
        }
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
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
