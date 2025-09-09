using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossSpecialSkill1 : MonoBehaviour
{
    public Text timerText;
    public GameObject[] gameObjects;


    private float currentTime = 3;


    bool use;

    private void Update()
    {
        if (use)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                use = false;
                currentTime = 3;
                foreach (GameObject go in gameObjects)
                {
                    if (go != null)
                    {
                        go.SetActive(true);
                    }
                }
                gameObject.SetActive(false);
            }
        }
    }

    public void Use()
    {
        use = true;
        gameObject.SetActive(true);
        foreach (GameObject go in gameObjects)
        {
            if (go != null)
            {
                if (go.TryGetComponent<InventoryUI>(out var inventory))
                {
                    inventory.Hide();
                }
                go.SetActive(false);
            }
        }
    }
}
