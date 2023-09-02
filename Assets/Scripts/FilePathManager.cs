using UnityEngine;

[CreateAssetMenu(fileName = "FilePathManager", menuName = "ScriptableObjects/FilePathManager")]
public class FilePathManager : ScriptableObject
{
    public string objectName;
    public string albedoTextureName;
    public string specularTextureName;
    public string normalMapName;

    public string objectPath;
    public string albedoTexturePath;
    public string specularTexturePath;
    public string normalMapPath;
}
