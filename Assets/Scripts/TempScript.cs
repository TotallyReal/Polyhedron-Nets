using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempScript : MonoBehaviour
{

    [SerializeField] private InputAction action;
    [SerializeField] private InputActionReference reference;

    private void Awake()
    {
        Debug.Log("Temp awake");
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Temp start");
        NetsPlayerInput input = new NetsPlayerInput();
        input.Player.Enable();
        input.Player.Focus.performed += Focus_performed;
        input.Player.Action.performed += Action_performed;
        input.MouseSelection.Enable();
        input.MouseSelection.EdgeSelect.performed += EdgeSelect_performed;


        int[] hello = new int[] { 1, 2, 3, 4, 5 };
        (hello[1], hello[2]) = (hello[2], hello[1]);
        Debug.Log(string.Join(',', hello));

        //MouseInput mouseInput = new MouseInput();
        //mouseInput.Player.Enable();
        //mouseInput.Player.PointerSelect.performed += (ctx) => Debug.Log("Standard pointer");
        //mouseInput.Player.CtrlPointerSelect.performed += (ctx) => Debug.Log("Control pointer");
        //mouseInput.Player.PointerPosition.performed += (ctx) => Debug.Log("position");
    }

    private void Action_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("Action");
    }

    private void EdgeSelect_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("edge select");
    }


    private void Focus_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("Focus");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
