using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public List<GameObject> enemys;

    public int stage;
    public GameObject[] enemysPrefabs;
    private int index = 0;
    public int level1EnemyMax;
    public int level2EnemyMax;
    public int level3EnemyMax;
    public int destroyObjctMax;
    public bool bossSpanw;
    public GameObject bossPrefab;
    public bool clear;
    public Stage nextStage;

    private bool enemySpawn;

    public Transform[] SpawnPoint;
    public float currentTimer;
    public float timerMax;
    
    private void Awake()
    {
	
    }

    private void Start()
    {
        if (enemys != null)
        {
            if(stage == 3)
            {
                if (bossPrefab != null)
                {
                    GameManager.Instance.boss3Spawn.SetActive(true);
                    StartCoroutine(Boss3Spawn());
                    Time.timeScale = 0;
                }
            }
            enemys[index].SetActive(true);
            enemys[index].transform.position = SpawnPoint[Random.Range(0, SpawnPoint.Length)].position;
        }
    }
    IEnumerator Boss3Spawn()
    {
        yield return new WaitForSecondsRealtime(5.5f);
        Time.timeScale = 1;
        Destroy(GameManager.Instance.boss3Spawn);
        var boss = Instantiate(bossPrefab);
        boss.transform.position = SpawnPoint[Random.Range(0, SpawnPoint.Length)].position;
        Destroy(gameObject);
    }


    private void Update()
    {
        currentTimer += Time.deltaTime;
        if (currentTimer >= timerMax && !GameManager.Instance.gameOver)
        {
            GameManager.Instance.messageUI.Add("타임 오버", Color.red, true);
            GameManager.Instance.gameOver = true;
            switch (stage)
            {
                case 1:
                    GameManager.Instance.GameOver(GameOverReason.stage1TimeOut);
                    break;
                case 2:
                    GameManager.Instance.GameOver(GameOverReason.stage2TimeOut);
                    break;
                case 3:
                    GameManager.Instance.GameOver(GameOverReason.stage3TimeOut);
                    break;
            }
        }
        if(clear && nextStage != null)
        {
            nextStage.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        if (stage == 1 && !bossSpanw)
        {
            if (GameManager.Instance.Stage1Level1EnemyCount >= level1EnemyMax &&
                GameManager.Instance.Stage1Level2EnemyCount >= level2EnemyMax)
            {
                bossSpanw = true;
                //보스 소환
                GameManager.Instance.messageUI.Add("보스1이 출현하였습니다!!", Color.red, true);
                var boss = Instantiate(bossPrefab, transform.position, Quaternion.identity);
                boss.GetComponent<Enemy>().stage = this;
            }
        }else if (stage == 2 && !bossSpanw)
        {
            if (GameManager.Instance.Stage2Level2EnemyCount >= level2EnemyMax &&
                GameManager.Instance.Stage2Level3EnemyCount >= level3EnemyMax &&
                GameManager.Instance.Stage2DestoyObjectCount >= destroyObjctMax)
            {
                bossSpanw = true;
                //보스 소환
                GameManager.Instance.messageUI.Add("보스2이 출현하였습니다!!", Color.red, true);
                var boss = Instantiate(bossPrefab, transform.position, Quaternion.identity);
                boss.GetComponent<Enemy>().stage = this;
            }
        }
        if (enemys.Count > 0 && index <= enemys.Count)
        {
            enemySpawn = false;
            if (enemys[index].activeSelf)
            {
                bool isDestroy = true;
                for (int i = 0; i < enemys[index].transform.childCount; i++)
                {
                    if (enemys[index].transform.GetChild(i).gameObject.activeSelf)
                    {
                        isDestroy = false;
                    }
                }

                if (isDestroy)
                {
                    Destroy(enemys[index]);
                    enemys.RemoveAt(index);
                    if (enemys.Count != 0)
                    {
                        if (bossSpanw)
                        {
                            return;
                        }
                        enemys[index].SetActive(true);
                        enemys[index].transform.position = SpawnPoint[Random.Range(0, SpawnPoint.Length)].position;
                    }
                    else
                    {
                        enemySpawn = true;
                    }
                        
                }
            }
        }
        else
        {
            if (!enemySpawn)
            {
                return;
            }
            if (bossSpanw)
            {
                return;
            }
            enemySpawn = false;
            switch (stage)
            {
                case 1:
                    if (GameManager.Instance.Stage1Level1EnemyCount < level1EnemyMax &&
                        GameManager.Instance.Stage1Level2EnemyCount < level2EnemyMax)
                    {
                        int index = Random.Range(0, enemysPrefabs.Length);
                        var spawn = Instantiate(enemysPrefabs[index], transform);
                        spawn.transform.localPosition = Vector3.zero;
                        spawn.SetActive(true);
                        enemys.Add(spawn);
                    }else if (GameManager.Instance.Stage1Level1EnemyCount < level1EnemyMax)
                    {
                        int index = Random.Range(0, 2);
                        var spawn = Instantiate(enemysPrefabs[index], transform);
                        spawn.transform.localPosition = Vector3.zero;
                        spawn.SetActive(true);
                        enemys.Add(spawn);
                    }else if (GameManager.Instance.Stage1Level2EnemyCount < level2EnemyMax)
                    {
                        int index = Random.Range(2, enemysPrefabs.Length);
                        var spawn = Instantiate(enemysPrefabs[index], transform);
                        spawn.transform.localPosition = Vector3.zero;
                        spawn.SetActive(true);
                        enemys.Add(spawn);
                    }
                    break;
                case 2:
                    if (GameManager.Instance.Stage2Level2EnemyCount < level2EnemyMax &&
                        GameManager.Instance.Stage2Level3EnemyCount < level3EnemyMax)
                    {
                        int index = Random.Range(0, enemysPrefabs.Length);
                        var spawn = Instantiate(enemysPrefabs[index], transform);
                        spawn.transform.localPosition = Vector3.zero;
                        spawn.SetActive(true);
                        enemys.Add(spawn);
                    }else if (GameManager.Instance.Stage2Level2EnemyCount < level2EnemyMax)
                    {
                        int index = Random.Range(0, 2);
                        var spawn = Instantiate(enemysPrefabs[index], transform);
                        spawn.transform.localPosition = Vector3.zero;
                        spawn.SetActive(true);
                        enemys.Add(spawn);
                    }else if (GameManager.Instance.Stage2Level3EnemyCount < level3EnemyMax)
                    {
                        int index = Random.Range(2, enemysPrefabs.Length);
                        var spawn = Instantiate(enemysPrefabs[index], transform);
                        spawn.transform.localPosition = Vector3.zero;
                        spawn.SetActive(true);
                        enemys.Add(spawn);
                    }
                    break;
            }
            
        }
    }


}
