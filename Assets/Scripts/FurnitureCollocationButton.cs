using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

public class FurnitureCollocationButton : MonoBehaviour
{
    [SerializeField]
    GameObject placementIndicator;

    public Sprite[] buttonImages;
    public Button button;
    public Transform furniturePool;
    public ARRaycastManager arRaycastManager;

    private GameObject instantiatedGesture;
    private GameObject loadFurniture;
    private string furniturePath;

    // Start is called before the first frame update.
    void Awake()
    {
        furniturePath = "Furniture/###";
        loadFurniture = Resources.Load<GameObject>(furniturePath);
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

            GameObject placeFurniture = Instantiate(loadFurniture, placementIndicator.transform.position, placementIndicator.transform.rotation) as GameObject;
            furniturePool.transform.position = placementIndicator.transform.position;
            placeFurniture.transform.SetParent(furniturePool);
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