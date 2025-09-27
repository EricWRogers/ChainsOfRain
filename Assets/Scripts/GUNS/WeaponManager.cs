using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;


    public GameObject rightArm;
    public GameObject leftArm;

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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject OpenArmCheck()
    {
        if (rightArm.transform.childCount == 0)
        {
            return rightArm;
        }
        else if (leftArm.transform.childCount == 0)
        {
            return leftArm;
        }
        else
        {
            return null;
        }
    }

    public void AttatchGun(GameObject _gun)
    {
        GameObject arm = OpenArmCheck();

       GameObject gun = Instantiate(_gun, arm.transform.parent.parent.GetChild(0).position, arm.transform.parent.parent.GetChild(0).rotation, arm.transform);

        gun.GetComponent<Gunbase>().leftHanded = arm.name == "LeftArm";
        gun.GetComponent<Gunbase>().rightHanded = arm.name == "RightArm";

    }
}
