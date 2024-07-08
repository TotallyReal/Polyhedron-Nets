using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach to camera focus locations
/// </summary>
public abstract class Focus : MonoBehaviour 
{

    abstract public void LoseFocus();

    abstract public void GetFocus();

    public void TakeFocusFrom(Focus other)
    {
        other.LoseFocus();
        GetFocus();
    }

}
