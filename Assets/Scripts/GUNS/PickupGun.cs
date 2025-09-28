using SuperPupSystems.Helper;
using UnityEngine;

public class PickupGun : MonoBehaviour
{
    public int health = 10;
    public GunType gun;

    public float spinSpeed;
    public float amplitude = 0.5f; 
    public float frequency = 1f;   

    Vector3 startPos;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
           bool didwork = WeaponManager.instance.AttatchGun(gun);
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

    

    private void Start()
    {
        startPos = transform.position;
    }
    private void Update()
    {
        transform.position = startPos + Vector3.up * Mathf.Sin(Time.time * Mathf.PI * frequency) * amplitude; //Hover


        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f); //SPin
    }
}
