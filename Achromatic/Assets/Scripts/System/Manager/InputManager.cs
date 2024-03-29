using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : SingletonBehavior<InputManager>
{
    const KeyCode JUMP = KeyCode.W;
    const KeyCode LEFT = KeyCode.A;
    const KeyCode RIGHT = KeyCode.D;
    const KeyCode SIT = KeyCode.S;
    const KeyCode DASH = KeyCode.Space;
    const KeyCode LIGHT_ATTACK = KeyCode.Mouse0;
    const KeyCode HEAVY_ATTACK = KeyCode.Mouse1;

    public UnityEvent JumpEvent;
    public UnityEvent<float> MoveEvent;
    public UnityEvent SitEvent;
    public UnityEvent<Vector2> DashEvent;
    public UnityEvent<Vector2> LightAttackEvent;
    public UnityEvent<Vector2> HeavyAttackEvent;

    private Camera mainCamera;
    public Vector2 MouseVec { get; private set; }

    protected override void OnAwake()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        MouseVec = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKey(LEFT))
        {
            MoveEvent?.Invoke(-1);
        }
        else if (Input.GetKey(RIGHT))
        {
            MoveEvent?.Invoke(1);
        }
        else
        {
            MoveEvent?.Invoke(0);
        }

        if (Input.GetKey(LIGHT_ATTACK))
        {
            LightAttackEvent?.Invoke(MouseVec);
        }
        else if (Input.GetKey(HEAVY_ATTACK))
        {
            HeavyAttackEvent?.Invoke(MouseVec);
        }

        if (Input.GetKey(JUMP))
        {
            JumpEvent?.Invoke();
        }
        if (Input.GetKey(SIT))
        {
            SitEvent?.Invoke();
        }
        if(Input.GetKey(DASH))
        {
            DashEvent?.Invoke(MouseVec);
        }
        
    }
}
