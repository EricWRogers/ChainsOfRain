using UnityEngine;

public class SemiAuto : Gunbase
{
    public override void Fire(Transform _firingPoint, GameObject _bulletPrefab)
    {
        GameObject temp = Instantiate(_bulletPrefab, _firingPoint);

        UpdateDamage(damage, temp); //Just to keep things in line.
    }
}
