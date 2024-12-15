using UnityEngine;

public class DoorController : MonoBehaviour
{
    private float delta = 4f;
    private float start;

    public float speed = 4f;
    private enum State
    {
        Opened,
        Closed,
    }

    private State state = State.Closed;

    void Start()
    {
        Vector3 pos = transform.position;
        start = pos.y;
    }

    void Update()
    {
        if (state == State.Opened)
        {
            Vector3 pos = transform.position;
            if (pos.y > start - delta) 
            {
                pos.y -= speed * Time.deltaTime;
                if (pos.y < start - delta)
                {
                    pos.y = start - delta;
                }

                transform.position = pos;
            }
        } 
        else
        {
            Vector3 pos = transform.position;
            if (pos.y < start)
            {
                pos.y += speed * Time.deltaTime;
                if (pos.y > start)
                {
                    pos.y = start;
                }

                transform.position = pos;
            }
        }
    }
    public void Open()
    {
        state = State.Opened;
    }

    public void Close()
    {
        state = State.Closed;
    }
}
