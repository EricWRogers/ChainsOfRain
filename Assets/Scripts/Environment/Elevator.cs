using UnityEngine;
using KinematicCharacterControler;

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
            other.GetComponent<PlayerMovement>().enabled = false;
            other.gameObject.transform.SetParent(transform);
        }
    }
}
