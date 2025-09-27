using UnityEngine;
using UnityEngine.Rendering;

public class SemiAuto : Gunbase
{
    public float fireRate = 1f;

   
    public float nextFireTime;

    
    public override void Fire(Transform _firingPoint, GameObject _bulletPrefab)
    {
        if(Time.time >= nextFireTime && ammo !=0)
        {
            GameObject temp = Instantiate(_bulletPrefab, _firingPoint);

            UpdateDamage(damage, temp); //Just to keep things in line.

            ammo--;
        }


       
    }
}
