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

    public FilePathManager FilePathManager;

    byte[] specularBytes;
    byte[] albedoBytes;
    byte[] normalBytes;

    private void Awake()
    {
        specularBytes = File.ReadAllBytes(FilePathManager.specularTexturePath + FilePathManager.specularTextureName);
        albedoBytes = File.ReadAllBytes(FilePathManager.albedoTexturePath + FilePathManager.albedoTextureName);
        normalBytes = File.ReadAllBytes(FilePathManager.normalMapPath + FilePathManager.normalMapName);

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
