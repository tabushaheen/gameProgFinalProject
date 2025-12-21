using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorInteraction : MonoBehaviour
{
    public GameObject winScreen;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inv = other.GetComponent<PlayerInventory>();
            if (inv != null && inv.hasKey)
            {
                winScreen.SetActive(true);
                SceneManager.LoadScene("WinScreen");
            }
        }
    }
}