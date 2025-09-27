using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ChargeUI : MonoBehaviour
{
    public GameObject Weapon;
    public Slider chargeSlider;

    void Start()
    {
        chargeSlider.maxValue = Weapon.GetComponent<GTWeaponScript>().maxCharge;
    }

    void Update()
    {
        chargeSlider.value = Weapon.GetComponent<GTWeaponScript>().curCharge;
    }

}
