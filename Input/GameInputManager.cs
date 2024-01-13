using System.Collections;
using System.Collections.Generic;
using GGG.Tool.Singleton;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputManager : Singleton<GameInputManager>
{
    private GameInputAction _gameInputAction;
    public Vector2 Movement => _gameInputAction.GameInput.Movement.ReadValue<Vector2>();
    public Vector2 CameraLook => _gameInputAction.GameInput.CameraLook.ReadValue<Vector2>();
    public bool Run => _gameInputAction.GameInput.Run.triggered;
    public bool LAttack => _gameInputAction.GameInput.LAttack.triggered;
    public bool RAttack => _gameInputAction.GameInput.RAttack.triggered;
    public bool Climb => _gameInputAction.GameInput.Climb.triggered;
    public bool Grab => _gameInputAction.GameInput.Grab.triggered;
    public bool TakeOut => _gameInputAction.GameInput.TakeOut.triggered;
    public bool UseKick => _gameInputAction.GameInput.UseKick.triggered;
    public bool Parry => _gameInputAction.GameInput.Parry.phase==InputActionPhase.Performed;
    public bool EquipWeapon=> _gameInputAction.GameInput.EquipWeapon.triggered;
    public bool Dodge => _gameInputAction.GameInput.Dodge.triggered;
    public bool Switch => _gameInputAction.GameInput.Switch.triggered;
    public float Zoom => _gameInputAction.GameInput.Zoom.ReadValue<float>();

    protected override void Awake()
    {
        base.Awake();
        _gameInputAction ??=new GameInputAction();
    }
    private void OnEnable()
    {
        _gameInputAction.Enable();
    }
    private void OnDisable()
    {
        _gameInputAction.Disable();
    }
}
