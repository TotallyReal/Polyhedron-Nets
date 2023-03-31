using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseTexture : MonoBehaviour
{

    public static ChooseTexture Instance { get; private set; }
    private VisualPolyhedron visualPolyhedron;


    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private List<Button> textureButtons;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var button in textureButtons)
        {
            button.onClick.AddListener(() => { ChooseMaterial(button.GetComponent<Image>().material); });
        }
        

    }

    public void SetVisualPolyhedron(VisualPolyhedron visualPolyhedron)
    {
        this.visualPolyhedron = visualPolyhedron;
    }

    private void ChooseMaterial(Material material)
    {
        Debug.Log(material.ToString());
        if (visualPolyhedron != null)
            visualPolyhedron.SetFaceMaterial(material);
    }
}
