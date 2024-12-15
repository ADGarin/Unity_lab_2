using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private GameObject bombPrefab;
    private Camera _camera;

    void Start()
    {
        _camera = GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnGUI()
    {
        int size = 12;
        float posX = _camera.pixelWidth / 2 - size / 4;
        float posY = _camera.pixelHeight / 2 - size / 2;
        GUI.Label(new Rect(posX, posY, size, size), "*");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                GameObject bomb = Instantiate(bombPrefab) as GameObject;
                bomb.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
                bomb.transform.rotation = transform.rotation;
                StartCoroutine(DestroyGameObject(bomb, 5f));
            }
            else
            {
                GameObject fireball = Instantiate(fireballPrefab) as GameObject;
                fireball.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
                fireball.transform.rotation = transform.rotation;
                StartCoroutine(DestroyGameObject(fireball, 4f));
            }

            //Vector3 point = new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
            //Ray ray = _camera.ScreenPointToRay(point);
            //RaycastHit hit;
            //if (Physics.Raycast(ray, out hit))
            //{
            //    GameObject hitObject = hit.transform.gameObject;
            //    Target target = hitObject.GetComponent<Target>();
            //    if (target != null)
            //    {
            //        target.ReactToHit();
            //    }
            //    else
            //    {
            //        StartCoroutine(SphereIndicator(hit.point));
            //    }
            //}
        }
    }

    private IEnumerator DestroyGameObject(GameObject fireball, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (fireball != null) Destroy(fireball);
    }

    private IEnumerator SphereIndicator(Vector3 pos)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = pos;
        yield return new WaitForSeconds(1);
        Destroy(sphere);
    }
}
