using SuperPupSystems.Helper;
using UnityEngine;

public class LegPickup : MonoBehaviour
{
    public GameObject legPrefab;
    public int health = 10;
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

    public void AddHealth()
    {
        PlayerMovement.instance.gameObject.GetComponent<Health>().Heal(health);
    }
}
