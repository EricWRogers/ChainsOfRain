using UnityEngine;

public class Carosel : MonoBehaviour
{
 
    public float speed = 5f;

    public bool positive = false;
    void Update()
    {
        if(positive)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
        }
        else
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
        }
    }
}

