using UnityEngine;

public class LegPickup : MonoBehaviour
{
    public GameObject legPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LegManager.instance.AttatchLeg(legPrefab);
            Destroy(gameObject);
        }
    }
}
