using SuperPupSystems.Helper;
using UnityEngine;

public class LegPickup : MonoBehaviour
{
    public LegType legType;
    public int health = 10;

    public float spinSpeed;
    public float amplitude = 0.5f;
    public float frequency = 1f;
    public float heightOffset = 1.0f;
    public GameObject destroyTarget;
    Vector3 startPos;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           bool didwork = LegManager.instance.AttatchLeg(legType);
            if (didwork)
            {
                Destroy(destroyTarget);
            }
        }
    }

    public void AddHealth()
    {
        PlayerMovement.instance.gameObject.GetComponent<Health>().Heal(health);
    }

    private void Awake()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1000.0f))
        {
            startPos = hit.point;
        }
        else
        {
            startPos = transform.position;
        }
    }
    private void Update()
    {
        transform.position = (Vector3.up * heightOffset) + startPos + Vector3.up * Mathf.Sin(Time.time * Mathf.PI * frequency) * amplitude; //Hover


        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f); //SPin
    }
}
