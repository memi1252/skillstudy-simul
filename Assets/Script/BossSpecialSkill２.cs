using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossSpecialSkill２ : MonoBehaviour
{
    public Image back;

    int index = 0;
    bool use;
    float currentTime = 0;

    private void Update()
    {
        if(index == 5)
        {
            index = 0;
        }
        if (use)
        {
            currentTime += Time.deltaTime;
            if(currentTime >= 3)
            {
                use = false;
                index++;
                gameObject.SetActive(false);
            }
        }
    }

    public void Use()
    {
        use = true;
        gameObject.SetActive(true);
        switch(index)
        {
            case 0:
                back.color = Color.gray;
                break;
            case 1:
                back.color = Color.red;
                break;
            case 2:
                back.color = Color.yellow;
                break;
            case 3:
                back.color = Color.blue;
                break;
           case 4:
                back.color = Color.green;
                break;
        }
    }


}
