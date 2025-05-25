using System.IO;
using UnityEngine;

public class ImageLoader : MonoBehaviour
{
    [SerializeField] private GameObject paintingPrefab; // Reference to the Painting prefab

    private string[] fileTypes;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // imageFileType = NativeFilePicker.ConvertExtensionToFileType("jpg");
#if UNITY_ANDROID
        fileTypes = new string[] { "image/*" };
#else
		fileTypes = new string[] { "public.image" };
#endif
    }

    public void LoadImage()
    {
        NativeFilePicker.PickFile((path) =>
        {
            if (path != null)
            {
                Debug.Log("Image path: " + path);
                byte[] fileData = File.ReadAllBytes(path);
                Texture2D texture = new Texture2D(2, 2); // Create a new texture
                if (texture.LoadImage(fileData)) // This auto-resizes the texture
                {
                    Painting spawnedPainting = Instantiate(paintingPrefab).GetComponent<Painting>();
                    spawnedPainting.Picture = texture;
                }
                else
                {
                    Debug.LogError("Failed to load image data into texture.");
                }
            }
            else
            {
                Debug.Log("No image selected");
            }
        }, fileTypes);
    }
}