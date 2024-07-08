using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseTexture : MonoBehaviour
{

    [SerializeField] private VisualPolyhedronFactory factory;

    [SerializeField] private List<Button> textureButtons;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var button in textureButtons)
        {
            button.onClick.AddListener(() => { ChooseMaterial(button.GetComponent<Image>().material); });
        }
    }

    private void ChooseMaterial(Material material)
    {
        Debug.Log(material.ToString());
        if (factory.GetVisualPolyhedron() != null)
            factory.GetVisualPolyhedron().SetFaceMaterial(material);
    }
}
