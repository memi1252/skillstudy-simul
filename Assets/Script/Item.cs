using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum items
{
    heal, mp
}

public class Item : MonoBehaviour
{
    

    public items item;
    private Text text;

    private void Awake()
    {
        text = GetComponentInChildren<Text>();
    }

    private void Start()
    {
        switch (item)
        {
            case items.heal:
                text.text = "채력 회복아이템";
                break;
            case items.mp:
                text.text = "마나 회복 아이템";
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (var playerObj in GameObject.FindGameObjectsWithTag("Player"))
            {
                Player player = playerObj.GetComponent<Player>();
                if (player.root)
                {
                    if (player.inventoryCount >= player.maxInventoryCount)
                    {
                        GameManager.Instance.messageUI.Add("인벤토리에 자리가 부족합니다.", Color.red, true);
                    }
                    else
                    {
                        ItemGet(player);
                        Destroy(gameObject);
                    }
                }
            }
            
            
        }
    }

    private void ItemGet(Player player)
    {
        switch (item)
        {
            case items.heal:
                GameManager.Instance.messageUI.Add("체력회복 아이템 획득", Color.green, true);
                break;
            case items.mp:
                GameManager.Instance.messageUI.Add("마나회복 아이템 획득", Color.green, true);
                break;
        }
        player.inventoryCount++;
        InventoryGet(player, item, true);
        
    }

    private void InventoryGet(Player player, items item, bool stack)
    {
        switch (player.stats)
        {
            case playerStats.near:
                GameManager.Instance.GetItem1(item, 1, stack);
                break;
            case playerStats.far:
                GameManager.Instance.GetItem2(item, 1, stack);
                break;
            case playerStats.magic:
                GameManager.Instance.GetItem3(item, 1, stack);
                break;
        }
    }
}
