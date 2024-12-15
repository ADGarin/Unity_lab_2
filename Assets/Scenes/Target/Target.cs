using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;

public class Target : MonoBehaviour
{
    public float speed = 3.0f;
    public float obstacleRange = 5.0f;
    public float rotSpeed = 15.0f;

    public float minTurnAngle = 10.0f;
    public float maxTurnAngle = 110.0f;

    //private bool _rotationCoroutine = false;

    private enum State
    {
        Alive,
        Die
    }
    private State _state = State.Alive;

 
    void Update()
    {
        if (_state == State.Die) return;

        transform.Translate(0, 0, speed * Time.deltaTime);

        //Debug.Log(GetComponent<Rigidbody>().angularVelocity);
        //if (Mathf.Abs(GetComponent<Rigidbody>().angularVelocity.y) > 0.01f) return;

        RaycastHit hit;
        if (observeDirection(transform.forward, 1.75f, out hit) && isPlayer(hit))
        {
            //var toPosition = (hit.transform.position - transform.position).normalized;
            //var fromPosition = transform.forward;
            //toPosition.y = fromPosition.y = 0;
            //float angleToPosition = Vector3.Angle(fromPosition, toPosition);

            //var targetRotation = Quaternion.LookRotation(hit.transform.forward - transform.forward);
            //transform.Rotate(targetRotation.eulerAngles);//Quaternion.RotateTowards(transform.rotation, targetRotation, speed * Time.smoothDeltaTime);

            //transform.Rotate(0, -angleToPosition, 0);
            //Debug.Log($"Find player {toPosition}:{fromPosition}-{angleToPosition}");
            //var targetRotation = Quaternion.LookRotation(hit.transform.position - transform.position);
            //transform.rotation = targetRotation;//Quaternion.RotateTowards(transform.rotation,forward targetRotation, speed * Time.smoothDeltaTime);
            //transform.LookAt(hit.point);

            var hitPos = new Vector3(hit.transform.position.x, 0, hit.transform.position.z);
            var selfPos = new Vector3(transform.position.x, 0, transform.position.z);

            var qTo = Quaternion.LookRotation(hitPos - selfPos);
            qTo = Quaternion.Slerp(transform.rotation, qTo, 10 * Time.deltaTime);
            GetComponent<Rigidbody>().MoveRotation(qTo);

            var shoot = gameObject.GetComponent<ShootTg>();
            if (shoot != null)
            {
                shoot.Shoot();
            }
     
            return;
        }

        bool needTurn = false;
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
            //Debug.Log("Motion Tuen");
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

            //Quaternion curRotation = transform.rotation;
            //Quaternion newRotation = Quaternion.Euler(
            //    curRotation.eulerAngles.x,
            //    curRotation.eulerAngles.y + angle,
            //    curRotation.eulerAngles.z
            //);

            //newRotation = Quaternion.Slerp(transform.rotation, newRotation, 10 * Time.deltaTime);
            //GetComponent<Rigidbody>().MoveRotation(newRotation);
            //transform.rotation = Quaternion.Lerp(curRotation, newRotation, rotSpeed * Time.deltaTime);

            //_rotationCoroutine = true;
            //StartCoroutine(Rotate(newRotation));

            transform.Rotate(0, angle, 0);
        }
    }

    private IEnumerator Rotate(Quaternion rotation)
    {
        while (transform.rotation != rotation)
        {
            var newRotation = Quaternion.RotateTowards(transform.rotation, rotation, rotSpeed * Time.deltaTime);

            transform.rotation = newRotation;

            yield return null;
        }

        //_rotationCoroutine = false;
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
            //Destroy(this.gameObject);
            _state = State.Die;
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
        return obj != null && obj.GetComponent<IHeath>() != null;
    }

    private bool observeDirection(Vector3 direction, float radius, out RaycastHit hit)
    {
        Ray ray = new Ray(transform.position, direction);
        return Physics.SphereCast(ray, radius, out hit);
    }
}
