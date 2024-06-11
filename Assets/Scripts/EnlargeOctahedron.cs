using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class EnlargeOctahedron : MonoBehaviour
{

    NetsPlayerInput input;
    [SerializeField] Transform cube;

    private void Awake()
    {
        input = new NetsPlayerInput();
        input.Player.Enable();
    }

    private void OnEnable()
    {
        input.Player.NextStep.performed += EnlageOctahedron;
    }

    private void OnDisable()
    {
        input.Player.NextStep.performed -= EnlageOctahedron;
    }

    private float scale = 1f;

    private void EnlageOctahedron(InputAction.CallbackContext obj)
    {
        transform.DOScale(scale, 1);
        if (scale > 1)
        {
            cube.transform.DOScale(0.5f, 1);
        }
        scale += 0.5f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
