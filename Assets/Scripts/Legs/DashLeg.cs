using KinematicCharacterControler;
using UnityEngine;

public class DashLeg : Legbase
{
    public void ToggleOn()
    {
        PlayerMovement playerMovement = PlayerMovement.instance;

        playerMovement.dashForce += 10;
        playerMovement.dashDuration += .2f;
    }

    public void ToggleOff()
    {
        PlayerMovement playerMovement = PlayerMovement.instance;

        playerMovement.dashForce -= 10;
        playerMovement.dashDuration -= .2f;
    }
}
