using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TextureTilingController : MonoBehaviour
{

    [SerializeField] private float textureScale = 2f; // Use this to contrain texture to a certain size

    private Material material;

    Vector3 prevScale = Vector3.one;
    float prevTextureScale = -1f;


    [ContextMenu("Awake")]
    private void Awake()
    {
        // TODO : can I extract the material here, or should I use sharedMaterial?
        material = GetComponent<Renderer>().material;       
    }

    // Use this for initialization
    void Start()
    {
        this.prevScale = gameObject.transform.lossyScale;
        this.prevTextureScale = this.textureScale;

        this.UpdateTiling();
    }

    // Update is called once per frame
    void Update()
    {
        // If something has changed
        if (gameObject.transform.lossyScale != prevScale || !Mathf.Approximately(textureScale, prevTextureScale))
            this.UpdateTiling();

        // Maintain previous state variables
        this.prevScale = gameObject.transform.lossyScale;
        this.prevTextureScale = this.textureScale;
    }

    [ContextMenu("UpdateTiling")]
    void UpdateTiling()
    {
        // A Unity plane is 10 units x 10 units
        float planeSizeX = 10f;
        float planeSizeZ = 10f;
        Texture texture = material.mainTexture;

        material.mainTextureScale = new Vector2(
            textureScale * planeSizeX * gameObject.transform.lossyScale.x / texture.width,
            textureScale * planeSizeZ * gameObject.transform.lossyScale.z / texture.height);
    }
}
