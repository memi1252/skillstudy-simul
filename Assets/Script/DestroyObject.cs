using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public GameObject pung;
    public float range = 6;

    private void Update()
    {
        Collider[] nearCol = Physics.OverlapSphere(transform.position, range);
        foreach (Collider col in nearCol)
        {
            if (col.CompareTag("Player"))
            {
                if(col.TryGetComponent<Player>(out Player player))
                {
                    if (player.root)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            transform.GetComponent<MeshRenderer>().enabled = false;
                            pung.SetActive(true);
                            StartCoroutine(Pung());
                        }
                    }
                }
            }
        }
    }

    IEnumerator Pung()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
