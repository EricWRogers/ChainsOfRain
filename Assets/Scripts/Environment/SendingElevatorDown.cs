using UnityEngine;

public class SendingElevatorDown : MonoBehaviour
{
    public Animator anim;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetTrigger("isMoving");
        }
    }
}
