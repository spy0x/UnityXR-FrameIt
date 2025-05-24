using UnityEngine;
using UnityEngine.UI;

public class Painting : MonoBehaviour
{
    [SerializeField] private Texture2D picture;
    [SerializeField] private RawImage rawImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rawImage.texture = picture;
        MatchScaleToTexture(picture);
    }

    private float GetTextureAspectRatio(Texture2D texture)
    {
        if (texture == null || texture.height == 0) return 1f; // Fallback to square
        return (float)texture.width / texture.height;
    }
    
    private void MatchScaleToTexture(Texture2D texture)
    {
        float aspectRatio = GetTextureAspectRatio(texture);
    
        transform.localScale = new Vector3(transform.localScale.x * aspectRatio, transform.localScale.y, transform.localScale.z);
    }
}
