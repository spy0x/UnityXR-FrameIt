using System.Collections;
using Meta.XR.MRUtilityKit;
using Oculus.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Painting : MonoBehaviour
{
    [SerializeField] private RawImage rawImage;
    [SerializeField] private GrabFreeTransformer grabFreeTransformer;
    [SerializeField] private float autoPositionDistance = 0.2f;
    [SerializeField] private Transform rayPoint;
    [SerializeField] private TextMeshProUGUI countdownText;
    
    private void MatchScaleToTexture(Texture texture)
    {
        float aspectRatio = GetTextureAspectRatio(texture);
    
        transform.localScale = new Vector3(transform.localScale.x * aspectRatio, transform.localScale.y, transform.localScale.z);
    }

    private float GetTextureAspectRatio(Texture texture)
    {
        if (texture == null || texture.height == 0) return 1f; // Fallback to square
        return (float)texture.width / texture.height;
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
    
    public void SetPicture(Texture newTexture, bool matchScale = true)
    {
        rawImage.texture = newTexture;
        if (matchScale) MatchScaleToTexture(newTexture);
    }

    public void SetCountdownText(string countdown)
    {
        if (countdownText)
        {
            countdownText.text = countdown;
        }
    }
}
