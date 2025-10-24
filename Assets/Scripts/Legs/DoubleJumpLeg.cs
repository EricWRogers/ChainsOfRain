using KinematicCharacterControler;
using UnityEngine;

public class DoubleJumpLeg : Legbase
{
    public void ToggleOn()
    {
        PlayerMovement playerMovement = PlayerMovement.instance;
        
        playerMovement.maxJumpCount += 1;
    }

    public void ToggleOff()
    {
        PlayerMovement playerMovement = PlayerMovement.instance;

        playerMovement.maxJumpCount -= 1;
    }
}
