using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    public Gunbase Weapon;
    public TextMeshProUGUI ammoCounterText;

    void Update()
    {
        ammoCounterText.text = (Weapon.ammo + "/" + Weapon.maxAmmo);
    }
}
