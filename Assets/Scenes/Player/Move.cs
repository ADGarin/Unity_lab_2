using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 6.0f;
    public float gravity = -9.8f;
    public float jumpSpeed = 0.5f;
    public float maxFallSpeed = -15.0f;
    private CharacterController _charController;

    private float _vertSpeed = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _charController = GetComponent<CharacterController>();

        Rigidbody body = GetComponent<Rigidbody>();
        if (body != null)
        {
            body.mass = 60f;
            body.useGravity = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var state = GetComponent<State>();
        if (state != null && state.GetHealth() <= 0)
        {
            return;
        }

        Vector3 curMovement = transform.position;

        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;

        Vector3 movement = new Vector3(deltaX, 0, deltaZ);
        movement = Vector3.ClampMagnitude(movement, speed);
        movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement);

        if (_charController.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _vertSpeed = jumpSpeed;
            }
            else
            {
                _vertSpeed = 0f;
            }
        }
        else
        {
            _vertSpeed += gravity * 5 * Time.deltaTime;
            if (_vertSpeed < maxFallSpeed)
            {
                _vertSpeed = maxFallSpeed;
            }
        }

        movement.y += _vertSpeed;

        _charController.Move(movement);
    }
}
