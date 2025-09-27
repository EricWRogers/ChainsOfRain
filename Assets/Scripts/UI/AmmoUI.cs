using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    public GameObject Weapon;
    public TextMeshProUGUI ammoCounterText;

    void Update()
    {
        ammoCounterText.text = (Weapon.GetComponent<GTWeaponScript>().curAmmo + "/" + Weapon.GetComponent<GTWeaponScript>().maxAmmo);
    }
}
