using UnityEngine;

public class CloseDoorTrigger : MonoBehaviour
{
    [SerializeField] GameObject door;
    void OnTriggerExit(Collider other)
    {
        door.SendMessage("Close");

        var health = other.gameObject.GetComponent<IHeath>();
        if (health != null)
        {
            health.Damage(-10);
        }

        Destroy(this.gameObject);
    }
}
