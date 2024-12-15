using UnityEngine;
using System.Collections;

public class ShootTg : MonoBehaviour
{
    [SerializeField] private GameObject fireballPrefab;

    private GameObject _fireball;

    public void Shoot()
    {
        if (_fireball != null) return;
        
        _fireball = Instantiate(fireballPrefab) as GameObject;
        _fireball.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
        _fireball.transform.rotation = transform.rotation;
        StartCoroutine(DestroyFireball(_fireball));
    }

    private IEnumerator DestroyFireball(GameObject fireball)
    {
        yield return new WaitForSeconds(10f);
        if (fireball != null) Destroy(fireball);
    }
}
