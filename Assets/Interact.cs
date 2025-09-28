using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Interact : MonoBehaviour
{

    public bool lookingAtInteractable = false;
    public LayerMask layerMask;
    private Transform m_startPos;

    void Start()
    {
        m_startPos = Camera.main.transform;
    }
    void Update()
    {
        if (Physics.Raycast(m_startPos.position, m_startPos.forward, out RaycastHit hit, 5f, layerMask))
        {
            Debug.Log("hit");
            if (hit.transform.CompareTag("Interactable"))
            {
                lookingAtInteractable = true;
            }
            else
            {
                lookingAtInteractable = false;
            }
            if (lookingAtInteractable && Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("Interacted with: " + hit.collider.name);
                hit.transform.GetComponent<Interactable>().Interacted.Invoke();
            }
        }


    }
    private void OnDrawGizmos()
    {
        if (lookingAtInteractable) Gizmos.color = Color.blue;
        else Gizmos.color = Color.red;
        Gizmos.DrawRay(m_startPos.position, m_startPos.forward * 5f);
    }
    
    
}
