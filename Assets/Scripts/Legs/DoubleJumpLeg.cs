using KinematicCharacterControler;
using UnityEngine;

public class DoubleJumpLeg : Legbase
{
    private void Start()
    {
        PlayerMovement playerMovement = PlayerMovement.instance;
        
        playerMovement.maxJumpCount += 1;
    }

    private void OnDestroy()
    {
        PlayerMovement playerMovement = PlayerMovement.instance;

        playerMovement.maxJumpCount -= 1;
    }
}
