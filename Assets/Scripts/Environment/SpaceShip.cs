using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    public Animator anim;

    public void AfterBossDeath()
    {
        anim.Play("DropContainers");
    }
}
