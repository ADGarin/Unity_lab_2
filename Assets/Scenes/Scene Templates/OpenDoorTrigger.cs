using UnityEngine;

public class OpenDoorTrigger : MonoBehaviour
{
    [SerializeField] GameObject door;
    void OnTriggerEnter(Collider other)
    {
        var health = other.gameObject.GetComponent<IHealth>();
        if (health != null)
        {
            var gameController = GameObject.Find("Controller");
            gameController.SendMessage("EndStage");

            door.SendMessage("Open");
        }
    }
}
