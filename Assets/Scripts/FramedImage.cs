using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramedImage : MonoBehaviour
{

    [SerializeField] private Transform frame;
    [SerializeField] private Renderer plane;
    [SerializeField] private Texture2D texture;
    [SerializeField] private float width;

    private float imageScale = 0.79f;
    private float imagePosition = 0.61f;

    private void OnValidate()
    {
        float ratio = (float)texture.height / texture.width;
        plane.transform.localScale = new Vector3(width / 10f, 1, width * ratio / 10f);
        plane.material.SetTexture("_MainTex", texture);
        frame.localScale = new Vector3(width * imageScale, 1, width * ratio * 0.75f);
        frame.localPosition = new Vector3(-width * imagePosition, 0, 0);
    }

}
