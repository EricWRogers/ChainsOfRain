using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : GunUI
{
    public Slider chargeSlider;
    public TextMeshProUGUI ammoCounterText;
    new void Start()
    {
        base.Start();
        chargeSlider.maxValue = weapon.maxAmmo;
    }
    void Update()
    {
        chargeSlider.value = weapon.ammo;
        ammoCounterText.text = weapon.ammo.ToString();
        if (weapon.leftHanded)
        {
            ammoCounterText.rectTransform.localScale = new Vector3(-1, 1, 1);
        }

    }

}
