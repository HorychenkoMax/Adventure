using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private PlayerInputActions playerInputAction;
    private void Awake()
    {
        Instance = this;

        playerInputAction = new PlayerInputActions();
        playerInputAction.Enable();
    }

    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = playerInputAction.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }

    public Vector3 GetMousePosition()
    {
        Vector3 mousePos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
        return mousePos;
    }
}
