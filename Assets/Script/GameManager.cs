using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct ItemData
{
    public int count;
    public bool stackble;
}

[Serializable]
public class InventoryEntry
{
    public items Key;
    public ItemData Value;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private List<InventoryEntry> inventory1List = new List<InventoryEntry>();
    [SerializeField]
    private List<InventoryEntry> inventory2List = new List<InventoryEntry>();
    [SerializeField]
    private List<InventoryEntry> inventory3List = new List<InventoryEntry>();
    
    public Dictionary<items, ItemData> inventory1 = new Dictionary<items, ItemData>();
    public Dictionary<items, ItemData> inventory2 = new Dictionary<items, ItemData>();
    public Dictionary<items, ItemData> inventory3 = new Dictionary<items, ItemData>();
    
    public MessageUI messageUI;

    public GameObject[] skillUI;

    public bool[] playerLife = new bool[3];
    

    public bool cameraMove = true;

    public int stage = 1;
    public int Stage1Level1EnemyCount;
    public int Stage1Level2EnemyCount;


    public int score;
    public Text scoreText;
    
    public InventoryUI inventoryUI;


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
        scoreText.text = score.ToString();

        if (Input.GetKeyDown(KeyCode.I))
        {
            if(inventoryUI.gameObject.activeSelf)
            {
                inventoryUI.Hide();
            }
            else
            {
                inventoryUI.Show();
            }
            
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1) && !players[0].root && playerLife[0])
        {
            messageUI.Add("전사 캐릭터 변경", Color.green);
            players[0].root = true;
            players[1].root = false;
            players[2].root = false;
            skillUI[0].SetActive(true);
            skillUI[1].SetActive(false);
            skillUI[2].SetActive(false);
        }else if (Input.GetKeyDown(KeyCode.Alpha2) && !players[1].root && playerLife[1])
        {
            messageUI.Add("아처 캐릭터 변경", Color.green);
            players[0].root = false;
            players[1].root = true;
            players[2].root = false;
            skillUI[0].SetActive(false);
            skillUI[1].SetActive(true);
            skillUI[2].SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && !players[2].root && playerLife[2])
        {
            messageUI.Add("마법사 캐릭터 변경", Color.green);
            players[0].root = false;
            players[1].root = false;
            players[2].root = true;
            skillUI[0].SetActive(false);
            skillUI[1].SetActive(false);
            skillUI[2].SetActive(true);
        }
    }
    
    private void OnValidate()
    {
        inventory1List.Clear();
        inventory2List.Clear();
        inventory3List.Clear();
        foreach (var kvp in inventory1)
        {
            inventory1List.Add(new InventoryEntry { Key = kvp.Key, Value = kvp.Value });
        }
        foreach (var kvp in inventory2)
        {
            inventory2List.Add(new InventoryEntry { Key = kvp.Key, Value = kvp.Value });
        }
        foreach (var kvp in inventory3)
        {
            inventory3List.Add(new InventoryEntry { Key = kvp.Key, Value = kvp.Value });
        }
    }

    public void GetItem1(items item, int count, bool stack)
    {
        if (inventory1.ContainsKey(item))
        {
            if (inventory1[item].stackble)
            {
                var value = inventory1[item];
                value.count += count;
                inventory1[item] = value;
            }
            else
            {
                ItemData data = new ItemData();
                data.count = count;
                data.stackble = stack;
                inventory1.Add(item,data);
            }
        }
        else
        {
            ItemData data = new ItemData();
            data.count = count;
            data.stackble = stack;
            inventory1.Add(item,data);
        }

        OnValidate();

    }
    public void GetItem2(items item, int count, bool stack)
    {
        if (inventory2.ContainsKey(item))
        {
            if (inventory2[item].stackble)
            {
                var value = inventory2[item];
                value.count += count;
                inventory2[item] = value;
            }
            else
            {
                ItemData data = new ItemData();
                data.count = count;
                data.stackble = stack;
                inventory2.Add(item,data);
            }
        }
        else
        {
            ItemData data = new ItemData();
            data.count = count;
            data.stackble = stack;
            inventory2.Add(item,data);
        }

        OnValidate();
    }
    public void GetItem3(items item, int count, bool stack)
    {
        if (inventory3.ContainsKey(item))
        {
            if (inventory3[item].stackble)
            {
                var value = inventory3[item];
                value.count += count;
                inventory3[item] = value;
            }
            else
            {
                ItemData data = new ItemData();
                data.count = count;
                data.stackble = stack;
                inventory3.Add(item,data);
            }
        }
        else
        {
            ItemData data = new ItemData();
            data.count = count;
            data.stackble = stack;
            inventory3.Add(item,data);
        }

        OnValidate();
    }
}
