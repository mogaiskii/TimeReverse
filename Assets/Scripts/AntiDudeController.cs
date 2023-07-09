using System.Collections.Generic;
using UnityEngine;

public class AntiDudeController : MonoBehaviour
{
    private List<Vector3> _positionsToGo;
    private bool _readyToGo;

    public void SetPositions(List<Vector3> positions)
    {
        _positionsToGo = positions;
        _readyToGo = true;
    }

    void FixedUpdate()
    {
        if (_readyToGo && _positionsToGo.Count > 0 && !ProgramManager.instance.freeze)
        {
            transform.position = _positionsToGo[0];
            _positionsToGo.RemoveAt(0);
        }
    }
}
