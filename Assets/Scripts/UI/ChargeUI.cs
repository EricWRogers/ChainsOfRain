using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ChargeUI : GunUI
{
    
    public Slider chargeSlider;
    public Image fillImage;
    public float flashSpeed;
    public Color baseColor;
    public Color warningColor;

    new void Start()
    {
        base.Start();
        chargeSlider.maxValue = weapon.maxAmmo;
        baseColor = fillImage.color;
    }

    void Update()
    {
        chargeSlider.value = weapon.ammo;
        if (chargeSlider.value >= weapon.GetComponent<LaserGun>().overChargeWarning)//   (chargeSlider.maxValue * 100) * 100)
        {
            
            float pongTime = Mathf.PingPong(Time.time * flashSpeed, 1f);
            fillImage.color = Color.Lerp(baseColor, warningColor, pongTime);
        }
        else
        {
            fillImage.color = baseColor;
        }
        
    }

}
