using System;
using SuperPupSystems.Helper;
using UnityEngine;

public class LaserGun : Gunbase
{
   //MaxAmmo is our max charge.
   //Ammo is our current charge.
    public float overChargeWarning; //The number at which we warn the player we're approaching burnout.
    public bool burnedOut = false;
    public float chargeRate;
    public LineRenderer beam;
    private float damageTime;
    

    public bool firing = false;
    public override void Fire(Transform _firingPoint, GameObject _bulletPrefab)
    {
        //if (burnedOut) return; //Maybe we play some stuff to let the player know its burned here as well.

        Logger.instance.Log("Firing mah lazar!", Logger.LogType.Gun);
        firing = true;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, Mathf.Infinity, -1) && damageTime >= 0.5f)
        {
            beam.positionCount = 2;
            beam.SetPosition(0, transform.position);
            beam.SetPosition(1, hit.point);
            if (hit.transform.gameObject.tag == "Enemy")
            {
                hit.transform.gameObject.GetComponent<Health>().Damage(damage);
                damageTime = 0f;
            }
        }
    
        
        
    }

    new void Start()
    {
        base.Start();
        ammo = 0;
    }


    // Update is called once per frame
    new void Update()
    {
        beam.positionCount = 0;
        base.Update();
        if (!burnedOut)
        {
            if (ammo < maxAmmo)
            {
                if (firing)
                {
                    ammo++;

                }
            }
            else if (ammo >= overChargeWarning)
            {
                Logger.instance.Log("Warning! Weapon overheating!", Logger.LogType.Gun);
                //Play warning effect

            }
            if (ammo >= maxAmmo)
            {
                burnedOut = true;
            }

            if (!firing && ammo != 0)
            {
                ammo--;
            }


        }
        damageTime += Time.deltaTime;
    }

    public override void ReleaseFiring()
    {
        firing = false;
    }
}
