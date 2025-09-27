using UnityEngine;

public class PickupGun : MonoBehaviour
{

    public GameObject gunPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            WeaponManager.instance.AttatchGun(gunPrefab);
            Destroy(gameObject);
        }
    }
}
