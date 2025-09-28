using UnityEngine;

public class AutoGun : Gunbase
{
    public float fireRate = 1f;


    public float nextFireTime;

    public override void Fire(Transform _firingPoint, GameObject _bulletPrefab)
    {
        if(Time.time >= nextFireTime && ammo != 0)
        {
            onFire.Invoke();
            GameObject temp = Instantiate(_bulletPrefab, _firingPoint.position, _firingPoint.rotation);

            UpdateDamage(damage, temp); //Just to keep things in line.

            ammo--;
        }
    }

    public override void ReleaseFiring()
    {
        
    }
}
