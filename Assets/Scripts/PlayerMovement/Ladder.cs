using UnityEngine;

public class Ladder : MonoBehaviour
{

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerMovement.instance.onLadder = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerMovement.instance.onLadder = false;
        }
    }
}
