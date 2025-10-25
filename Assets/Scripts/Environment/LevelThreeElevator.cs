using UnityEngine;
using KinematicCharacterControler;

public class LevelThreeElevator : MonoBehaviour
{
    public float speedForce = 1.0f;
    public float moveDistance = 10.0f;
    public float waitAtTopTimer = 2.0f;
    public Transform topPoint;
    public Transform bottomPoint;
    private GameObject player;
    private bool isPlayerOnElevator = false;
    private bool movingUp = false;
    private bool movingDown = false;
    private bool waiting = false;
    private float startY;
    private float waitTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startY = transform.position.y;
        player = FindFirstObjectByType<WeaponManager>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (movingUp)
        {
            transform.Translate(Vector3.up * speedForce * Time.deltaTime, Space.World);

            if (transform.position.y >= startY + moveDistance)
            {
                transform.position = new Vector3(transform.position.x, startY + moveDistance, transform.position.z);
                movingUp = false;
                waiting = true;
                waitTimer = 0f;

                // Allow player to walk off
                if (isPlayerOnElevator && player != null)
                {
                    player.transform.SetParent(null);
                    player.GetComponent<PlayerMovement>().enabled = true;
                }
            }
        }

        // --- Wait at top ---
        if (waiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitAtTopTimer)
            {
                waiting = false;
                if (!isPlayerOnElevator)
                {
                    movingDown = true;
                }
            }
        }

        // --- Move elevator down ---
        if (movingDown)
        {
            transform.Translate(Vector3.down * speedForce * Time.deltaTime);

            if (transform.position.y <= startY)
            {
                transform.position = new Vector3(transform.position.x, startY, transform.position.z);
                movingDown = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerOnElevator = true;
            player = other.gameObject;
            
            Vector3 offsetPos = player.transform.position;
            offsetPos.y = transform.position.y + 3.0f; 
            player.transform.position = offsetPos;

            if (!movingUp && !movingDown && !waiting)
            {
                movingUp = true;
                if (other.GetComponent<PlayerMovement>() != null)
                    player.GetComponent<PlayerMovement>().enabled = false;
                player.transform.SetParent(transform);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnElevator = false;
            player.transform.SetParent(null);
            player.GetComponent<PlayerMovement>().enabled = true;

            if (!waiting)
                movingDown = true;
        }
    }
}
