using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footPrint : MonoBehaviour
{
    public GameObject footPrintPrefab;
    public Transform pos1;
    public Transform pos2;

    public Transform footPrintPoint;

    public void right()
    {
        var foot = Instantiate(footPrintPrefab);
        foot.transform.position = pos1.position;
        foot.transform.rotation = pos1.rotation;
        foot.transform.SetParent(footPrintPoint);
    }

    public void left()
    {
        var foot = Instantiate(footPrintPrefab);
        foot.transform.position = pos2.position;
        foot.transform.rotation = pos2.rotation;
        foot.transform.SetParent(footPrintPoint);
    }
}
