using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageUI : MonoBehaviour
{
    public GameObject messageSlot;
    public Transform Panel;

    private void Start()
    {
        Add("테스트중테스트중");
        Add("테스트중테스트중", Color.red);
        Add("테스트중테스트중", Color.red, true);
    }

    public void Add(string text)
    {
        var slot = Instantiate(messageSlot);
        slot.transform.SetParent(Panel);
        slot.GetComponent<MessageSlot>().Set(text);
    }
    public void Add(string text, Color color)
    {
        var slot = Instantiate(messageSlot);
        slot.transform.SetParent(Panel);
        slot.GetComponent<MessageSlot>().Set(text);
        slot.GetComponent<MessageSlot>().text.color = color;
    }
    public void Add(string text, Color color, bool bold)
    {
        var slot = Instantiate(messageSlot);
        slot.transform.SetParent(Panel);
        slot.GetComponent<MessageSlot>().Set(text);
        slot.GetComponent<MessageSlot>().text.color = color;
        if (bold)
        {
            slot.GetComponent<MessageSlot>().text.fontStyle = FontStyle.Bold;
        }
        
    }
}
