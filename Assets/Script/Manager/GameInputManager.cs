using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInputManager : MonoBehaviour
{
    #region Singleton
    public static GameInputManager Instance;

    void Awake()
    {
        if(Instance == null) Instance = this;
    }
    #endregion

    #region ForEvent
    public event EventHandler OnShootAction;
    #endregion

    private PlayerInput playerInput;
    private Vector2 inputVector;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = new PlayerInput();
        playerInput.Player.Enable(); 

        playerInput.Player.Shoot.performed += Shoot_Performed; 
    }

    private void Shoot_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnShootAction?.Invoke(this, EventArgs.Empty);
    }   

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInput.Player.Movement.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }

    public Vector2 GetAttackVectorNormalized()
    {
        Vector2 inputVector = playerInput.Player.Aim.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }
}
