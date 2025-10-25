using UnityEngine;
using KinematicCharacterControler;

public class Elevator : MonoBehaviour
{
    public bool isBossDead = false;
    public bool isThirdLevelElevator = false;
    public float offset = 1.2f;
    public Animator elevator;

    public void BossDead()
    {
        isBossDead = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isThirdLevelElevator)
            {
                elevator.Play("GoToFirstFloorElevator");
                other.GetComponent<PlayerMovement>().enabled = false;
            other.gameObject.transform.SetParent(transform);
            }
            else if (isBossDead)
            {
                elevator.Play("ElevatorMove");
                other.GetComponent<PlayerMovement>().enabled = false;
                other.gameObject.transform.SetParent(transform);
            }
            
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Unset Parent Transform
        }
    }
}
