using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
using Dummiesman;
using UnityEngine.Android;
using System.IO;

public class FurnitureCollocationButton : MonoBehaviour
{
    [SerializeField]
    GameObject placementIndicator;

    public Sprite[] buttonImages;
    public Button button;
    public Transform furniturePool;
    public ARRaycastManager arRaycastManager;
    public Material furnitureMaterial;
    public GameObject gesture;

    private GameObject instantiatedGesture;
    
    private string furniturePath;

    public FilePathManager filePathManager;

    // Start is called before the first frame update.
    void Awake()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
        }
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }

        furniturePath = filePathManager.filePath + "/" + filePathManager.objectName;
        button.onClick.AddListener(CollocationButtonClickEvent);
    }

    // Clickevent that collocates furniture(object) where indicator locates.
    void CollocationButtonClickEvent()
    {
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

        if (buttonText.text.Equals("Image_Input"))
        {
            button.GetComponent<Image>().sprite = buttonImages[0];
            placementIndicator.SetActive(false);

            GameObject placeFurniture = new OBJLoader().Load(furniturePath);
            placeFurniture.transform.position = placementIndicator.transform.position;
            placeFurniture.transform.rotation = placementIndicator.transform.rotation;

            furniturePool.transform.position = placementIndicator.transform.position;
            placeFurniture.transform.SetParent(furniturePool);
            furniturePool.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = furnitureMaterial;
            placeFurniture.transform.position = new Vector3(
                placementIndicator.transform.position.x, placementIndicator.transform.position.y + (GetHeight(placeFurniture) / 2f), placementIndicator.transform.position.z);

            buttonText.SetText("Delete");
        }
        else if (buttonText.text.Equals("Delete"))
        {
            button.GetComponent<Image>().sprite = buttonImages[1];
            furniturePool.transform.localScale = new Vector3(1f, 1f, 1f);

            placementIndicator.SetActive(true);
            Destroy(furniturePool.GetChild(0).gameObject);

            buttonText.SetText("Image_Input");
            Destroy(instantiatedGesture);
        }
    }

    // Get height of input gameobject.
    float GetHeight(GameObject gameObject)
    {
        MeshFilter meshFilter = gameObject.GetComponentInChildren<MeshFilter>();
        Bounds bounds = meshFilter.sharedMesh.bounds;
        return bounds.size.y;
    }
}
