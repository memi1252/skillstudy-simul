using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemData
{
    public int id;
    public int count;
    public bool stackble;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public MessageUI messageUI;

    public List<ItemData> inventory1 = new List<ItemData>();
    public List<ItemData> inventory2 = new List<ItemData>();
    public List<ItemData> inventory3 = new List<ItemData>();


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }else
        {
            Destroy(gameObject);
        }
    }

    public Player[] players;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !players[0].root)
        {
            messageUI.Add("전사를 선택하였습니다.", Color.green);
            players[0].root = true;
            players[1].root = false;
            players[2].root = false;
        }else if (Input.GetKeyDown(KeyCode.Alpha2) && !players[1].root)
        {
            messageUI.Add("궁수를 선택하였습니다.", Color.green);
            players[0].root = false;
            players[1].root = true;
            players[2].root = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && !players[2].root)
        {
            messageUI.Add("마법사를 선택하였습니다.", Color.green);
            players[0].root = false;
            players[1].root = false;
            players[2].root = true;
        }
    }

    public void GetItem1(int id, int count, bool stack)
    {
        ItemData data = new ItemData();
        data.id = id;
        data.count = count;
        data.stackble = stack;
        inventory1.Add(data);
    }
    public void GetItem2(int id, int count, bool stack)
    {
        ItemData data = new ItemData();
        data.id = id;
        data.count = count;
        data.stackble = stack;
        inventory2.Add(data);
    }
    public void GetItem3(int id, int count, bool stack)
    {
        ItemData data = new ItemData();
        data.id = id;
        data.count = count;
        data.stackble = stack;
        inventory3.Add(data);
    }
}
