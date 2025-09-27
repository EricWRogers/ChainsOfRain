using UnityEngine;
using SuperPupSystems.Helper;

public class Train : MonoBehaviour
{
    public int amountOfDamage = 25;
    public Animator train;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAnimation(string _name)
    {
        train.Play(_name);

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<Health>().Damage(amountOfDamage);
        }
    }
}
