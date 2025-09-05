using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventory1;
    public GameObject inventory2;
    public GameObject inventory3;

    public GameObject inventorySlot;
    
  

    private void Start()
    {
        //Hide
    }

    public void Show()
    {
        gameObject.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameManager.Instance.cameraMove = false;
        UpdateUI();
    }

    public void Hide()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.Instance.cameraMove = true;
        gameObject.SetActive(false);
    }

    private void UpdateUI()
    {
        foreach (var item in GameManager.Instance.inventory1)
        {
           var slot = Instantiate(inventorySlot, inventory1.transform.GetChild(1).transform, true);
           slot.GetComponent<InventorySlot>().Set(item.Key, item.Value.count);
           slot.GetComponent<InventorySlot>().index = 1;
        }
        foreach (var item in GameManager.Instance.inventory2)
        {
            var slot = Instantiate(inventorySlot, inventory2.transform.GetChild(1).transform, true);
            slot.GetComponent<InventorySlot>().Set(item.Key, item.Value.count);
            slot.GetComponent<InventorySlot>().index = 2;
        }
        foreach (var item in GameManager.Instance.inventory3)
        {
            var slot = Instantiate(inventorySlot, inventory3.transform.GetChild(1).transform, true);
            slot.GetComponent<InventorySlot>().Set(item.Key, item.Value.count);
            slot.GetComponent<InventorySlot>().index = 3;
        }
        
        Time.timeScale = 0;
    }


}
