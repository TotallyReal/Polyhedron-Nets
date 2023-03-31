using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTarget : MonoBehaviour
{

    private Renderer targetRenderer;
    [SerializeField] private Material standardMaterial;
    [SerializeField] private Material selectedMaterial;

    private bool selected = false;

    // Start is called before the first frame update
    void Start()
    {
        targetRenderer = GetComponent<Renderer>();
        //targetRenderer.material = standardMaterial;
    }

    public bool IsSelected()
    {
        return selected;
    }

    public void SelectTarget()
    {
        selected = !selected;
        if (selected)
        {
            standardMaterial = targetRenderer.material;
            targetRenderer.material = selectedMaterial;
        } else {
            targetRenderer.material = standardMaterial;
        }
    }

    public void DeselectTarget()
    {
        //targetRenderer.material = standardMaterial;
    }
}
