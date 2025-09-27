using UnityEngine;

public class PickupGun : MonoBehaviour
{

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
}
