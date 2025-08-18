using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageSlot : MonoBehaviour
{
    public Text text;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(destroy());
    }

    public void Set(string text)
    {
        this.text.text = text;
    }
    
    IEnumerator destroy()
    {
        yield return new WaitForSeconds(1.5f);
        animator.SetTrigger("Hide");
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
