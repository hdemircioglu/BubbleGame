using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LinkHandler : MonoBehaviour, IPointerClickHandler
{
    private TMP_Text textMeshPro;

    void Awake()
    {
        textMeshPro = GetComponent<TMP_Text>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Get the index of the clicked character
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMeshPro, Input.mousePosition, null);

        if (linkIndex != -1)
        {
            // Get the link information
            TMP_LinkInfo linkInfo = textMeshPro.textInfo.linkInfo[linkIndex];
            string url = linkInfo.GetLinkID();

            try
            {
                // Debug log to see what URL we're trying to open
                Debug.Log($"Attempting to open URL: {url}");
                
                // Check if the URL is valid
                if (url.StartsWith("http://") || url.StartsWith("https://"))
                {
                    Application.OpenURL(url);
                }
                else
                {
                    // If URL doesn't have protocol, add https://
                    Application.OpenURL("https://" + url);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to open URL: {url}. Error: {e.Message}");
            }
        }
    }
}
