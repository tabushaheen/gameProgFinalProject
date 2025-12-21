using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        PlayerInventory inventory = other.GetComponent<PlayerInventory>();
        
        if (inventory != null)
        {
            inventory.hasKey = true;
            Debug.Log("Key picked up hasKey = " + inventory.hasKey);

            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("Player collided but no inventory");
    }
}
}