using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoubleClickFocus : Focus
{

    [SerializeField] private Focus mainFocus;
    [SerializeField] private Transform clickObject;
    [SerializeField] private List<MonoBehaviour> scripts;
    [SerializeField] private List<GameObject> objects;

    private NetsPlayerInput input;

    private void Awake()
    {
        //input = new NetsPlayerInput();
        //input.Camera.Enable();
    }

    private void Start()
    {
        LoseFocus();
    }

    private void OnEnable()
    {
        input.Player.Focus.performed += DoubleClick;
    }

    private void OnDisable()
    {
        input.Player.Focus.performed -= DoubleClick;
    }

    private bool isFocused = false;

    private void DoubleClick(InputAction.CallbackContext obj)
    {
        if (isFocused)
        {
            isFocused = false;
            mainFocus.TakeFocusFrom(this);
        }
        else
        {
            if (RaycastSelector.Instance.ObjectAtMousePosition(out Transform transform)
                && transform == clickObject)
            {
                isFocused = true;
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

        clickObject.gameObject.SetActive(true);
    }
}
