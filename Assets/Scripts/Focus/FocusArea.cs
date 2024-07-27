using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Gain focus when an object is double clicked. Lost it when double clicked again anywhere. 
/// The given scripts and objects are enabled\disabled when gaining\losing focus.
/// and disable them when focus is lost.
/// </summary>
public class FocusArea : Focus
{

    [SerializeField] private Focus mainFocus;
    [SerializeField] private Transform clickObject;
    [Header("Enable\\Disable when gaining\\losing focus")]
    [SerializeField] private List<MonoBehaviour> scripts;
    [SerializeField] private List<GameObject> objects;


    [SerializeField] private MouseTypeEvent mouseEvent;

    private NetsPlayerInput input;

    private void Awake()
    {
        input = new NetsPlayerInput();
        input.Player.Enable();
    }

    private void Start()
    {
        LoseFocus();
    }

    #region ------------------- enable \ disable -------------------

    private void OnEnable()
    {
        input.Player.Focus.performed += DoubleClick;
    }

    private void OnDisable()
    {
        input.Player.Focus.performed -= DoubleClick;
    }

    #endregion

    private bool isFocused = false;

    private void DoubleClick(InputAction.CallbackContext obj)
    {
        if (isFocused)
        {
            mainFocus.TakeFocusFrom(this);
        }
        else
        {
            Transform transform = mouseEvent.ObjectAtMousePosition();
            if (transform == clickObject)
            {
                TakeFocusFrom(mainFocus);
            }
        }
    }


    public override void LoseFocus()
    {
        foreach (var script in scripts)
        {
            script.enabled = false;
        }
        foreach (var obj in objects)
        {
            obj.SetActive(false);
        }

        isFocused = false;
        clickObject.gameObject.SetActive(true);
    }

    public override void GetFocus()
    {
        foreach (var script in scripts)
        {
            script.enabled = true;
        }
        foreach (var obj in objects)
        {
            obj.SetActive(true);
        }

        isFocused = true;
        clickObject.gameObject.SetActive(false);
    }
}
