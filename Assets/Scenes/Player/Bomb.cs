using UnityEngine;

public class Boomb : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    public float speed = 10.0f;

    public float radius = 20000f;
    public float force = 500000f;
    void Update()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Explode(collision.GetContact(0).point);
    }

    void Explode(Vector3 pos)
    {
        Collider[] overlappedColliders = Physics.OverlapSphere(pos, radius);

        foreach (Collider collider in overlappedColliders)
        {
            Rigidbody rigidbody = collider.attachedRigidbody;
            if (rigidbody)
            {
                rigidbody.AddExplosionForce(force, pos, radius, 3f);
            }

            var target = collider.GetComponent<Target>();
            if (target)
            {
                target.ReactToHit();
            }
        }

        var expl = Instantiate(explosionPrefab);
        if (expl != null)
        {
            expl.transform.position = pos;
            var ps = expl.GetComponent<ParticleSystem>();
            ps.Play();
            Destroy(expl, ps.main.duration);
        }

        Destroy(gameObject);
    }
}