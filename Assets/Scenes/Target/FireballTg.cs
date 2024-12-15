using UnityEngine;

public class FireballTg : MonoBehaviour
{
    public float speed = 10.0f;
    public float damage = 10f;

    void Update()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        var target = other.gameObject.GetComponent<IHeath>();
        if (target != null)
        {
            target.Damage(damage);
        }

        Destroy(this.gameObject);
    }
}
