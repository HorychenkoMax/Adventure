using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private PlayerInputActions playerInputAction;

    public event EventHandler OnPlayerAttack;

    private void Awake()
    {
        Instance = this;

        playerInputAction = new PlayerInputActions();
        playerInputAction.Enable();

        playerInputAction.Combat.Attack.started += PlayerAttack_started;
    }

    private void PlayerAttack_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if(OnPlayerAttack != null)
        {
            OnPlayerAttack.Invoke(this, EventArgs.Empty);
        }
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
