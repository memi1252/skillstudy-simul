using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using System.Linq;


[System.Serializable]
public class RankData
{
    public string initail;
    public int score;
}

[System.Serializable]
public class RankDataList
{
    public List<RankData> dataList = new List<RankData>();
}

public class RankManager : MonoBehaviour
{
    public static RankManager instance;

    public List<RankData> data = new List<RankData>();
    private string saveName = "saveData.json";

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        Load();
        Save();
    }

    public void Save()
    {
        RankDataList listWrapper = new RankDataList();
        listWrapper.dataList = this.data;

        string json = JsonUtility.ToJson(listWrapper);
#if UNITY_EDITOR
        string path = Path.Combine(Application.dataPath, saveName);
#else
         string path = Path.Combine(Directory.GetParent(Application.dataPath).FullName, saveName);
#endif


        File.WriteAllText(path, json);
    }

    public void Load()
    {
#if UNITY_EDITOR
        string path = Path.Combine(Application.dataPath, saveName);
#else
         string path = Path.Combine(Directory.GetParent(Application.dataPath).FullName, saveName);
#endif

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            RankDataList loadData = JsonUtility.FromJson<RankDataList>(json);
           
            if(loadData != null )
            {
                data.Clear();
                data = loadData.dataList;
            }
          
        }
        data = data.OrderByDescending(rank => rank.score).ToList();
    }
    public void RankAdd(int Score)
    {
        RankData data = new RankData();
        data.score = Score;
        this.data.Add(data);
        this.data = this.data.OrderByDescending(rank => rank.score).ToList();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
#if UNITY_EDITOR
            string path = Path.Combine(Application.dataPath, saveName);
#else
         string path = Path.Combine(Directory.GetParent(Application.dataPath).FullName, saveName);
#endif

            if (File.Exists(path))
            {
                File.Delete(path);
                data.Clear();
                Debug.Log("삭제됨");
            }
            else
            {
                Debug.Log("존재하지 않음");
            }
        }
        else if(Input.GetKeyDown(KeyCode.PageUp))
        {
            RankAdd(index);
            index += 100;
        }
    }
    int index = 100;
}
