using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

[DefaultExecutionOrder(-1)]
public class DefaultMouseTypeEvent : MonoBehaviour
{
    public static MouseTypeEvent standard { get; private set; }
    public static MouseTypeEvent alternative { get; private set; }

    [SerializeField] MouseTypeEvent mouseEventLocal;
    [SerializeField] MouseTypeEvent rightMouseEventLocal;


    private void Awake()
    {
        standard = mouseEventLocal;
        alternative = rightMouseEventLocal;
    }
}
