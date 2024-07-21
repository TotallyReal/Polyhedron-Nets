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
    private bool selectable = true;

    public void SetSelectable(bool selectable)
    {
        this.selectable = selectable;
    }

    void Awake()
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
        if (!selectable)
            return;
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
