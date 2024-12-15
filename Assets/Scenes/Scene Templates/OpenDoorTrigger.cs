using UnityEngine;

public class OpenDoorTrigger : MonoBehaviour
{
    [SerializeField] GameObject door;
    void OnTriggerEnter(Collider other)
    {
        door.SendMessage("Open");
    }
}
