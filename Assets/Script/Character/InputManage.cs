using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputManage : MonoBehaviour
{
    // Start is called before the first frame update

    private static InputManage _instance;

    public static InputManage Instance
    {
        get { return _instance; }
    }

    public InputPlayer _inputPlayer;
    private void Awake()
    {
        if(_instance != null&& _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        _inputPlayer = new InputPlayer();
        Cursor.visible = true;

    }

    private void OnEnable()
    {
        _inputPlayer.Enable();
    }
    private void OnDisable()
    {
        _inputPlayer.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return _inputPlayer.Player.Move.ReadValue<Vector2>();
    }

    public Vector2 GetMouse()
    {
        return _inputPlayer.Player.Look.ReadValue<Vector2>();
    }

    public bool PlayerJumpedThisFrame()
    {
        return _inputPlayer.Player.Jump.triggered;
    }


    public bool SwitchCameraThis()
    {
        return _inputPlayer.Player.SwitchCamera.triggered;
    }

    public bool IsLeftClickTriggered()
    {
        return _inputPlayer.Player.Attack.triggered; // "Attack" là action đã định nghĩa trong Input Actions
    }

    


}
