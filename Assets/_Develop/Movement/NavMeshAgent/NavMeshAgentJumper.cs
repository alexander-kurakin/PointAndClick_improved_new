using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentJumper 
{
    private float _speed;
    private NavMeshAgent _agent;
    private MonoBehaviour _couroutineRunner;
    private Coroutine _jumpProcess;
    private AnimationCurve _yOffsetCurve;

    public NavMeshAgentJumper(
        float speed,
        NavMeshAgent agent,
        MonoBehaviour couroutineRunner,
        AnimationCurve yOffsetCurve)
    {
        _speed = speed;
        _agent = agent;
        _couroutineRunner = couroutineRunner;
        _yOffsetCurve = yOffsetCurve;
    }

    public bool InProcess => _jumpProcess != null;

    public void Jump(OffMeshLinkData offMeshLinkData)
    {
        if (InProcess)
            return;

        _jumpProcess = _couroutineRunner.StartCoroutine(JumpProcess(offMeshLinkData));
    }

    private IEnumerator JumpProcess(OffMeshLinkData offMeshLinkData)
    { 
        Vector3 startPosition = offMeshLinkData.startPos;
        Vector3 endPosition = offMeshLinkData.endPos;

        float duration = Vector3.Distance(startPosition, endPosition) / _speed;

        float progress = 0;

        while (progress < duration)
        {
            float yOffset = _yOffsetCurve.Evaluate(progress / duration);

            _agent.transform.position = Vector3.Lerp(startPosition, endPosition, progress / duration) + Vector3.up * yOffset;
            progress += Time.deltaTime; 
            yield return null;
        }

        _agent.CompleteOffMeshLink();
        _jumpProcess = null;
    }
}
