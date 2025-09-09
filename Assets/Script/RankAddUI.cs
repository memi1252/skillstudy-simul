using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class RankAddUI : MonoBehaviour
{
    public InputField initailInputField;
    public GameObject errorText;
    private Animator animator;

    private bool first = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }


    public void Show()
    {
        gameObject.SetActive(true);
        if (!first)
        {
            first = true;
        }
        else
        {
            animator.SetBool("Show", true);
        }
    }

    public void Registration()
    {
        if(initailInputField != null)
        {
            if(initailInputField.text.Length < 3)
            {
                StartCoroutine(error());
                return;
            }

            RankData data = new RankData();
            data.initail = initailInputField.text;
            data.score = GameManager.Instance.score;
            RankManager.instance.RankAdd(data);
            animator.SetBool("Show", false);
            StartCoroutine(Hide());
        }
    }

    IEnumerator Hide()
    {
        yield return new WaitForSecondsRealtime(2);
        gameObject.SetActive(false);
    }

    IEnumerator error()
    {
        errorText.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        errorText.SetActive(false);
    }
}
