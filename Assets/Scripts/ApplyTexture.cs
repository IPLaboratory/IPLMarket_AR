using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyTexture : MonoBehaviour
{
    public Material material;

    private Texture specular;
    private Texture albedo;
    private Texture normal;

    private void Awake()
    {
        specular = Resources.Load<Texture>("Furniture/texture_ks");
        albedo = Resources.Load<Texture>("Furniture/texture_kd");
        normal = Resources.Load<Texture>("Furniture/texture_n");
    }

    // Start is called before the first frame update
    void Start()
    {
        material.SetTexture("_specular", specular);
        material.SetTexture("_albedo", albedo);
        material.SetTexture("_normal", normal);
    }
}
