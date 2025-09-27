using UnityEngine;

public class LaserGun : Gunbase
{
   //MaxAmmo is our max charge.
   //Ammo is our current charge.
    public float overChargeWarning; //The number at which we warn the player we're approaching burnout.
    public bool burnedOut = false; 

    public bool firing = false;
    public override void Fire(Transform _firingPoint, GameObject _bulletPrefab)
    {
        //if (burnedOut) return; //Maybe we play some stuff to let the player know its burned here as well.

        Logger.instance.Log("Firing mah lazar!", Logger.LogType.Gun);
        firing = true;
        
        
    }

    private void Start()
    {
        ammo = 0;
    }


    // Update is called once per frame
    void Update()
    {
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
            else if (ammo >= maxAmmo)
            {
                burnedOut = true;
            }

            if (!firing && ammo != 0)
            {
                ammo--;
            }


        }
    }
}
