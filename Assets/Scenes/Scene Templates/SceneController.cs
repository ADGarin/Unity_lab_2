using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

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

    public int StageCount = 5;
    public int StartSegmentCount = 6;
    public float ForwardProbability = 0.5f;
    public float StartTargetProbability = 0.3f;

    private float _step = 20f; 

    private float _curAngle = 0f;
    private GameObject _curSegment;
    private GameObject _player;
    private int _curStage = 0;

    private string _endMessage = "";
    private Vector2 _nativeSize = new Vector2(640, 480);

    private class Stage
    {
        public Stage()
        {
            segments = new List<GameObject>();
            targets = new List<GameObject>();
        }

        public void Clear()
        {
            clearGameObjects(targets);
            clearGameObjects(segments);
        }

        public List<GameObject> segments;
        public List<GameObject> targets;

        private void clearGameObjects(List<GameObject> objects)
        {
            foreach (var obj in objects)
            {
                Destroy(obj);
            }

            objects.Clear();
        }

    }

    private List<Stage> _stages;

    void Start()
    {
        _stages = new List<Stage>();
        _stages.Add(createStage(StartSegmentCount, true, false, StartTargetProbability));

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

    void OnGUI()
    {
        if (_endMessage.Length == 0) return;

        var centeredStyle = GUI.skin.GetStyle("Label");
        float scale = Screen.width / _nativeSize.x;
        centeredStyle.fontSize = (int)(40.0f * scale);
        centeredStyle.alignment = TextAnchor.UpperCenter;
        GUI.Label(new Rect(Screen.width / 2 - (int)(250.0f * scale), 
                Screen.height / 2 - (int)(50.0f * scale), 
                (int)(500.0f * scale), 
                (int)(100.0f * scale)), 
                _endMessage, centeredStyle);
    }
    

    public void EndLevel()
    {
        _endMessage = "End Game";
        Time.timeScale = 0;
        StartCoroutine(quit());
    }

    public void FailLevel()
    {
        _endMessage = "Game Over";
        StartCoroutine(quit());
    }

    public void EndStage()
    {
        _curStage++;
        var segmentCount = StartSegmentCount + StartSegmentCount * _curStage / 2;
        float targetProbability = StartTargetProbability + 0.1f * _curStage;

        _stages.Add(createStage(segmentCount, false, _curStage >= StageCount - 1, targetProbability));
    }

    public void StartStage()
    {
        var completedStage = _stages[_curStage - 1];
        completedStage.Clear();
    }

    private Stage createStage(int segmentCount, bool isFirst, bool isLast, float targetProbability)
    {
        Stage stage = new Stage();

        if (isFirst)
        {
            createStart();
        }

        stage.segments.Add(_curSegment);

        for (int i = 0; i < segmentCount; i++)
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

            stage.segments.Add(_curSegment);
        }

        if (!isLast)
        {
            createStageEnd();
        }
        else
        {
            createLevelEnd();
        }

        for (int i = 2; i < stage.segments.Count; i++)
        {
            var segment = stage.segments[i];
            var target = addTarget(segment, targetProbability);
            if (target != null)
            {
                stage.targets.Add(target);
            }
        }

        return stage;
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

    private GameObject addTarget(GameObject segment, float targetProbability)
    {
        var rnd = Random.Range(0f, 1f);
        if (rnd <= targetProbability)
        {
            var target = Instantiate(targetPrefab) as GameObject;
            target.transform.Translate(segment.transform.localPosition);
            target.transform.Translate(0, 2, 0);

            var movement = -target.transform.forward;
            target.transform.LookAt(movement);

            return target;
        }

        return null;
    }

    private IEnumerator quit()
    {
        yield return new WaitForSeconds(5f);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("Welcome");
    }
}
