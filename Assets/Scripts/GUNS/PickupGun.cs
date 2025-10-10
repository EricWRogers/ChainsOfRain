using SuperPupSystems.Helper;
using UnityEngine;
using UnityEngine.UI;

public class PickupGun : MonoBehaviour
{
    public int health = 10;
    public GunType gun;

    public float spinSpeed;
    public float amplitude = 0.5f; 
    public float frequency = 1f;
    public float heightOffset = 1.0f;
    public LayerMask mask;

    public bool doRayCast = true;
    public float timeForce = 1.0f;
    private float rayLength = 1.25f;
    private Timer timer;


    Vector3 groundPos;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
           bool didwork = WeaponManager.instance.AttatchGun(gun);
            if (didwork)
            {
                AddHealth();
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
        timer = gameObject.GetComponent<Timer>();
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down * rayLength, Color.green);
        if (doRayCast)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, rayLength, (int)mask))
            {
                //start timer
                if (timer != null)
                    timer.StartTimer();
                HoverAndSpin();
            }
            else
            {
                transform.Translate(Vector3.down * timeForce * Time.deltaTime);
            }
        }
        else
        {
            HoverAndSpin();
        }
        
    }

    private void HoverAndSpin()
    {
        if(groundPos == new Vector3(0f, 0f, 0f))
        {
            groundPos = transform.position;
        }
        float newY = groundPos.y + heightOffset + Mathf.Sin(Time.time * Mathf.PI * frequency) * amplitude;
        transform.position = new Vector3(groundPos.x, newY, groundPos.z); // only need to hover on the y axis
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);
    }

    public void Disappear()
    {
        
    }
}
