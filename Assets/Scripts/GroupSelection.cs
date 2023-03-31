using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupSelection : MonoBehaviour
{

    public static GroupSelection Instance { get; private set; }

    public delegate void OnSelected();
    [SerializeField] private bool selectionActive = true;


    private Dictionary<Transform, OnSelected> selectionGroups;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("GroupSelection Instance is already defined.");
            return;
        }
        Instance = this;

        selectionGroups = new Dictionary<Transform, OnSelected>();
    }

    private void Start()
    {
        MouseSelector.Instance.OnObjectPressed += OnObjectPressed;
    }


    /// <summary>
    /// Registers the given object (represented by the Transform variable), so that 
    /// onSelected will be called when the object is pressed
    /// </summary>
    /// <param name="transform"> The object registered </param>
    /// <param name="onSelected"> The function to call if the object is pressed</param>
    /// <returns></returns>
    public void Register(Transform transform, OnSelected onSelected)
    {
        selectionGroups.Add(transform, onSelected);
    }

    private void OnObjectPressed(object sender, Transform e)
    {
        if (selectionActive && selectionGroups.TryGetValue(e, out OnSelected onSelected)){
            onSelected();
        }
    }
}
