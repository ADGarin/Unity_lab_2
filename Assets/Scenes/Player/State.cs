using UnityEngine;

public class State : MonoBehaviour, IHealth
{
    private float _health = 100f;
    private float _maxHealth = 100f;

    private Vector2 _nativeSize = new Vector2(640, 480);

    void OnGUI()
    {
        var style = GUI.skin.GetStyle("Box");
        float scale = Screen.width / _nativeSize.x;
        style.fontSize = (int)(16.0f * scale);
        style.alignment = TextAnchor.UpperCenter;
        GUI.Box(new Rect(Screen.width - (int)(110.0f * scale), 0, 
            (int)(110.0f * scale), (int)(25.0f * scale)),
                $"Health {_health}", style);
    }

    public void Damage(float damage)
    {
        if (_health <= 0) return;
  
        _health -= damage;
        if (_health > _maxHealth)
        {
            _health = _maxHealth;
        }
        else if (_health <= 0)
        {
            transform.Rotate(-90, 0, 0);

            var gameController = GameObject.Find("Controller");
            gameController.SendMessage("FailLevel");
        }
    }

    public float GetHealth() { return _health; }
}
