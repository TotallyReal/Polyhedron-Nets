using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RaycastSelector : MonoBehaviour
{
    /**
     * Remember to move this class ahead in the script execution order (in project settings).
     */


    /*public static RaycastSelector Instance { get; private set; }


    // TODO: should not be public
    public static MouseTypeEvent playerMouseEvent;

    
    private MouseTypeEvent input;

    void Awake()
    {
        if (Instance != null)
            return;
        Instance = this;

        input = new MouseTypeEvent();
        input.Player.Enable();

        playerMouseEvent = new MouseEvent(
            "Player", input.Player.PointerSelect, input.Player.PointerPosition, MouseTypeEvent.Dimension.D3);
    }

    private void OnEnable()
    {
        playerMouseEvent.OnEnable();
    }

    private void OnDisable()
    {
        playerMouseEvent.OnEnable();
    }*/
}
