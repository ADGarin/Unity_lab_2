using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private GameObject bombPrefab;
    private Camera _camera;
    private GUIStyle _style;

    private Vector2 _nativeSize = new Vector2(640, 480);

    private enum State
    {
        sPalay,
        sExit
    }

    private State _state = State.sPalay;

    void Start()
    {
        _camera = GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _style = new GUIStyle();
        _style.fontSize = 20;
        _style.normal.textColor = Color.white;
    }

    void OnGUI()
    {
        int size = 12;
        float posX = _camera.pixelWidth / 2 - size / 4;
        float posY = _camera.pixelHeight / 2 - size / 2;
        GUI.Label(new Rect(posX, posY, size, size), "*", _style);

        if (_state == State.sExit)
        {
            GUIStyle style = GUI.skin.GetStyle("Button");
            float scale = Screen.width / _nativeSize.x;
            style.fontSize = (int)(20.0f * scale);

            if (GUI.Button(new Rect(Screen.width / 2 - (int)(60.0f * scale),
                Screen.height / 2 - (int)(80.0f * scale),
                (int)(120.0f * scale), (int)(40.0f * scale)), "Exit", style))
            {
                Application.Quit();
            }

            if (GUI.Button(new Rect(Screen.width / 2 - (int)(60.0f * scale),
                Screen.height / 2 - (int)(20.0f * scale),
                (int)(120.0f * scale), (int)(40.0f * scale)), "Continue", style))
            {
                _state = State.sPalay;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                Time.timeScale = 1;
            }
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            _state = State.sExit;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Time.timeScale = 0;
        }

        if (_state == State.sExit)
        {
            return;
        }

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
