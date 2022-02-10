using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TMP_Text))]
public class OpenHyperlink : MonoBehaviour, IPointerClickHandler
{
    private TMP_Text m_textMeshPro;
    private Camera m_uiCamera;
    void Start()
    {
        m_textMeshPro = GetComponent<TMP_Text>();
        Camera[] cameras = FindObjectsOfType<Camera>();
        for (int i = 0; i < cameras.Length; i++)
        {
            if (cameras[i].CompareTag("UICamera")) // this may be whatever for your case
            {
                m_uiCamera = cameras[i];
                break;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(m_textMeshPro, Input.mousePosition, null);
        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = m_textMeshPro.textInfo.linkInfo[linkIndex];
            Application.OpenURL(linkInfo.GetLinkID());
        }
    }
}