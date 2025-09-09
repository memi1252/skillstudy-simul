using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RankUI : MonoBehaviour
{
    public GameObject rankSlot;
    public Transform rankPanel;
    private Animator animator;

    private List<GameObject> DestrotyObject = new List<GameObject>();
    bool first = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        if(!first)
        {
            first = true;
        }
        else
        {
            animator.SetBool("Show", true);
        }
        if(DestrotyObject.Count > 0 )
        {
            for(int i = DestrotyObject.Count - 1; i >= 0; i--)
            {
                Destroy( DestrotyObject[i]);
            }
            DestrotyObject.Clear();
        }
        RankManager.instance.Load();
        RankManager.instance.data = RankManager.instance.data.OrderByDescending(rank => rank.score).ToList();
        for(int i = 0; i< 10; i++)
        {
            var slot = Instantiate(rankSlot);
            slot.transform.SetParent(rankPanel);
            slot.GetComponent<RankSlot>().Set(i+1, RankManager.instance.data[i].initail, RankManager.instance.data[i].score);
            DestrotyObject.Add(slot);
        }

    }

    public void Hide()
    {
        animator.SetBool("Show", false);
        Invoke("Hide2", 2);
    }

    public void Hide2()
    {
        gameObject.SetActive(false);
    }
}
