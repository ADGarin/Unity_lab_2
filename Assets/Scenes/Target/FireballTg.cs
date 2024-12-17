using UnityEngine;

public class FireballTg : MonoBehaviour
{
    public float speed = 15.0f;
    public float damage = 10f;

    void Update()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        var target = other.gameObject.GetComponent<IHealth>();
        if (target != null)
        {
            target.Damage(damage);
        }

        Destroy(this.gameObject);
    }
}
