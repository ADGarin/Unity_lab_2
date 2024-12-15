using UnityEngine;
using System.Collections.Generic;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject startPrefab;
    [SerializeField] private GameObject endStagePrefab;
    [SerializeField] private GameObject endLevelPrefab;
    [SerializeField] private GameObject forwardPrefab;
    [SerializeField] private GameObject turnRightPrefab;
    [SerializeField] private GameObject turnLeftPrefab;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject targetPrefab;

    public int StageCount = 3;
    public int SegmentCount = 20;
    public float ForwardProbability = 0.5f;
    public float TargetProbability = 0.2f;

    private float _step = 20f; 

    private float _curAngle = 0f;
    private GameObject _curSegment;
    private GameObject _player;

    private List<GameObject> _segments;

    void Start()
    {
        _segments = new List<GameObject>();
        createStart();
        _segments.Add(_curSegment);

        for (int s = 0; s < StageCount; s++)
        {
            for (int i = 0; i < SegmentCount; i++)
            {
                var rnd = Random.Range(0f, 1f);
                if (rnd <= ForwardProbability)
                {
                    goForward();
                }
                else
                {
                    var isLeft = _curAngle > 0f ||
                        _curAngle == 0f && rnd <= 0.5 + ForwardProbability / 2;

                    if (isLeft)
                    {
                        turnLeft();
                    }
                    else
                    {
                        turnRight();
                    }
                }

                _segments.Add(_curSegment);
            }

            if (s < StageCount - 1)
            {
                createStageEnd();
            }
            else
            {
                createLevelEnd();
            }
        }

        foreach (var segment in  _segments)
        {
            addTarget(segment);
        }

        //goForward();
        //goForward();
        //turnLeft();
        //goForward();
        //goForward();
        //turnRight();
        //goForward();
        //goForward();
        //turnRight();
        //goForward();
        //goForward();
        //turnLeft();
        //goForward();
        //goForward();
    }

    void Update()
    {
        
    }

    public void EndLevel()
    {
        Debug.Log("End Level");
    }

    public void FailLevel()
    {
        Debug.Log("Fail Level");
    }

    private void createStart()
    {
        _curSegment = Instantiate(startPrefab) as GameObject;
        _player = Instantiate(playerPrefab) as GameObject;
    }
    private void createStageEnd()
    {
        var end = Instantiate(endStagePrefab) as GameObject;
        setPos(end);

        _curSegment = end;
    }

    private void createLevelEnd()
    {
        var end = Instantiate(endLevelPrefab) as GameObject;
        setPos(end);

        _curSegment = end;
    }
    private void turnLeft()
    {
        var turn = Instantiate(turnLeftPrefab) as GameObject;
        setPos(turn);

        _curAngle -= 90;
        _curSegment = turn;
    }

    private void turnRight()
    {
        var turn = Instantiate(turnRightPrefab) as GameObject;
        setPos(turn);

        _curAngle += 90;
        _curSegment = turn;
    }

    private void goForward()
    {
        var forward = Instantiate(forwardPrefab) as GameObject;
        setPos(forward);

        _curSegment = forward;
    }

    private void setPos(GameObject obj)
    {
        obj.transform.Translate(_curSegment.transform.localPosition);
        obj.transform.Rotate(0, _curAngle, 0);

        var movement = obj.transform.forward;
        movement *= _step;

        movement = obj.transform.TransformDirection(movement);
        if (_curAngle != 0f) movement = -movement;
        obj.transform.Translate(movement);
    }

    private void addTarget(GameObject sgment)
    {
        var rnd = Random.Range(0f, 1f);
        if (rnd <= TargetProbability)
        {
            var target = Instantiate(targetPrefab) as GameObject;
            target.transform.Translate(sgment.transform.localPosition);
            target.transform.Translate(0, 2, 0);


            var movement = -target.transform.forward;
            target.transform.LookAt(movement);
        }
    }
}
