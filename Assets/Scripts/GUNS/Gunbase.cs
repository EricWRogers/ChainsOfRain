using SuperPupSystems.Helper;
using UnityEngine;

public abstract class Gunbase : MonoBehaviour
{
    public bool leftHanded = false;

    public bool rightHanded = false;


    public GameObject bulletPrefab;

    public int damage = 1;

    public Transform firingPoint;
    private KeyCode activeHand => leftHanded ? KeyCode.Mouse0 : KeyCode.Mouse1;

    
    private void Update()
    {
        if(Input.GetKeyDown(activeHand))
        {
            Debug.Log("Firing!");
            Fire(firingPoint, bulletPrefab);
        }
    }

    public abstract void Fire(Transform _firingPoint, GameObject _bulletPrefab);


    public void UpdateDamage(int _value, GameObject _bullet)
    {
        _bullet.GetComponent<Bullet>().damage = _value;
    }
}
