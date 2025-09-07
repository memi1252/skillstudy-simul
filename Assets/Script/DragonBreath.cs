using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBreath : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(other.TryGetComponent<Player>(out Player player))
            {
                player.hp -= 5f * Time.deltaTime;
            }
        }
    }
}
