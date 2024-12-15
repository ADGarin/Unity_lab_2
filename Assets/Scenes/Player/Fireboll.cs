using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour
{
    public float speed = 15.0f;
    public int damage = 1;
    void Update()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    void OnCollisionExit(Collision collision)
    {
        Target target = collision.gameObject.GetComponent<Target>();
        if (target != null)
        {
            target.ReactToHit();
        }

        Destroy(this.gameObject);
    }
}