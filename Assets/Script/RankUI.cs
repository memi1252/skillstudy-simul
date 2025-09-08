using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RankUI : MonoBehaviour
{
    public GameObject rankSlot;
    public Transform rankPanel;
    private int index =1;

    private List<GameObject> DestrotyObject = new List<GameObject>();


    public void Show()
    {
        gameObject.SetActive(true);
        if(DestrotyObject.Count > 0 )
        {
            for(int i = DestrotyObject.Count - 1; i >= 0; i--)
            {
                Destroy( DestrotyObject[i]);
            }
            DestrotyObject.Clear();
        }
        index = 1;
        RankManager.instance.Load();
        RankManager.instance.data = RankManager.instance.data.OrderByDescending(rank => rank.score).ToList();
        foreach (var rank in RankManager.instance.data)
        {
            var slot = Instantiate(rankSlot);
            slot.transform.SetParent(rankPanel);
            slot.GetComponent<RankSlot>().Set(index, rank.initail, rank.score);
            DestrotyObject.Add(slot);
            index++;
        }
    }
}
