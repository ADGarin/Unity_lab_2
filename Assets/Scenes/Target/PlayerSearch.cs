using UnityEngine;

public class PlayerSearch : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool findDirection2Player(Vector3 direction, out RaycastHit hit)
    {
        Ray ray = new Ray(transform.position, direction);
        return Physics.SphereCast(ray, 0.75f, out hit);
    }
}
