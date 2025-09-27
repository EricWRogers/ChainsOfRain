using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ChargeUI : MonoBehaviour
{
    public GameObject Weapon;
    public Slider chargeSlider;
    public Image fillImage;
    public float flashSpeed;
    public float overHeatPercent;
    public Color baseColor;
    public Color warningColor;

    void Start()
    {
        chargeSlider.maxValue = Weapon.GetComponent<GTWeaponScript>().maxCharge;
        baseColor = fillImage.color;
    }

    void Update()
    {
        chargeSlider.value = Weapon.GetComponent<GTWeaponScript>().curCharge;
        if (chargeSlider.value >= overHeatPercent / (chargeSlider.maxValue * 100) * 100)
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
