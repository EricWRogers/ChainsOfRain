using SuperPupSystems.Helper;
using UnityEngine;
using UnityEngine.Events;

public abstract class Gunbase : MonoBehaviour
{
    public bool leftHanded = false;

    public bool rightHanded = false;


    public bool semi = false;
    public bool auto = false;

    public GameObject bulletPrefab;

    public GameObject jettisonPrefab;

    public float jettisonForce;
    public int limitedAmmo = 10;
    public int ammo = 10;

    public int damage = 1;

    public Transform firingPoint;
    private KeyCode activeHand => leftHanded ? KeyCode.Mouse0 : KeyCode.Mouse1;

    private InputType inputMode => semi ? InputType.GetKeyDown : InputType.GetKey;



    public UnityEvent onJettison;
    private void Update()
    {
        switch (inputMode)
        {
            case InputType.GetKeyDown:
                if (Input.GetKeyDown(activeHand))
                {
                    Debug.Log("Firing!");
                    Fire(firingPoint, bulletPrefab);
                }
                break;
            case InputType.GetKey:
                if (Input.GetKey(activeHand))
                {
                    Debug.Log("Firing (holding)!");
                    Fire(firingPoint, bulletPrefab);
                }
                break;
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            ammo = limitedAmmo;
        }
    }

    public abstract void Fire(Transform _firingPoint, GameObject _bulletPrefab);


    public void UpdateDamage(int _value, GameObject _bullet)
    {
        _bullet.GetComponent<Bullet>().damage = _value;
    }


    public void Jettison()
    {
        onJettison.Invoke();

       GameObject temp = Instantiate(jettisonPrefab, gameObject.transform.position, gameObject.transform.rotation);

        temp.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * jettisonForce, ForceMode.Impulse);
    }
    private enum InputType
    {
        GetKey,
        GetKeyDown
    }

}
