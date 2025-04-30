using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeController : MonoBehaviour
{
    public FixedTouchField _FixedTouchField;
    public CameraLook _CameraLook;
    void Update()
    {
        _CameraLook.LockAxis = _FixedTouchField.TouchDist;
    }
}
