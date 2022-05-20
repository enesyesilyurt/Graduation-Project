using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerManager player = other.GetComponent<PlayerManager>();
        if (player != null)
        {
            Collect(player);
        }
    }

    protected virtual void Collect(PlayerManager player)
    {
        gameObject.SetActive(false);
    }
}
