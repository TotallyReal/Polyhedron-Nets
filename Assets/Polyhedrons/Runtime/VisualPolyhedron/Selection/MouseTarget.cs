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

    public void SetStandardMaterial(Material material)
    {
        if (material != null)
        {
            standardMaterial = material;
            if (targetRenderer == null)
                targetRenderer = GetComponent<Renderer>(); // TODO: added because it can run in editor. Find a way around this.
            if (!selected)
                targetRenderer.material = standardMaterial;
        }
    }

    public void SetSelectedMaterial(Material material)
    {
        if (material != null)
        {
            selectedMaterial = material;
            if (targetRenderer == null)
                targetRenderer = GetComponent<Renderer>(); // TODO: added because it can run in editor. Find a way around this.
            if (selected)
                targetRenderer.material = selectedMaterial;
        }
    }

    public bool IsSelected()
    {
        return selected;
    }

    public void SelectTarget()
    {
        if (!selectable)
            return;
        SetSelected(!selected);
    }

    public void DeselectTarget()
    {
        //targetRenderer.material = standardMaterial;
    }

    internal void SetSelected(bool selected)
    {
        this.selected = selected;
        if (selected)
        {
            standardMaterial = targetRenderer.material;
            targetRenderer.material = selectedMaterial;
        }
        else
        {
            targetRenderer.material = standardMaterial;
        }
    }
}
