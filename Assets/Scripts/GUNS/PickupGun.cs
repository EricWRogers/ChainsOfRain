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


    Vector3 startPos;
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

    // private void Awake()
    // {
    //     RaycastHit hit;
    //     if (Physics.Raycast(transform.position, Vector3.down, out hit, 1000.0f, (int)mask))
    //     {
    //         startPos = hit.point;
    //     }
    //     else
    //     {
    //         startPos = transform.position;
    //     }
    // }

    private void Start()
    {
        timer = gameObject.GetComponent<Timer>();
    }

    private void Update()
    {
        // transform.position = (Vector3.up * heightOffset) + startPos + Vector3.up * Mathf.Sin(Time.time * Mathf.PI * frequency) * amplitude; //Hover
        // transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f); //Spin
        
        Debug.DrawRay(transform.position, Vector3.down * rayLength, Color.green);
        if(doRayCast)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, rayLength, (int)mask))
            {
                //start timer
                if(timer != null)
                    timer.StartTimer();
                HoverAndSpin();
            }
            else
            {
                transform.position -= new Vector3(transform.position.x, transform.position.y - Time.deltaTime * timeForce, transform.position.z);
            }
        }
    }

    private void HoverAndSpin()
    {
        transform.position = (Vector3.up * heightOffset) + Vector3.up * Mathf.Sin(Time.time * Mathf.PI * frequency) * amplitude;
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);
    }
}
