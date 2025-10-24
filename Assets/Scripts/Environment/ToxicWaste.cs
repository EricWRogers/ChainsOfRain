using UnityEngine;
using SuperPupSystems.Helper;

public class ToxicWaste : MonoBehaviour
{
    public int damage = 2;
    float tickTime = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

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
