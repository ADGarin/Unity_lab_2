using UnityEngine;

public class EndLevelTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var gameController = GameObject.Find("Controller");
            gameController.SendMessage("EndLevel");
        }
    }
}
