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

    void Update()
    {
        var state = GetComponent<State>();
        if (state != null && state.GetHealth() <= 0)
        {
            return;
        }

        var realSpeed = speed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            // RUN
            realSpeed *= 2;
        }

        float deltaX = Input.GetAxis("Horizontal") * realSpeed;
        float deltaZ = Input.GetAxis("Vertical") * realSpeed;

        Vector3 movement = new Vector3(deltaX, 0, deltaZ);
        movement = Vector3.ClampMagnitude(movement, realSpeed);
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
