using SuperPupSystems.Helper;
using UnityEngine;

public class AcidicBarrell : MonoBehaviour
{
    public GameObject parent;
    public int damage = 2;
    float tickTime = 2f;
    float timer = 10f;

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Disappear();
        }
    }

    public void Disappear()
    {
        Destroy(parent);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            tickTime -= Time.deltaTime;

            if (tickTime <= 0f)
            {
                other.GetComponent<Health>().Damage(damage);
                tickTime = 2.0f;
            }
        }
    }
}
