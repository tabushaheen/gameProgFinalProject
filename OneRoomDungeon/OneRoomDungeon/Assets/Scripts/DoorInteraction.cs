using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public GameObject winScreen;

    private bool hasWon = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasWon) return;

        PlayerInventory inv = other.GetComponentInParent<PlayerInventory>();
        if (inv == null) return;

        if (!inv.hasKey) return;

        hasWon = true;

        Debug.Log("Player with key passed");
        if (winScreen != null)
            winScreen.SetActive(true);
        Time.timeScale = 0f;
    }
}
