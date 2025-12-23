using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    [Header("Pickup")]
    public float pickupRadius = 1.2f;   // increase if you want it easier
    public string playerTag = "Player";

    Transform playerRoot;

    void Start()
    {
        var p = GameObject.FindGameObjectWithTag(playerTag);
        if (p != null) playerRoot = p.transform;
    }

    void Update()
    {
        if (playerRoot == null) return;

        // Flat/top-down distance (ignore Y)
        Vector3 a = transform.position; a.y = 0f;
        Vector3 b = playerRoot.position; b.y = 0f;

        if ((a - b).sqrMagnitude <= pickupRadius * pickupRadius)
        {
            PlayerInventory inv = playerRoot.GetComponent<PlayerInventory>();
            if (inv != null)
            {
                inv.hasKey = true;
                Debug.Log("KEY PICKED UP");
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("PlayerInventory missing on Player root.");
            }
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
#endif
}
