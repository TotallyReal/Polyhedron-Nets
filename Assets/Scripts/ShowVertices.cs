using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowVertices : MonoBehaviour
{

    private NetsPlayerInput input;

    [SerializeField] private List<Transform> steps;
    List<Transform>.Enumerator stepEnum;

    private void Awake()
    {
        input = new NetsPlayerInput();
        input.Player.Enable();
        stepEnum = steps.GetEnumerator();
    }

    private void OnEnable()
    {
        input.Player.NextStep.performed += NextStep;
    }

    private void OnDisable()
    {
        input.Player.NextStep.performed -= NextStep;
    }

    private void NextStep(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (stepEnum.MoveNext())
        {
            stepEnum.Current.gameObject.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

}
