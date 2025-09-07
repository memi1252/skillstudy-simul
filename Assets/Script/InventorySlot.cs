using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    private Image image;
    private Text countText;

    public Sprite[] itemImages;
    private int count = 0;
    private items item;
    public int index;

    public void Set(items item, int count)
    {
        this.item = item;
        this.count = count;
        countText = GetComponentInChildren<Text>();
        image = GetComponent<Image>();
        switch (this.item)
        {
            case items.heal1:
                image.sprite = itemImages[0];
                break;
            case items.heal2:
                image.sprite = itemImages[0];
                image.color = Color.blue;
                break;
            case items.heal3:
                image.sprite = itemImages[0];
                image.color = Color.green;
                break;
            case items.mp1:
                image.sprite = itemImages[1];
                break;
            case items.mp2:
                image.sprite = itemImages[1];
                image.color = Color.red;
                break;
            case items.mp3:
                image.sprite = itemImages[1];
                image.color = Color.yellow;
                break;
        }

        countText.text = count.ToString();

    }

    public void Use()
    {
        switch (item)
        {
            case items.heal1:
                GameManager.Instance.players[index - 1].hp += GameManager.Instance.players[index - 1].hp * 0.1f;
                break;
            case items.heal2:
                GameManager.Instance.players[index - 1].hp += GameManager.Instance.players[index - 1].hp * 0.3f;
                break;
            case items.heal3:
                GameManager.Instance.players[index - 1].hp += GameManager.Instance.players[index - 1].hp * 0.5f;
                break;
            case items.mp1:
                GameManager.Instance.players[index - 1].mp += GameManager.Instance.players[index - 1].mp * 0.1f;
                break;
            case items.mp2:
                GameManager.Instance.players[index - 1].mp += GameManager.Instance.players[index - 1].mp * 0.3f;
                break;
            case items.mp3:
                GameManager.Instance.players[index - 1].mp += GameManager.Instance.players[index - 1].mp * 0.5f;
                break;
            case items.attack:
                switch (GameManager.Instance.players[index - 1].stats)
                {
                    case playerStats.near:
                        if (SkillManager.instance.spectacularBattle1)
                        {
                            return;
                        }
                        break;
                    case playerStats.far:
                        if (SkillManager.instance.spectacularBattle2)
                        {
                            return;
                        }
                        break;
                    case playerStats.magic:
                        if (SkillManager.instance.spectacularBattle3)
                        {
                            return;
                        }
                        break;
                }
                GameManager.Instance.players[index-1].spectacularBattle = true;
                break;
            case items.mentalFocus:
                switch (GameManager.Instance.players[index - 1].stats)
                {
                    case playerStats.near:
                        if (SkillManager.instance.mentalFocus1)
                        {
                            return;
                        }
                        break;
                    case playerStats.far:
                        if (SkillManager.instance.mentalFocus2)
                        {
                            return;
                        }
                        break;
                    case playerStats.magic:
                        if (SkillManager.instance.mentalFocus3)
                        {
                            return;
                        }
                        break;
                }
                GameManager.Instance.players[index-1].mentalFocus = true;
                break;
            case items.lifeRestoration:
                Player player = GameManager.Instance.players[index - 1];
                if (player.isDie)
                {
                    player.isDie = false;
                    player.hp = player.maxHp;
                    player.GetComponentInChildren<Animator>().SetTrigger("NoDie");
                    player.hpText.color = Color.green;
                    player.hpSlider.color = Color.green;
                }
                else
                {
                    Collider[] nearCol = Physics.OverlapSphere(player.transform.position, 7);
                    foreach (Collider col in nearCol)
                    {
                        if(col.TryGetComponent<Player>(out var player1))
                        {
                            if (player1.isDie)
                            {
                                player1.isDie= false;
                                player1.hp = player1.maxHp;
                                player1.GetComponentInChildren<Animator>().SetTrigger("NoDie");
                            }
                        }
                    }
                }
                    break;
        }
        switch (index)
        {
            case 1:
                if (GameManager.Instance.inventory1.ContainsKey(item))
                {
                    var inventoryValue = GameManager.Instance.inventory1[item];
                    inventoryValue.count--;
                    GameManager.Instance.inventory1[item] = inventoryValue;

                }
                count--;
                if (count <= 0)
                {
                    GameManager.Instance.inventory1.Remove(item);
                    GameManager.Instance.players[0].inventoryCount--;
                    Destroy(gameObject);
                }

                countText.text = count.ToString();
                break;
            case 2:
                if (GameManager.Instance.inventory2.ContainsKey(item))
                {
                    var inventoryValue = GameManager.Instance.inventory2[item];
                    inventoryValue.count--;
                    GameManager.Instance.inventory2[item] = inventoryValue;
                    
                }
                count--;
                if (count <= 0)
                {
                    GameManager.Instance.inventory2.Remove(item);
                    GameManager.Instance.players[1].inventoryCount--;
                    Destroy(gameObject);
                }

                countText.text = count.ToString();
                break;
            case 3:
                if (GameManager.Instance.inventory3.ContainsKey(item))
                {
                    var inventoryValue = GameManager.Instance.inventory3[item];
                    inventoryValue.count--;
                    GameManager.Instance.inventory3[item] = inventoryValue;
                    
                }
                count--;
                if (count <= 0)
                {
                    GameManager.Instance.inventory3.Remove(item);
                    GameManager.Instance.players[2].inventoryCount--;
                    Destroy(gameObject);
                }

                countText.text = count.ToString();
                break;
        }
    }
}
