using UnityEngine;

public class Elevator : MonoBehaviour
{
    public bool isBossDead = false;
    public float offset = 1.2f;
    public Animator elevator;

    public void BossDead()
    {
        isBossDead = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && isBossDead)
        {
            elevator.Play("ElevatorMove");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && isBossDead)
        {
            other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x, transform.position.y + offset,other.gameObject.transform.position.z);
        }
    }
}
