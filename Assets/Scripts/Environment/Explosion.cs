using SuperPupSystems.Helper;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public int damage;
    public GameObject parent;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            other.GetComponent<Health>().Damage(damage);
            Destroy(parent);
        }
    }
}
