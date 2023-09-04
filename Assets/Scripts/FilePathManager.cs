using UnityEngine;

[CreateAssetMenu(fileName = "FilePathManager", menuName = "ScriptableObjects/FilePathManager")]
public class FilePathManager : ScriptableObject
{
    public string objectName;
    public string albedoTextureName;
    public string specularTextureName;
    public string normalMapName;

    public string filePath;
}
