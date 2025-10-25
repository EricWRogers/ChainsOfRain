using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class AutoGun : Gunbase
{
    public float fireRate = 1f;
    [SerializeField] VisualEffect muzzleFlash;
    public UnityEvent onAttatch;

    public float nextFireTime;
    public bool firing = false;
    private bool wasFiring = false;
    public override void Fire(Transform _firingPoint, GameObject _bulletPrefab)
    {
    
        if(ammo == 0)
        {
            ReleaseFiring();
        }

        if (attatched && Time.time >= nextFireTime && ammo > 0)
        {
            nextFireTime = Time.time + fireRate;
        if (firing && !wasFiring)
        {
        
         onFire.Invoke(); 
        }
        wasFiring = firing;
        firing = true;
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
        firing = false;
        onRelease.Invoke();
    }
}
