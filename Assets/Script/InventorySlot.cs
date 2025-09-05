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
            case items.heal:
                image.sprite = itemImages[0];
                //스프라이트 변경
                break;
            case items.mp:
                image.sprite = itemImages[1];
                break;
        }

        countText.text = count.ToString();

    }

    public void Use()
    {
        switch (item)
        {
            case items.heal:
                GameManager.Instance.players[index-1].hp += 20;
                break;  
            case items.mp:
                GameManager.Instance.players[index-1].mp += 20;
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
                    Destroy(gameObject);
                }

                countText.text = count.ToString();
                break;
        }
    }
}
