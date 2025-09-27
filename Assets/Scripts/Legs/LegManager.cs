using UnityEngine;

public class LegManager : MonoBehaviour
{
    public static LegManager instance;

    public GameObject rightLeg;
    public GameObject leftLeg;


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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (leftLeg.transform.childCount != 0)
            {


                GameObject leg = leftLeg.transform.GetChild(0).gameObject;
                leg.GetComponent<Legbase>().Jettison();
                Destroy(leg);
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (rightLeg.transform.childCount != 0)
            {

                GameObject leg = rightLeg.transform.GetChild(0).gameObject;
                leg.GetComponent<Legbase>().Jettison();
                Destroy(leg);
            }
        }
    }

    public GameObject OpenLegCheck()
    {
        if (rightLeg.transform.childCount == 0)
        {
            return rightLeg;
        }
        else if (leftLeg.transform.childCount == 0)
        {
            return leftLeg;
        }
        else
        {
            return null;
        }

    }

    public bool AttatchLeg(GameObject _leg)
    {
        GameObject leg = OpenLegCheck();

        if (leg == null)
        {
            return false;
        }

        GameObject new_leg = Instantiate(_leg, leg.transform.parent.parent.GetChild(0).position, leg.transform.parent.parent.GetChild(0).rotation, leg.transform);

        new_leg.GetComponent<Legbase>().leftLegged = leg.name == "LeftLeg";
        new_leg.GetComponent<Legbase>().rightLegged = leg.name == "RightLeg";

        return true;

    }

}
