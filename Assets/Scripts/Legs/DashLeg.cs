using KinematicCharacterControler;
using UnityEngine;

public class DashLeg : Legbase
{
    private void Start()
    {
        PlayerMovement playerMovement = PlayerMovement.instance;

        playerMovement.dashForce += 10;
        playerMovement.dashDuration += .2f;
    }

    private void OnDestroy()
    {
        PlayerMovement playerMovement = PlayerMovement.instance;

        playerMovement.dashForce -= 10;
        playerMovement.dashDuration -= .2f;
    }
}
