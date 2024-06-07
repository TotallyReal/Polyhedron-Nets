using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Runtime.CompilerServices;
using UnityEngine.InputSystem;
using System;
using StarterAssets;
using Cinemachine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.XR;

public class CameraSwitch : Focus
{
    //[SerializeField] private List<GameObject> stateOnObjects;

    [SerializeField] private Focus playerFocus;
    [SerializeField] private Transform clickObject;
    [SerializeField] private CinemachineVirtualCamera toCamera;
    [SerializeField] private EdgePicker edgePicker;
    //[SerializeField] private Camera mainCamera;
    //[SerializeField] private FirstPersonController controller;
    // [SerializeField] Camera switchCamera;
    // [SerializeField] float tweenDurationSec = 10f;

    //private bool mainCameraLive = true;

    NetsPlayerInput input;

    private void Awake()
    {
        input = new NetsPlayerInput();
        input.Player.Enable();
        toCamera.gameObject.SetActive(false);
        //input = new NetsPlayerInput();
        //input.MouseSelection.Enable();
    }

    private void OnEnable()
    {
        input.Player.Focus.performed += DoubleClick;
        //input.MouseSelection.EdgeSelect.performed += DoubleClick;
    }

    private void OnDisable()
    {
        input.Player.Focus.performed -= DoubleClick;
        //input.MouseSelection.EdgeSelect.performed -= DoubleClick;
    }

    private bool isFocused = false;

    private void DoubleClick(InputAction.CallbackContext obj)
    {
        if (isFocused)
        {
            isFocused = false;
            playerFocus.TakeFocusFrom(this);
        } else
        {
            if (RaycastSelector.Instance.ObjectAtMousePosition(out Transform transform)
                && transform == clickObject)
            {
                isFocused = true;
                TakeFocusFrom(playerFocus);
            }
        }
        //SwitchCameras();
    }

    public override void LoseFocus()
    {
        edgePicker.enabled = false;
        toCamera.gameObject.SetActive(false);
        clickObject.gameObject.SetActive(true);
    }

    public override void GetFocus()
    {
        edgePicker.enabled = true;
        toCamera.gameObject.SetActive(true);
        clickObject.gameObject.SetActive(false);
        //clickObject.gameObject.SetActive(false);
        //StartCoroutine(DisableClickObject(0.1f));
    }

    private IEnumerator DisableClickObject(float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    /*
    public void SwitchBetween(Camera currentCamera, Camera nextCamera)
    {
        if (!currentCamera.gameObject.activeSelf)
            return;

        switchCamera.transform.position = currentCamera.transform.position;
        currentCamera.gameObject.SetActive(false);
        switchCamera.gameObject.SetActive(true);

        void SwitchToNext()
        {
            switchCamera.gameObject.SetActive(false);
            nextCamera.gameObject.SetActive(true);
        }

        switchCamera.transform.DOMove(nextCamera.transform.position, tweenDurationSec)
                              .OnComplete(SwitchToNext);
        switchCamera.transform.DORotateQuaternion(nextCamera.transform.rotation, tweenDurationSec);
    }*/
    /*
    public void SwitchCameras()
    {
        mainCameraLive = !mainCameraLive;

        Cursor.visible = !mainCameraLive;
        toCamera.gameObject.SetActive(!mainCameraLive);
        //mainCamera.gameObject.SetActive(mainCameraLive);
        controller.gameObject.SetActive(mainCameraLive);
        //controller.enabled = mainCameraLive;
        edgePicker.enabled = !mainCameraLive;

        Cursor.lockState = mainCameraLive ? CursorLockMode.Locked : CursorLockMode.None;
    }*/
}
