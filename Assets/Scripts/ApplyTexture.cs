using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ApplyTexture : MonoBehaviour
{
    public Material material;

    private Texture2D specular;
    private Texture2D albedo;
    private Texture2D normal;

    byte[] specularBytes = File.ReadAllBytes("/data/data/com.DefaultCompany.IPL_Test/files/texture_ks.png");
    byte[] albedoBytes = File.ReadAllBytes("/data/data/com.DefaultCompany.IPL_Test/files/texture_kd.png");
    byte[] normalBytes = File.ReadAllBytes("/data/data/com.DefaultCompany.IPL_Test/files/texture_n.png");

    private void Awake()
    {
        specular = new Texture2D(0, 0);
        albedo = new Texture2D(0, 0);
        normal = new Texture2D(0, 0);

        specular.LoadImage(specularBytes);
        albedo.LoadImage(albedoBytes);
        normal.LoadImage(normalBytes);
    }

    // Start is called before the first frame update
    void Start()
    {
        material.SetTexture("_specular", specular);
        material.SetTexture("_albedo", albedo);
        material.SetTexture("_normal", normal);
    }
}
