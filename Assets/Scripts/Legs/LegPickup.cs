using UnityEngine;

public class LegPickup : MonoBehaviour
{
    public GameObject legPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           bool didwork = LegManager.instance.AttatchLeg(legPrefab);
            if (didwork)
            {
                Destroy(gameObject);
            }
        }
    }
}
