using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsUI : MonoBehaviour
{
    [SerializeField] private Button unfoldButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button compareButton;

    // Start is called before the first frame update
    void Start()
    {
        unfoldButton.onClick.AddListener(() => { VisualPolyhedronFactory.Instance.CreateTransformGraph(); });
        restartButton.onClick.AddListener(() => { VisualPolyhedronFactory.Instance.CreatePolyhedron(); });
        compareButton.onClick.AddListener(() => { VisualPolyhedronFactory.Instance.CompareUnfolding(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
