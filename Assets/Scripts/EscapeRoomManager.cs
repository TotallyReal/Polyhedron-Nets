using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeRoomManager : MonoBehaviour
{
    public static EscapeRoomManager Instance;

    [SerializeField] private VisualPolyhedronFactory mainPolyhedronFactory;
    [SerializeField] private VisualPolyhedronFactory shadowPolyhedronFactory;

    private void Awake()
    {
        Instance = this;
    }

    public void RestartPolyhedrons(bool restartShadow = true)
    {
        mainPolyhedronFactory.CreatePolyhedron();
        if (restartShadow)
            shadowPolyhedronFactory.CreatePolyhedron();
    }
}
