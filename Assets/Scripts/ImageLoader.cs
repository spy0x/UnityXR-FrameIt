using System.Collections;
using System.IO;
using PassthroughCameraSamples;
using UnityEngine;

public class ImageLoader : MonoBehaviour
{
    [SerializeField] private GameObject paintingPrefab; // Reference to the Painting prefab
    [SerializeField] private WebCamTextureManager webCamTextureManager;
    [SerializeField] private Transform playerHead;
    [SerializeField] private float cameraCaptureDelay = 3f; // Delay to allow camera to initialize
    [SerializeField] private OVRPassthroughLayer passthroughLayer; // Reference to the passthrough layer
    [SerializeField] private AnimationCurve cameraBrightnessCurve;
    [SerializeField] private AnimationCurve cameraContrastCurve;
    [SerializeField] private float cameraFlashbackDuration = 0.5f; // Duration of the camera flashback effect
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip cameraFlashSound;
    [SerializeField] private AudioClip cameraCountdownSound;
    [SerializeField] private float passthroughBrightnessCamera = -0.3f; // Brigthness for the passthrough layer during camera capture

    private string[] fileTypes;
    private Coroutine cameraCoroutine;

    public void LoadImage()
    {
#if UNITY_ANDROID
        fileTypes = new string[] { "image/*" };
#else
		fileTypes = new string[] { "public.image" };
#endif
        NativeFilePicker.PickFile((path) =>
        {
            if (path == null) return;
            byte[] fileData = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(2, 2); // Create a new texture
            if (texture.LoadImage(fileData)) // This auto-resizes the texture
            {
                Painting spawnedPainting = Instantiate(paintingPrefab, playerHead).GetComponent<Painting>();
                spawnedPainting.SetPicture(texture);
                StartCoroutine(DetachPaintingAfterDelay(spawnedPainting, .5f)); // Detach after .5 second
            }
        }, fileTypes);
    }

    public void TakePicture()
    {
        if (cameraCoroutine != null) return;
        cameraCoroutine = StartCoroutine(CaptureWebCamTexture());
    }

    private IEnumerator CaptureWebCamTexture()
    {
        webCamTextureManager.gameObject.SetActive(true);
        yield return new WaitUntil(() => webCamTextureManager.WebCamTexture);
        passthroughLayer.colorMapEditorBrightness = passthroughBrightnessCamera; // Set the passthrough opacity
        Painting painting = Instantiate(paintingPrefab, playerHead).GetComponent<Painting>();
        painting.SetPicture(webCamTextureManager.WebCamTexture);
        float elapsedTime = 0f;
        int currentSecond = 0;
        while (elapsedTime < cameraCaptureDelay)
        {
            elapsedTime += Time.deltaTime;
            int countdown = Mathf.CeilToInt(cameraCaptureDelay - elapsedTime);
            painting.SetCountdownText(countdown.ToString());
            if (audioSource && cameraCountdownSound)
            {
                if (currentSecond != countdown)
                {
                    currentSecond = countdown;
                    audioSource.PlayOneShot(cameraCountdownSound);
                }
            }
            yield return null;
        }
        painting.SetCountdownText("");
        Texture2D copy = new Texture2D(webCamTextureManager.WebCamTexture.width,
            webCamTextureManager.WebCamTexture.height, TextureFormat.RGBA32, false);
        copy.SetPixels32(webCamTextureManager.WebCamTexture.GetPixels32());
        copy.Apply();
        StartCoroutine(DetachPaintingAfterDelay(painting, .0f)); // Detach after 1 second
        StartCoroutine(CameraFlash());
        painting.SetPicture(copy, false);
        webCamTextureManager.gameObject.SetActive(false);
        cameraCoroutine = null;
    }

    private IEnumerator DetachPaintingAfterDelay(Painting painting, float delay)
    {
        yield return new WaitForSeconds(delay);
        painting.transform.SetParent(null); // Detach from player head
    }

    private IEnumerator CameraFlash()
    {
        if (!passthroughLayer) yield break;
        float elapsedTime = 0f;
        if (audioSource && cameraFlashSound)
        {
            audioSource.PlayOneShot(cameraFlashSound);
        }

        while (elapsedTime < cameraFlashbackDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / cameraFlashbackDuration;
            float brightness = cameraBrightnessCurve.Evaluate(t);
            passthroughLayer.colorMapEditorBrightness = brightness;
            float contrast = cameraContrastCurve.Evaluate(t);
            passthroughLayer.colorMapEditorContrast = contrast;
            yield return null;
        }
    }
}