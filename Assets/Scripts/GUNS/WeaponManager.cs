using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;


    public GameObject rightArm;
    public GunType rightArmType = GunType.None;
    public GameObject leftArm;
    public GunType leftArmType = GunType.None;


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
                leftArmType = GunType.None;
                Gunbase gun = leftArm.GetComponentInChildren<Gunbase>();
                gun.Jettison();
                gun.gameObject.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (rightArmType != GunType.None)
            {
                rightArmType = GunType.None;
                Gunbase gun = rightArm.GetComponentInChildren<Gunbase>();
                gun.Jettison();
                gun.gameObject.SetActive(false);
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
        GameObject arm = OpenArmCheck();

        if (arm == null)
        {
            return false;
        }

        Debug.Log("Guntype: " +  arm.name);
        switch (_gun)
        {
            case GunType.None:

                break;

            case GunType.SemiAuto:

                arm.GetComponentInChildren<SemiAuto>().transform.GetChild(0).gameObject.SetActive(true);
                arm.GetComponentInChildren<SemiAuto>().leftHanded = arm.name == "LeftBicep";
                arm.GetComponentInChildren<SemiAuto>().rightHanded = arm.name == "RightBicep";

                arm.GetComponentInChildren<SemiAuto>().canShoot = true;
                break;

            case GunType.Auto:
                arm.GetComponentInChildren<AutoGun>().transform.GetChild(0).gameObject.SetActive(true);
                arm.GetComponentInChildren<AutoGun>().leftHanded = arm.name == "LeftBicep";
                arm.GetComponentInChildren<AutoGun>().rightHanded = arm.name == "RightBicep";
                arm.GetComponentInChildren<SemiAuto>().canShoot = true;
                break;

            case GunType.Laser:
                arm.GetComponentInChildren<LaserGun>().transform.GetChild(0).gameObject.SetActive(true);
                arm.GetComponentInChildren<LaserGun>().leftHanded = arm.name == "LeftBicep";
                arm.GetComponentInChildren<LaserGun>().rightHanded = arm.name == "RightBicep";
                arm.GetComponentInChildren<SemiAuto>().canShoot = true;
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
