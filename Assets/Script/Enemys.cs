using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemys : MonoBehaviour
{
    public int minCount;
    public int maxCount;
    public List<GameObject> enemys;
    public List<GameObject> enemysPlus;

    public List<GameObject> spawnEnemy;

    private void OnEnable()
    {
        int index = Random.Range(minCount, maxCount+1);
        for (int i = 0; i < index; i++)
        {
            enemys[i].SetActive(true);
            spawnEnemy.Add(enemys[i]);
        }

        for (int i = 0; i < enemysPlus.Count; i++)
        {
            enemysPlus[i].SetActive(true);
            spawnEnemy.Add(enemys[i]);
        }
    }
}
