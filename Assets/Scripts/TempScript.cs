using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempScript : MonoBehaviour
{

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
