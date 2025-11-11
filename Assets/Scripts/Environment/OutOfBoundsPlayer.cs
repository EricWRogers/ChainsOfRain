using UnityEngine;

public class OutOfBoundsPlayer : MonoBehaviour
{
    public Transform safeLocation;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            other.transform.position = safeLocation.position; 
        }
    }
}
