using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    public bool shouldBeOpen = false;
    private int activateOnce = 0;
    public Animator doorAnim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        doorAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(!isOpen && shouldBeOpen && activateOnce == 0)
            {
                Debug.Log("Open da Door");
            }
            else if(isOpen && !shouldBeOpen && activateOnce == 0)
            {
                Debug.Log("Close da Door");
            }
            activateOnce++;
        }
    }
}
