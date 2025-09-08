using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyStats
{
    naer,
    far,
    boss1,
    boss2,
    boss3
}

[System.Serializable]
public class dropItem
{
    public GameObject itemPrefab;
    [Range(0, 1)] public float perscent;
}

public class Enemy : MonoBehaviour
{
    public EnemyStats stats;
    public int level = 1;
    public float hp;
    public float maxHp;
    public int giveEx;
    public float speed = 4;
    public float attackDamage;
    public float attackRange;
    public float attackSpeed;
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
    public GameObject bulletPrefab;
    public Transform firePos;
    //public bool isAttack;

    [Header("boss1")] 
    public float boss1Skill1MaxColTime;
    private float boss1Skill1ColTime;
    public float boss1Skill2MaxColTime;
    private float boss1Skill2ColTime;
    public bool skill1;
    private float boss1EnemySpawnColTime;
    [Header("boss2")]
    public  float boss2Skill1MaxColTime;
    private float boss2Skill1ColTime;
    public  float boss2Skill2MaxColTime;
    private float boss2Skill2ColTime;
    public  float boss2Skill3MaxColTime;
    private float boss2Skill3ColTime;
    public  float boss2Skill4MaxColTime;
    private float boss2Skill4ColTime;
    private float boss2EnemySpawnColTime;
    public bool boss2Skill1;
    public bool boss2Skill2;
    public bool boss2Skill3;
    public bool boss2Skill4;
    public GameObject wavePrefab;
    private bool boss2Defence;
    private float boss2DefenceTime;
    [Header("boss3")] 
    public float boss3Skill1MaxColTime;
    private float boss3Skill1ColTime;
    public float boss3Skill2MaxColTime;
    private float boss3Skill2ColTime;
    public float boss3Skill3MaxColTime;
    private float boss3Skill3ColTime;
    public float boss3Skill4MaxColTime;
    private float boss3Skill4ColTime;
    public float boss3Skill5MaxColTime;
    private float boss3Skill5ColTime;
    public bool boss3Skill1;
    public bool boss3Skill2;
    public bool boss3Skill3;
    public bool boss3Skill4;
    public bool boss3Skill5;
    private float boss3EnemySpawnColTime;
    public GameObject dragonBreath;
    
    public GameObject[] enemys;
    public Stage stage;
    
    
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

