using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class SemiAuto : Gunbase
{
    public float fireRate = 1f;

   [SerializeField] VisualEffect muzzleFlash;
   
    public float nextFireTime;

    public UnityEvent onAttatch;

    public override void Fire(Transform _firingPoint, GameObject _bulletPrefab)
    {
        if(Time.time >= nextFireTime && ammo !=0)
        {
            onFire.Invoke();
            PlayMuzzleFlash();
            GameObject temp = Instantiate(_bulletPrefab, _firingPoint.position, _firingPoint.rotation);

            UpdateDamage(damage, temp); //Just to keep things in line.

            ammo--;
        }


       
    }


void PlayMuzzleFlash()
    {
        muzzleFlash.Play();
    }
    public override void ReleaseFiring()
    {
       
    }
}
