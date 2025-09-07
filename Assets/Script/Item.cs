using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum items
{
    heal1, heal2, heal3, mp1, mp2, mp3, attack, mentalFocus, lifeRestoration
}

public class Item : MonoBehaviour
{
    

    public items item;
    private Text text;
    public bool stack;

    private void Awake()
    {
        text = GetComponentInChildren<Text>();
    }

    private void Start()
    {
        switch (item)
        {
            case items.heal1:
                text.text = "생명력Lv.1아이템";
                break;
            case items.heal2:
                text.text = "생명력Lv.2아이템";
                break;
            case items.heal3:
                text.text = "생명력Lv.3아이템";
                break;
            case items.mp1:
                text.text = "정신력Lv.1아이템";
                break;
            case items.mp2:
                text.text = "정신력Lv.2아이템";
                break;
            case items.mp3:
                text.text = "정신력Lv.3아이템";
                break;
            case items.attack:
                text.text = "화려한전투아이템";
                break;
            case items.mentalFocus:
                text.text = "정신의집중아이템";
                break;
            case items.lifeRestoration:
                text.text = "생명의회복아이템";
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
                        bool get = false;
                        switch (player.stats)
                        {
                            case playerStats.near:
                                foreach(var item in GameManager.Instance.inventory1)
                                {
                                    if(item.Key == this.item)
                                    {
                                        if (item.Value.stackble)
                                        {
                                            if(item.Value.count < item.Value.max)
                                            {
                                                get = true;
                                            }
                                        }
                                    }
                                }
                                break;
                            case playerStats.far:
                                foreach (var item in GameManager.Instance.inventory2)
                                {
                                    if (item.Key == this.item)
                                    {
                                        if (item.Value.stackble)
                                        {
                                            if (item.Value.count < item.Value.max)
                                            {
                                                get = true;
                                            }
                                        }
                                    }
                                }
                                break;
                            case playerStats.magic:
                                foreach (var item in GameManager.Instance.inventory2)
                                {
                                    if (item.Key == this.item)
                                    {
                                        if (item.Value.stackble)
                                        {
                                            if (item.Value.count < item.Value.max)
                                            {
                                                get = true;
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                        if (get)
                        {
                            ItemGet(player, true);
                            Destroy(gameObject);
                        }
                        else
                        {
                            GameManager.Instance.messageUI.Add("인벤토리에 자리가 부족합니다.", Color.red, true);
                        } 
                    }
                    else
                    {
                        bool get = false;
                        switch (player.stats)
                        {
                            case playerStats.near:
                                foreach (var item in GameManager.Instance.inventory1)
                                {
                                    if (item.Key == this.item)
                                    {
                                        if (item.Value.stackble)
                                        {
                                            if (item.Value.count < item.Value.max)
                                            {
                                                get = true;
                                            }
                                        }
                                    }
                                }
                                break;
                            case playerStats.far:
                                foreach (var item in GameManager.Instance.inventory2)
                                {
                                    if (item.Key == this.item)
                                    {
                                        if (item.Value.stackble)
                                        {
                                            if (item.Value.count < item.Value.max)
                                            {
                                                get = true;
                                            }
                                        }
                                    }
                                }
                                break;
                            case playerStats.magic:
                                foreach (var item in GameManager.Instance.inventory2)
                                {
                                    if (item.Key == this.item)
                                    {
                                        if (item.Value.stackble)
                                        {
                                            if (item.Value.count < item.Value.max)
                                            {
                                                get = true;
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                        if (get)
                        {
                            ItemGet(player, true);
                        }
                        else
                        {
                            ItemGet(player, false);
                        }
                            
                        Destroy(gameObject);
                    }
                }
            }
            
            
        }
    }

    private void ItemGet(Player player, bool stack)
    {
        switch (item)
        {
            case items.heal1:
                GameManager.Instance.messageUI.Add("생명력Lv.1아이템 획득", Color.green, true);
                break;
            case items.heal2:
                GameManager.Instance.messageUI.Add("생명력Lv.2아이템 획득", Color.green, true);
                break;
            case items.heal3:
                GameManager.Instance.messageUI.Add("생명력Lv.3아이템 획득", Color.green, true);
                break;
            case items.mp1:
                GameManager.Instance.messageUI.Add("정신력Lv.1아이템 획득", Color.green, true);
                break;
            case items.mp2:
                GameManager.Instance.messageUI.Add("정신력Lv.2아이템 획득", Color.green, true);
                break;
            case items.mp3:
                GameManager.Instance.messageUI.Add("정신력Lv.3아이템 획득", Color.green, true);
                break;
            case items.attack:
                GameManager.Instance.messageUI.Add("화려한전투아이템 획득", Color.green, true);
                break;
            case items.mentalFocus:
                GameManager.Instance.messageUI.Add("정신의집중아이템 획득", Color.green, true);
                break;
            case items.lifeRestoration:
                GameManager.Instance.messageUI.Add("생명의회복아이템 획득", Color.green, true);
                break;
        }
        if(!stack)
        {
            player.inventoryCount++;
        }
        
        InventoryGet(player, item, this.stack);
        
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
