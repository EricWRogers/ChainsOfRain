using UnityEngine;
using SuperPupSystems.Helper;
using KinematicCharacterControler;

public class Train : MonoBehaviour
{
    public int amountOfDamage = 25;
    public float knockBackStrength = 20f;
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
            Vector3 dir = other.gameObject.transform.position-transform.position;
            dir.y = 0;
            PlayerMovement.instance.KnockBack(dir, knockBackStrength);
        }
        if(other.CompareTag("Enemy"))
        {
            Debug.Log("The name is " + other.gameObject.name);
            other.GetComponent<Health>().Kill();
        }
    }
}