    private float targetUpdateTimer = 1f;
    private void Update()
    {
        targetUpdateTimer += Time.deltaTime;
        
        if (targetUpdateTimer >= 1)
        {
            foreach (var player in FindObjectsOfType<Player>())
            {
                if (player.root)
                {
                    target = player.gameObject.transform;
                }
            }    
        }
        if (!isDie)
        {
            switch (stats)
            {
                case EnemyStats.boss1:
                    if (!skill1)
                    {
                        boss1Skill1ColTime += Time.deltaTime;
                    }
                    else
                    {
                        boss1Skill2ColTime += Time.deltaTime;
                    }
                    boss1EnemySpawnColTime += Time.deltaTime;
                    if (boss1EnemySpawnColTime >= 10)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            Vector2 randomCircle = Random.insideUnitCircle * 7;
                            Vector3 dropPosition = new Vector3(
                                transform.position.x + randomCircle.x,
                                transform.position.y,
                                transform.position.z + randomCircle.y
                            );
                            int index = Random.Range(0, enemys.Length);
                            Instantiate(enemys[index], dropPosition, Quaternion.identity);
                        }
                        boss1EnemySpawnColTime = 0;
                    }
                    break;
                case EnemyStats.boss2:
                    if (boss2Defence)
                    {
                        boss2DefenceTime += Time.deltaTime;
                        if (boss2DefenceTime >= 10)
                        {
                            boss2Defence = false;
                            animator.SetBool("Defend", boss2Defence);
                            boss2DefenceTime = 0;
                        }
                        else
                        {
                            hp += 2f * Time.deltaTime;
                        }
                    }

                    if (!boss2Skill1)
                    {
                        boss2Skill1ColTime += Time.deltaTime;
                    }
                    else if (!boss2Skill2)
                    {
                        boss2Skill2ColTime += Time.deltaTime;
                    }
                    else if (!boss2Skill3)
                    {
                        boss2Skill3ColTime += Time.deltaTime;
                    }
                    else if (!boss2Skill4)
                    {
                        boss2Skill4ColTime += Time.deltaTime;
                    }
                    boss2EnemySpawnColTime += Time.deltaTime;
                    if (boss2EnemySpawnColTime >= 20)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            Vector2 randomCircle = Random.insideUnitCircle * 7;
                            Vector3 dropPosition = new Vector3(
                                transform.position.x + randomCircle.x,
                                transform.position.y,
                                transform.position.z + randomCircle.y
                            );
                            int index = Random.Range(0, enemys.Length);
                            Instantiate(enemys[index], dropPosition, Quaternion.identity);
                        }
                        boss2EnemySpawnColTime = 0;
                    }
                    break;
                case EnemyStats.boss3:
                    if (!boss3Skill1)
                    {
                        boss3Skill1ColTime += Time.deltaTime;
                    }
                    else if (!boss3Skill2)
                    {
                        boss3Skill2ColTime += Time.deltaTime;
                    }
                    else if (!boss3Skill3)
                    {
                        boss3Skill3ColTime += Time.deltaTime;
                    }
                    else if (!boss3Skill4)
                    {
                        boss3Skill4ColTime += Time.deltaTime;
                    }
                    else if (!boss3Skill5)
                    {
                        boss3Skill5ColTime += Time.deltaTime;
                    }
                    boss3EnemySpawnColTime += Time.deltaTime;
                    if (boss3EnemySpawnColTime >= 25)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            Vector2 randomCircle = Random.insideUnitCircle * 7;
                            Vector3 dropPosition = new Vector3(
                                transform.position.x + randomCircle.x,
                                transform.position.y,
                                transform.position.z + randomCircle.y
                            );
                            int index = Random.Range(0, enemys.Length);
                            Instantiate(enemys[index], dropPosition, Quaternion.identity);
                        }
                        boss3EnemySpawnColTime = 0;
                    }
                    break;
            }
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
                switch (stats)
                {
                    case EnemyStats.boss1:
                        stage.clear = true;
                        GameManager.Instance.messageUI.Add("식인식물이 사망하였습니다!!", Color.red, true);
                        GameManager.Instance.stage++;
                        break;
                    case EnemyStats.boss2:
                        stage.clear = true;
                        GameManager.Instance.messageUI.Add("골램이 사망하였습니다!!", Color.red, true);
                        GameManager.Instance.stage++;
                        break;
                }
                GameManager.Instance.score += Random.Range(100, 301);
                if (GameManager.Instance.stage == 1)
                {
                    if (level == 1)
                    {
                        GameManager.Instance.Stage1Level1EnemyCount++;
                    }else if (level == 2)
                    {
                        GameManager.Instance.Stage1Level2EnemyCount++;
                    }
                }else if (level == 2)
                {
                    if (level == 2)
                    {
                        GameManager.Instance.Stage2Level2EnemyCount++;
                    }else if (level == 3)
                    {
                        GameManager.Instance.Stage2Level3EnemyCount++;
                    }
                }
                StartCoroutine(Die());
            }
        }
    }

    



    IEnumerator Die()
    {
        yield return new WaitForSeconds(3);
        if(stats == EnemyStats.far || stats == EnemyStats.naer)
        {
            GameManager.Instance.DropItem(transform.position);
        }
        else
        {
            Destroy(gameObject);
        }
        
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
                rb.velocity = Vector3.zero; // 이동 멈춤
                Attack();
            }
            else
            {
                Vector3 direction = (target.position - transform.position).normalized;
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(direction),
                    Time.deltaTime * 5f // 회전 속도 조절
                );

                float yVelocity = rb.velocity.y;
                direction *= speed;
                direction.y = yVelocity;
                rb.velocity = direction; // 이동 속도 설정
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
                transform.LookAt(target);
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                switch (stats)
                {
                    case EnemyStats.naer:
                    {
                        if (Random.Range(0, 2) == 1)
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
                        
                        break;
                    case EnemyStats.far:
                        var bullet = Instantiate(bulletPrefab);
                        bullet.transform.position = firePos.position;
                        bullet.transform.eulerAngles = new Vector3(0, firePos.eulerAngles.y, 0);
                        bullet.GetComponent<Bullet>().Set(this, attackDamage);
                        break;
                    case EnemyStats.boss1:
                    {
                        if (!skill1 && boss1Skill1ColTime >= boss1Skill1MaxColTime)
                        {
                            GameManager.Instance.messageUI.Add("식인식물: 머리 박치기!!", Color.red, true);
                            boss1Skill1ColTime = 0;
                            skill1 = true;
                            animator.SetTrigger("BodyAttack");
                            Collider[] nearCols = Physics.OverlapSphere(transform.position, 9);
                            foreach (var col in nearCols)
                            {
                                Player player;
                                if (col.TryGetComponent<Player>(out player))
                                {
                                    player.TakeDamage(attackDamage* 1.5f);
                                    player.GetComponent<Rigidbody>().AddForce(transform.forward * 15 + transform.up * 8, ForceMode.Impulse);
                                }
                            }
                        }else if(skill1 && boss1Skill1ColTime >= boss1Skill1MaxColTime)
                        {
                            GameManager.Instance.messageUI.Add("식인식물: 독 발사!!", Color.red, true);
                            boss1Skill1ColTime = 0;
                            skill1 = false;
                            animator.SetTrigger("Poison");
                            DropPoison(firePos, attackRange *3f);
                        }
                        else
                        {
                            animator.SetTrigger("Attack");
                            Player player;
                            if (target.TryGetComponent<Player>(out player))
                            {
                                player.TakeDamage(attackDamage);
                            }
                        }
                    }
                        break;
                    case EnemyStats.boss2:
                        if (!boss2Skill1 && boss2Skill1ColTime >= boss2Skill1MaxColTime)
                        {
                            boss2Skill1 = true;
                            boss2Skill1ColTime = 0;
                            GameManager.Instance.messageUI.Add("골램: 회전 공격!!", Color.red, true);
                            animator.SetTrigger("Spin");
                            Collider[] nearCols = Physics.OverlapSphere(transform.position, 9);
                            foreach (var pl in nearCols)
                            {
                                if (pl.TryGetComponent<Player>(out var player))
                                {
                                    player.TakeDamage(attackDamage* 1.5f);
                                    player.GetComponent<Rigidbody>().AddForce(transform.forward * 15 + transform.up * 2, ForceMode.Impulse);
                                }
                            }
                            boss2Skill2 = false;
                        }else if (!boss2Skill2 && boss2Skill2ColTime >= boss2Skill2MaxColTime)
                        {
                            boss2Skill2 = true;
                            boss2Skill2ColTime = 0;
                            GameManager.Instance.messageUI.Add("골램: 파동 공격", Color.red, true);
                            animator.SetTrigger("ShockwaveAttack");
                            var wave = Instantiate(wavePrefab, transform.position, Quaternion.identity);
                            wave.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                            wave.GetComponent<Wave>().damage = attackDamage * 1.5f;
                            boss2Skill3 = false;

                        }else if (!boss2Skill3 && boss2Skill3ColTime >= boss2Skill3MaxColTime)
                        {
                            boss2Skill3 = true;
                            boss2Skill3ColTime = 0;
                            GameManager.Instance.messageUI.Add("골램: 체력 회복", Color.red, true);
                            boss2Defence = true;
                            animator.SetBool("Defend", boss2Defence);
                            boss2Skill4 = false;
                        }else if (!boss2Skill4 && boss2Skill4ColTime >= boss2Skill4MaxColTime)
                        {
                            boss2Skill4 = true;
                            boss2Skill4ColTime = 0;
                            GameManager.Instance.messageUI.Add("골램: 지원 부르기", Color.red, true);
                            animator.SetTrigger("Roar");
                            for (int i = 0; i < 10; i++)
                            {
                                Vector2 randomCircle = Random.insideUnitCircle * 7;
                                Vector3 dropPosition = new Vector3(
                                    transform.position.x + randomCircle.x,
                                    transform.position.y,
                                    transform.position.z + randomCircle.y
                                );
                                int index = Random.Range(0, enemys.Length);
                                Instantiate(enemys[index], dropPosition, Quaternion.identity);
                            }
                            boss2Skill1 = false;
                            boss2Skill2 = true;
                            boss2Skill3 = true;
                        }
                        else
                        {
                            if (Random.Range(0, 2) == 1)
                            {
                                animator.SetTrigger("AttackRight");
                            }
                            else
                            {
                                animator.SetTrigger("AttackLaft");
                            }

                            if (target.TryGetComponent<Player>(out var player))
                            {
                                player.TakeDamage(attackDamage);
                            }
                        }
                        break;
                    case EnemyStats.boss3:
                        if (!boss3Skill1 && boss3Skill1ColTime >= boss3Skill1MaxColTime)
                        {
                            boss3Skill1 = true;
                            boss3Skill1ColTime = 0;
                            GameManager.Instance.messageUI.Add("드래곤: 화염구!!", Color.red, true);
                            animator.SetTrigger("ProjectileAttack");
                            var fireball = Instantiate(bulletPrefab);
                            fireball.transform.position = firePos.position;
                            fireball.transform.LookAt(target.transform);
                            boss3Skill2 = false;

                        }else if (!boss3Skill2 && boss3Skill2ColTime >= boss3Skill2MaxColTime)
                        {
                            boss3Skill2 = true;
                            boss3Skill2ColTime = 0;
                            GameManager.Instance.messageUI.Add("드래곤: 브래스!!", Color.red, true);
                            animator.SetBool("FireBreathAttack", true);
                            dragonBreath.SetActive(true);
                            StartCoroutine(DragonBreathOff(3));
                            boss3Skill3 = false;
                        }else if (!boss3Skill3 && boss3Skill3ColTime >= boss3Skill3MaxColTime)
                        {
                            boss3Skill3 = true;
                            boss3Skill3ColTime = 0;
                            GameManager.Instance.messageUI.Add("드래곤: 포효!!", Color.red, true);
                            animator.SetTrigger("Roar");
                            for (int i = 0; i < 15; i++)
                            {
                                Vector2 randomCircle = Random.insideUnitCircle * 7;
                                Vector3 dropPosition = new Vector3(
                                    transform.position.x + randomCircle.x,
                                    transform.position.y,
                                    transform.position.z + randomCircle.y
                                );
                                int index = Random.Range(0, enemys.Length);
                                Instantiate(enemys[index], dropPosition, Quaternion.identity);
                            }
                            boss3Skill4 = false;
                        }else if (!boss3Skill4 && boss3Skill4ColTime >= boss3Skill4MaxColTime)
                        {
                            boss3Skill4 = true;
                            boss3Skill4ColTime = 0;
                            GameManager.Instance.messageUI.Add("드래곤: 플라이화염구!!", Color.red, true);
                            animator.SetTrigger("FlyProjectileAttack");
                            var fireball = Instantiate(bulletPrefab);
                            fireball.transform.position = firePos.position;
                            fireball.transform.LookAt(target.transform);
                            boss3Skill5 = false;
                        }else if (!boss3Skill5 && boss3Skill5ColTime >= boss3Skill5MaxColTime)
                        {
                            boss3Skill5 = true;
                            boss3Skill5ColTime = 0;
                            GameManager.Instance.messageUI.Add("드래곤: 플라이브래스!!", Color.red, true);
                            animator.SetBool("FlyFireBreathAttack", true);
                            dragonBreath.SetActive(true);
                            StartCoroutine(DragonBreathOff(5));
                            boss3Skill1 = false;
                            boss3Skill2 = true;
                            boss3Skill3 = true;
                            boss3Skill4 = true;

                        }
                        else
                        {
                            animator.SetTrigger("Attack");
                            if (target.TryGetComponent<Player>(out var player))
                            {
                                
                                player.TakeDamage(attackDamage);
                            }
                        }
                        break;
                }
                
            }
        }
    }

    IEnumerator DragonBreathOff(float time)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool("FireBreathAttack", false);
        dragonBreath.SetActive(false);
    }
    
    private void DropPoison(Transform center, float radius)
    {
        for (int i = 0; i < 100; i++)
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
            bullet.GetComponent<SphereCollider>().radius = 1f;
            bullet.GetComponent<Bullet>().poizon = true;
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            StartCoroutine(DownPoizon(rb));
        }
    }

    IEnumerator DownPoizon(Rigidbody rb)
    {
        yield return new WaitForSeconds(0.8f);
        if (rb != null)
        {rb.transform.eulerAngles = new Vector3(90, 0, 0);
            rb.velocity = Vector3.down * 5f;
        }
    }



    public Vector2 TakeDamage(float damage)
    {
        if (isDie) return Vector2.zero;
        hp -= damage;
        animator.SetTrigger("Hit");
        animator.SetBool("Move", false);
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
