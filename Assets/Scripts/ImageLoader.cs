using System.IO;
using PassthroughCameraSamples;
using UnityEngine;

public class ImageLoader : MonoBehaviour
{
    [SerializeField] private GameObject paintingPrefab; // Reference to the Painting prefab
    [SerializeField] private WebCamTextureManager webCamTextureManager;

    private string[] fileTypes;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // imageFileType = NativeFilePicker.ConvertExtensionToFileType("jpg");
    }

    public void LoadImage()
    {
#if UNITY_ANDROID
        fileTypes = new string[] { "image/*" };
#else
		fileTypes = new string[] { "public.image" };
#endif
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

    public void TakePicture()
    {
        if (webCamTextureManager.WebCamTexture)
        {
            WebCamTexture webCamTexture = webCamTextureManager.WebCamTexture;
            Texture2D copy = new Texture2D(webCamTexture.width, webCamTexture.height, TextureFormat.RGBA32, false);
            copy.SetPixels32(webCamTexture.GetPixels32());
            copy.Apply();
            Painting spawnedPainting = Instantiate(paintingPrefab).GetComponent<Painting>();
            spawnedPainting.Picture = copy;
        }
        else
        {
            Debug.LogError("WebCamTexture is not available.");
        }
    }
}