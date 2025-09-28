using UnityEngine;

public class LegManager : MonoBehaviour
{
    public static LegManager instance;

    public GameObject rightLeg;
    public LegType rightLegType = LegType.None;
    public GameObject leftLeg;
    public LegType leftLegType = LegType.None;  

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
            if (leftLegType != LegType.None)
            {
                switch (leftLegType)
                {
                    case LegType.DoubleJump:
                        leftLegType = LegType.None;
                        DoubleJumpLeg doubleJump = leftLeg.GetComponentInChildren<DoubleJumpLeg>();
                        doubleJump.Jettison();
                        doubleJump.transform.GetChild(0).gameObject.SetActive(false);
                        doubleJump.canUse = false;
                        break;

                    case LegType.Dash:
                        leftLegType = LegType.None;
                        DashLeg dash = leftLeg.GetComponentInChildren<DashLeg>();
                        dash.Jettison();
                        dash.transform.GetChild(0).gameObject.SetActive(false);
                        dash.canUse = false;
                        break;
                   
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            
            if (rightLegType != LegType.None)
            {
                
                switch (rightLegType)
                {
                    case LegType.DoubleJump:
                        
                        rightLegType = LegType.None;
                        DoubleJumpLeg doubleJump = rightLeg.GetComponentInChildren<DoubleJumpLeg>();
                        doubleJump.Jettison();
                        doubleJump.transform.GetChild(0).gameObject.SetActive(false);
                        doubleJump.canUse = false;
                        break;

                    case LegType.Dash:
                        rightLegType = LegType.None;
                        DashLeg dash = rightLeg.GetComponentInChildren<DashLeg>();
                        dash.Jettison();
                        dash.transform.GetChild(0).gameObject.SetActive(false);
                        dash.canUse = false;
                        break;

                }
            }
        }
    }

    public GameObject OpenLegCheck()
    {
        if (rightLegType == LegType.None)
        {
            return rightLeg;
        }
        else if (leftLegType == LegType.None)
        {
            return leftLeg;
        }
        else
        {
            Debug.Log("No open leg");
            return null;
        }
    }

    public bool AttatchLeg(LegType _leg)
    {
        GameObject leg = OpenLegCheck();

        if (leg == null)
        {
            return false;
        }

        Debug.Log("legType: " + leg.name);
        switch (_leg)
        {
            case LegType.None:

                break;

            case LegType.DoubleJump:
                DoubleJumpLeg doubleJump = leg.GetComponentInChildren<DoubleJumpLeg>();
                doubleJump.transform.GetChild(0).gameObject.SetActive(true);
                doubleJump.leftLegged = leg.name == "LeftBicep";
                doubleJump.rightLegged = leg.name == "RightBicep";

                doubleJump.canUse = true;
                break;

            case LegType.Dash:
                DashLeg dashLeg = leg.GetComponentInChildren<DashLeg>();
                dashLeg.transform.GetChild(0).gameObject.SetActive(true);
                dashLeg.leftLegged = leg.name == "LeftBicep";
                dashLeg.leftLegged = leg.name == "RightBicep";
                dashLeg.canUse = true;
                break;

            default:
                return false;
        }


        if (leg == rightLeg)
        {
            rightLegType = _leg;
        }
        else
        {
            leftLegType = _leg;
        }


        return true;


    }

}

public enum LegType
{
    None,
    DoubleJump,
    Dash,
}
