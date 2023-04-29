using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCenterSelector : RaycastSelector
{

    void Start()
    {
        base.ChangeMousePositionForRaycasts(ScreenCenter);
    }

    public Vector2 ScreenCenter()
    {
        return new Vector2(Screen.width / 2, Screen.height / 2);
    }

}
