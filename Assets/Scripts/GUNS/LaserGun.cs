using System;
using SuperPupSystems.Helper;
using UnityEngine;
using UnityEngine.Events;
public class LaserGun : Gunbase
{
   //MaxAmmo is our max charge.
   //Ammo is our current charge.
    public float overChargeWarning; //The number at which we warn the player we're approaching burnout.
    public bool burnedOut = false;
    public float chargeRate;
    public LineRenderer beam;
    private float damageTime;
    public float damageInterval = 0.25f;
    public int maxSegments = 10;
    public LayerMask mask;


    public UnityEvent onAttatch;

    public bool firing = false;
    private float lastDistance = 10.0f;
    public override void Fire(Transform _firingPoint, GameObject _bulletPrefab)
    {

        if (burnedOut) return;
        
        Logger.instance.Log("Firing mah lazar!", Logger.LogType.Gun);
        firing = true;

        // deal damage and get distance
        if (Physics.Raycast(firingPoint.position, firingPoint.forward, out RaycastHit hit, Mathf.Infinity, mask) && damageTime >= damageInterval)
        {
            lastDistance = Vector3.Distance(hit.point, firingPoint.position);

            if (hit.transform.gameObject.GetComponent<Health>())
            {
                hit.transform.gameObject.GetComponent<Health>().Damage(damage);
                damageTime = 0f;
            }
        }
        else
        {
            if (Physics.Raycast(firingPoint.position, firingPoint.forward, out RaycastHit ehit, Mathf.Infinity, Physics.AllLayers))
                lastDistance = Vector3.Distance(ehit.point, firingPoint.position);
        }

        // update lazer
        int segments = Mathf.Max(1, maxSegments);
        float segmentLength = lastDistance / segments;
        int pointCount = segments + 1;

        beam.positionCount = pointCount;

        Vector3 dir = firingPoint.forward;
        Vector3 startPos = firingPoint.position;

        // set discrete points along the beam so the mesh doesn't stretch
        for (int i = 0; i < pointCount; i++)
        {
            beam.SetPosition(i, startPos + dir * (segmentLength * i));
        }
        
        
    }

    new void Start()
    {
        base.Start();
        ammo = 0;
    }


    // Update is called once per frame
    void LateUpdate()
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
