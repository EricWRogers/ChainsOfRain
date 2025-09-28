using SuperPupSystems.Helper;
using UnityEngine;

public class PickupGun : MonoBehaviour
{
    public int health = 10;
    public GameObject gunPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
           bool didwork = WeaponManager.instance.AttatchGun(gunPrefab);
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
