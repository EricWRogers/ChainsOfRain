using UnityEngine;


public class DoorShaderOpening : MonoBehaviour
{
    [SerializeField] private Material doorMat;
    [SerializeField] private float openingSpeed = 1f;
    public Vector2 openSize = new Vector2(1f, 2f);
    private Vector2 openingAmount;
    public bool isOpening = false;
    private float time = 0f;

    void Update()
    {
        if (isOpening)
        {
            openingAmount = Vector2.Lerp(Vector2.zero, openSize, time / openingSpeed);
            doorMat.SetVector("_DoorSize", openingAmount);
            time += Time.deltaTime;
        }
    }

    public void OpenDoor()
    {
        isOpening = true;
        doorMat.SetFloat("_DoorwayOpen", 1f);
    }
}
