using UnityEngine;
using UnityEngine.SceneManagement;
using SuperPupSystems.Helper;
public class TutorialReset : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
   {
     if(other.gameObject.transform.root.CompareTag("Player") && other.gameObject.GetComponent<Health>())
      {
        SceneManager.LoadScene("Tutorial");
      }
     else if(other.gameObject.transform.root.CompareTag("Enemy"))
     {
        Destroy(other.gameObject);
     }
   }
}
