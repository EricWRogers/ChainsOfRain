using SuperPupSystems.Helper;
using UnityEngine;

public class PickupGun : MonoBehaviour
{
    public int health = 10;
    public GunType gun;
    public float spinSpeed = 90f;
    public float hoverAmplitude = 0.25f;
    public float hoverFrequency = 2f;
    public LayerMask groundMask;
    public float hoverHeight = 1.36f;

    private Vector3 basePos;
 

    private void Start()
    {
       

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundMask))
        {
            basePos = hit.point + Vector3.up * hoverHeight;
            gameObject.transform.root.transform.position = basePos;
        }
        else
        {
            basePos = gameObject.transform.root.transform.position;
        }
    }

    private void Update()
    {
        // Spin
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);

        // Hover
        float hoverOffset = Mathf.Sin(Time.time * hoverFrequency) * hoverAmplitude;
        transform.position = basePos + Vector3.up * hoverOffset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && WeaponManager.instance.AttatchGun(gun))
        {
            PlayerMovement.instance.GetComponent<Health>().Heal(health);
            Destroy(gameObject);
        }
    }

    public void AddHealth()
    {
        PlayerMovement.instance.gameObject.GetComponent<Health>().Heal(health);
    }

   
}
