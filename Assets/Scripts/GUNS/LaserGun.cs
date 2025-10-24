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
    public int maxSegments = 10;
    

    public bool firing = false;
    public override void Fire(Transform _firingPoint, GameObject _bulletPrefab)
    {

        if (burnedOut) return;
        
        Logger.instance.Log("Firing mah lazar!", Logger.LogType.Gun);
        firing = true;
        if (Physics.Raycast(firingPoint.position, firingPoint.forward, out RaycastHit hit, Mathf.Infinity, Physics.AllLayers) && damageTime >= 0.5f)
        {
            float distance = Vector3.Distance(hit.point, firingPoint.position);
                    
            int segments = Mathf.Max(1, maxSegments);
            float segmentLength = distance / segments;
            int pointCount = segments + 1;

            beam.positionCount = pointCount;

            Vector3 dir = (hit.point - firingPoint.position).normalized;
            Vector3 startPos = firingPoint.position;

            // set discrete points along the beam so the mesh doesn't stretch
            for (int i = 0; i < pointCount; i++)
            {
                beam.SetPosition(i, startPos + dir * (segmentLength * i));
            }


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
        if(!firing)
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
