using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public GameObject pung;

    public void Hit()
    {
        transform.GetComponent<MeshRenderer>().enabled = false;
        pung.SetActive(true);
        if (GameManager.Instance.stage == 2)
        {
            GameManager.Instance.Stage2DestoyObjectCount++;
        }
        StartCoroutine(Pung());
    }

    IEnumerator Pung()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
