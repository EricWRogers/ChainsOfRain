using UnityEngine;
using UnityEngine.SceneManagement;
public class TutorialReset : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
   {
     if(other.gameObject.transform.root.CompareTag("Player"))
     {
        SceneManager.LoadScene("Tutorial");
     }
   }
}
