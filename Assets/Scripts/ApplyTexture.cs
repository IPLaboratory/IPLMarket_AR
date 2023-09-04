using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Android;

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

    // Start is called before the first frame update
    void Start()
    {
        specularBytes = File.ReadAllBytes(FilePathManager.filePath + "/" + FilePathManager.specularTextureName);
        albedoBytes = File.ReadAllBytes(FilePathManager.filePath + "/" + FilePathManager.albedoTextureName);
        normalBytes = File.ReadAllBytes(FilePathManager.filePath + "/" + FilePathManager.normalMapName);

        specular = new Texture2D(0, 0);
        albedo = new Texture2D(0, 0);
        normal = new Texture2D(0, 0);

        specular.LoadImage(specularBytes);
        albedo.LoadImage(albedoBytes);
        normal.LoadImage(normalBytes);

        material.SetTexture("_specular", specular);
        material.SetTexture("_albedo", albedo);
        material.SetTexture("_normal", normal);
    }
}
