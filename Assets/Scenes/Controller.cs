using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    private GameObject _enemy;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemy == null)
        {
            _enemy = Instantiate(enemyPrefab) as GameObject;

            float x = Random.Range(0, 20);
            float z = Random.Range(0, 20);

            _enemy.transform.position = new Vector3(x, 0, z);
            float angle = Random.Range(0, 360);
            _enemy.transform.Rotate(0, angle, 0);
        }
    }
}
