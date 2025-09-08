using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankSlot : MonoBehaviour
{
    public Text rankNumberText;
    public Text initalText;
    public Text pointText;


    public void Set(int index, string inital, int point)
    {
        rankNumberText.text = $"{index}.";
        initalText.text = inital;
        pointText.text = $"{point}Á¡";
    }
}
