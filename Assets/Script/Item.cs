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
                text.text = "ü�� ȸ��";
                break;
            case items.mp:
                text.text = "���� ȸ��";
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
                        GameManager.Instance.messageUI.Add("�������� ���� ������ �����մϴ�", Color.red, true);
                    }
                    else
                    {
                        ItemGet(player);
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
                GameManager.Instance.messageUI.Add("ü��ȸ���������� ȹ���Ͽ����ϴ�.", Color.green, true);
                player.inventoryCount++;
                InventoryGet(player, 0, true);
                Destroy(gameObject);
                break;
            case items.mp:
                GameManager.Instance.messageUI.Add("����ȸ���������� ȹ���Ͽ����ϴ�.", Color.green, true);
                player.inventoryCount++;
                InventoryGet(player, 1, true);
                Destroy(gameObject);
                break;
        }
    }

    private void InventoryGet(Player player, int id, bool stack)
    {
        switch (player.stats)
        {
            case playerStats.near:
                GameManager.Instance.GetItem1(id, 1, stack);
                break;
            case playerStats.far:
                GameManager.Instance.GetItem2(id, 1, stack);
                break;
            case playerStats.magic:
                GameManager.Instance.GetItem3(id, 1, stack);
                break;
        }
    }
}
