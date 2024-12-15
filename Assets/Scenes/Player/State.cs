using UnityEngine;

public class State : MonoBehaviour, IHeath
{
    private float _health = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float damage)
    {
        if (_health <= 0) return;
            Debug.Log($"Damage{damage}");
        _health -= damage;
        if (_health <= 0)
        {
            //transform.localEulerAngles = new Vector3(0, 0, -90);
            transform.Rotate(-90, 0, 0);

            var gameController = GameObject.Find("Controller");
            gameController.SendMessage("FailLevel");
        }
    }

    public float GetHealth() { return _health; }
}
