using System.Collections;
using Meta.XR.MRUtilityKit;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.UI;

public class Painting : MonoBehaviour
{
    [SerializeField] private Texture2D picture;
    [SerializeField] private RawImage rawImage;
    [SerializeField] private GrabFreeTransformer grabFreeTransformer;
    [SerializeField] private float autoPositionDistance = 0.2f;
    [SerializeField] private Transform rayPoint;
    public Texture2D Picture { get => picture; set => picture = value; }
    void Start()
    {
        // yield return new WaitForEndOfFrame(); // Ensure the UI is ready before setting the texture
        rawImage.texture = picture;
        MatchScaleToTexture(picture);
        grabFreeTransformer.enabled = true;
    }

    private float GetTextureAspectRatio(Texture2D texture)
    {
        if (texture == null || texture.height == 0) return 1f; // Fallback to square
        Debug.Log($"Texture Width: {texture.width}, Height: {texture.height}");
        return (float)texture.width / texture.height;
    }
    
    private void MatchScaleToTexture(Texture2D texture)
    {
        float aspectRatio = GetTextureAspectRatio(texture);
    
        transform.localScale = new Vector3(transform.localScale.x * aspectRatio, transform.localScale.y, transform.localScale.z);
    }

    public void TryPlaceOnWall()
    {
        MRUKRoom room = MRUK.Instance.GetCurrentRoom();
        if (!room) return;
        Ray ray = new Ray(rayPoint.position, rayPoint.forward);
        if (room.Raycast(ray, autoPositionDistance, out RaycastHit hit, out MRUKAnchor anchor))
        {
            if(anchor.Label != MRUKAnchor.SceneLabels.WALL_FACE) return;
            transform.position = hit.point;
            transform.rotation = Quaternion.LookRotation(hit.normal);
        }
    }
}
