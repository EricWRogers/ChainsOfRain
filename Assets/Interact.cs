using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Interact : MonoBehaviour
{
    
    public bool lookingAtInteractable = false;
    public TextMeshProUGUI interactText;
    void Update()
    {
        if (Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out RaycastHit hit, 2f))
        {

            if (hit.collider.CompareTag("Interactable"))
            {
                lookingAtInteractable = true;
                interactText.color = Color.white;
            }
            else
            {
                lookingAtInteractable = false;
            }
        }

        else
        {
            lookingAtInteractable = false;
            interactText.color = Color.clear;
        }

        if (lookingAtInteractable && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Interacted with: " + hit.collider.name);
            hit.collider.GetComponent<Interactable>().Interacted.Invoke();
        }
    }
    
    
}
