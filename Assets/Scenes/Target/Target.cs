using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour
{
    public float speed = 3.0f;
    public float obstacleRange = 5.0f;
    public float rotSpeed = 15.0f;

    public float minTurnAngle = 10.0f;
    public float maxTurnAngle = 110.0f;

    public float damage = 50.0f;

    private Rigidbody _rb;
    private GameObject _player;

    private enum State
    {
        Alive,
        Die
    }
    private State _state = State.Alive;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        if (_state == State.Die) return;

        transform.Translate(0, 0, speed * Time.deltaTime);
        //_rb.MovePosition(transform.position +
        //    transform.forward * speed * Time.fixedDeltaTime);

        bool needTurn = false;
        RaycastHit hit;
        if (observeDirection(transform.forward, 0.75f, out hit))
        {
            needTurn = !isOwnFireball(hit) && (isFireball(hit) || hit.distance < obstacleRange);
        }

        //if (!needTurn && observeDirection(-transform.forward, 0.75f, out hit))
        //{
        //    needTurn = isFireball(hit);
        //}

        if (!needTurn)
        {
            float val = Random.Range(0, 100);
            needTurn = val > 99f;
        }

        if (needTurn)
        {
            float turnRangeMiddle = maxTurnAngle - minTurnAngle;
            float angle = Random.Range(0, turnRangeMiddle * 2);
            if (angle <= turnRangeMiddle)
            {
                angle = -angle;
                angle -= minTurnAngle;

            }
            else
            {
                angle -= turnRangeMiddle;
                angle += minTurnAngle;
            }

            //Vector3 rotation = Vector3.up * angle;
            //Quaternion angleRot = Quaternion.Euler(rotation * Time.fixedDeltaTime);
            //_rb.MoveRotation(_rb.rotation * angleRot);

            //Quaternion curRotation = _rb.rotation;
            //Quaternion newRotation = Quaternion.Euler(
            //    curRotation.eulerAngles.x,
            //    curRotation.eulerAngles.y + angle,
            //    curRotation.eulerAngles.z
            //);

            //_rb.MoveRotation(newRotation);

            transform.Rotate(0, angle, 0);
        }
        else
        {
            if (observeDirection(transform.forward, 1.75f, out hit) && isPlayer(hit))
            {
                _player = hit.transform.gameObject;
            }

            if (_player != null)
            {
                rotateAndShoot(_player);
                return;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        var target = collision.gameObject.GetComponent<IHealth>();
        if (target != null)
        {
            target.Damage(damage);
        }

        var fireboll = collision.gameObject.GetComponent<Fireball>();
        if (fireboll != null && _player == null)
        {
            Collider[] overlappedColliders = Physics.OverlapSphere(transform.position, 20000f);
            foreach (Collider collider in overlappedColliders)
            {
                var gameObject = collider.gameObject;
                var player = gameObject.GetComponent<IHealth>();
                if (player != null)
                {
                    _player = gameObject;
                    break;
                }
            }
        }
    }

    public void ReactToHit()
    {
        StartCoroutine(Damage());
    }
    private IEnumerator Damage()
    {
        yield return new WaitForSeconds(3f);

        Vector3 local = new Vector3(0, 10, 0);
        Vector3 glob = transform.TransformDirection(local);

        if (Vector3.Angle(glob, local) < 45)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.mass /= 2;
            }
        }
        else
        {
            _state = State.Die;
        }
    }

    private void rotateAndShoot(GameObject player)
    {
        var hitPos = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        var selfPos = new Vector3(transform.position.x, 0, transform.position.z);

        var qTo = Quaternion.LookRotation(hitPos - selfPos);
        qTo = Quaternion.Slerp(transform.rotation, qTo, 10 * Time.deltaTime);
        _rb.MoveRotation(qTo);

        var shoot = gameObject.GetComponent<ShootTg>();
        if (shoot != null)
        {
            shoot.Shoot();
        }
    }

    private bool isFireball(RaycastHit hit)
    {
        GameObject obj = hit.transform.gameObject;
        return obj != null && obj.GetComponent<Fireball>() != null;
    }
    private bool isOwnFireball(RaycastHit hit)
    {
        GameObject obj = hit.transform.gameObject;
        return obj != null && obj.GetComponent<FireballTg>() != null;
    }

    private bool isPlayer(RaycastHit hit)
    {
        GameObject obj = hit.transform.gameObject;
        return obj != null && obj.GetComponent<IHealth>() != null;
    }

    private bool observeDirection(Vector3 direction, float radius, out RaycastHit hit)
    {
        Ray ray = new Ray(transform.position, direction);
        return Physics.SphereCast(ray, radius, out hit);
    }
}
