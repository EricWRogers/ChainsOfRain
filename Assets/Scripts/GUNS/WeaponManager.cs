using UnityEngine;
using UnityEngine.Events;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;


    public GameObject rightArm;
    public GunType rightArmType = GunType.None;
    public GameObject leftArm;
    public GunType leftArmType = GunType.None;
    public UnityEvent OnAttach;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (leftArmType != GunType.None)
            {
                switch (leftArmType)
                {
                    case GunType.SemiAuto:
                        leftArmType = GunType.None;
                        Gunbase semi = leftArm.GetComponentInChildren<SemiAuto>();
                        semi.Jettison();
                        semi.transform.GetChild(0).gameObject.SetActive(false);
                        semi.canShoot = false;
                        break;

                    case GunType.Auto:
                        leftArmType = GunType.None;
                        Gunbase auto = leftArm.GetComponentInChildren<AutoGun>();
                        auto.Jettison();
                        auto.transform.GetChild(0).gameObject.SetActive(false);
                        auto.canShoot = false;
                        break;

                    case GunType.Laser:
                        leftArmType = GunType.None;
                        Gunbase laser = leftArm.GetComponentInChildren<LaserGun>();
                        laser.Jettison();
                        laser.transform.GetChild(0).gameObject.SetActive(false);
                        laser.canShoot = false;
                        break;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (rightArmType != GunType.None)
            {
                switch (rightArmType)
                {
                    case GunType.SemiAuto:
                        rightArmType = GunType.None;
                        Gunbase semi = rightArm.GetComponentInChildren<SemiAuto>();
                        semi.Jettison();
                        semi.transform.GetChild(0).gameObject.SetActive(false);
                        semi.canShoot = false;
                        break;

                    case GunType.Auto:
                        rightArmType = GunType.None;
                        Gunbase auto = rightArm.GetComponentInChildren<AutoGun>();
                        auto.Jettison();
                        auto.transform.GetChild(0).gameObject.SetActive(false);
                        auto.canShoot = false;
                        break;

                    case GunType.Laser:
                        rightArmType = GunType.None;
                        Gunbase laser = rightArm.GetComponentInChildren<LaserGun>();
                        laser.Jettison();
                        laser.transform.GetChild(0).gameObject.SetActive(false);
                        laser.canShoot = false;
                        break;


                }
            }
        }
    }

    //public GameObject OpenArmCheck()
    //{
    //    if (rightArm.transform.childCount == 0)
    //    {
    //        return rightArm;
    //    }
    //    else if (leftArm.transform.childCount == 0)
    //    {
    //        return leftArm;
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //}

    public GameObject OpenArmCheck()
    {
        if (rightArmType == GunType.None)
        {
            return rightArm;
        }
        else if (leftArmType == GunType.None)
        {
            return leftArm;
        }
        else
        {
            Debug.Log("No open arm");
            return null;
        }
    }


    public bool AttatchGun(GunType _gun)
    {
        OnAttach.Invoke();
        GameObject arm = OpenArmCheck();

        if (arm == null)
        {
            return false;
        }

        Debug.Log("Guntype: " + arm.name);
        switch (_gun)
        {
            case GunType.None:

                break;

            case GunType.SemiAuto:

                SemiAuto semi = arm.GetComponentInChildren<SemiAuto>();
                semi.transform.GetChild(0).gameObject.SetActive(true);
                semi.leftHanded = arm.name == "LeftBicep";
                //if(semi.leftHanded) //GOtta flip it for the hand.
                //{
                //    Vector3 scale = semi.gameObject.transform.localScale;
                //    scale.z = scale.z * -1;
                //    semi.gameObject.transform.localScale = scale;
                //}
                semi.rightHanded = arm.name == "RightBicep";

                semi.canShoot = true;
                break;

            case GunType.Auto:
                AutoGun auto = arm.GetComponentInChildren<AutoGun>();
                auto.transform.GetChild(0).gameObject.SetActive(true);
                auto.leftHanded = arm.name == "LeftBicep";
                auto.rightHanded = arm.name == "RightBicep";
                auto.canShoot = true;
                break;

            case GunType.Laser:
                LaserGun laser = arm.GetComponentInChildren<LaserGun>();
                laser.transform.GetChild(0).gameObject.SetActive(true);
                laser.leftHanded = arm.name == "LeftBicep";
                laser.rightHanded = arm.name == "RightBicep";
                laser.canShoot = true;
                break;

            default:
                return false;
        }


        if (arm == rightArm)
        {
            rightArmType = _gun;
        }
        else
        {

            leftArmType = _gun;
        }


        return true;


    }

    //public bool AttatchGun(GameObject _gun)
    //{
    //    GameObject arm = OpenArmCheck();

    //    if(arm == null)
    //    {
    //        return false;
    //    }
    //   GameObject gun = Instantiate(_gun, arm.transform.parent.parent.GetChild(0).position, arm.transform.parent.parent.GetChild(0).rotation, arm.transform);

    //    gun.GetComponent<Gunbase>().leftHanded = arm.name == "LeftArm";
    //    gun.GetComponent<Gunbase>().rightHanded = arm.name == "RightArm";

    //    return true;

    //}

    
}

public enum GunType
{
    SemiAuto,
    Auto,
    Laser,
    None
}
