using UnityEngine;
public class TouchController : MonoBehaviour
{
    public FixedTouchField _FixedTouchField;
    public CameraLook _CameraLook;
    public PlayerMove _PlayerMove;
    public FixedButton _FixedButton;
    void Update()
    {
        _CameraLook.LockAxis = _FixedTouchField.TouchDist;
        _PlayerMove.Pressed = _FixedButton.Pressed;
    }
}