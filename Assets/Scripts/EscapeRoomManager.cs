using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeRoomManager : MonoBehaviour
{
    public static EscapeRoomManager Instance;

    [SerializeField] private VisualPolyhedronFactory mainPolyhedronFactory;
    [SerializeField] private VisualPolyhedronFactory shadowPolyhedronFactory;

    [SerializeField] private List<VisualPolyhedronFactory> factories;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        /*foreach (var factory in factories)
        {
            factory.CreatePolyhedron();
        }*/
    }

    public void RestartPolyhedrons(bool restartShadow = true)
    {
        mainPolyhedronFactory.CreatePolyhedron();
        if (restartShadow)
            shadowPolyhedronFactory.CreatePolyhedron();
    }
}
