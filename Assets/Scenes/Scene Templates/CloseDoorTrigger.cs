using UnityEngine;
using System.Collections;

public class CloseDoorTrigger : MonoBehaviour
{
    [SerializeField] GameObject door;
    void OnTriggerExit(Collider other)
    {
        var health = other.gameObject.GetComponent<IHealth>();
        if (health != null)
        {
            door.SendMessage("Close");
            health.Damage(-10);

            StartCoroutine(SendEndLevel(1f));
        }
    }

    private IEnumerator SendEndLevel(float delay)
    {
        yield return new WaitForSeconds(delay);
        var gameController = GameObject.Find("Controller");
        gameController.SendMessage("StartStage");

        Destroy(this.gameObject, 0.1f);
    }
}
