using UnityEngine;

public class TriggerTester : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER TEST: " + other.name);
    }
}
