using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
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
}
