using SuperPupSystems.Helper;
using UnityEngine;

public class BulletHitEffect : MonoBehaviour
{
    public GameObject hitParticle;
    public float destoryTime = 2f;

    public void SpawnHitEffect()
    {
        if (hitParticle == null) return;

        Bullet bullet = GetComponent<Bullet>();
        Quaternion rot = Quaternion.LookRotation(bullet.hitInfo.normal);
        var temp = Instantiate(hitParticle, bullet.hitInfo.point, rot);
        Destroy(temp, destoryTime);
    }

}
