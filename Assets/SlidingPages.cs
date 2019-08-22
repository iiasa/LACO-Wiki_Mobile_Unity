using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingPages : MonoBehaviour
{
    public RectTransform menu;
    public GameObject m_Back;
    public GameObject m_BackIgnore1 = null;
    public GameObject m_BackIgnore2 = null;
    public float m_Value = 0.0f;
    public float m_SlidingToValue = 0.0f;

    public int m_Id = 0;

    bool m_bSliding = false;

    public int m_CurPage = 0;
    public int m_NrPages = 3;

    public float m_SlidingThreshold = 0.1f;
    public float m_SlidingSpeed2 = 0.3f;
    public GameObject m_Viewport;

    public GameObject m_PageChangeListener = null;

    // Use this for initialization
    void Start()
    {

    }


    Vector3 m_MousePositionDown;
    bool m_bDragging = false;
    bool m_bDraggingOverThreshold = false;

    bool IsPointInRT(Vector3 point, RectTransform rt)
    {
        Vector2 point2 = new Vector2(point.x, point.y);
        return UnityEngine.RectTransformUtility.RectangleContainsScreenPoint(rt, point2);
    }


    // Update is called once per frame
    void Update()
    {
        float ratio = 1.0f;

        float value = m_Value;
        float offset = 0.0f;

        m_bDraggingOverThreshold = false;

        if (Input.GetMouseButtonDown(0))
        {
            m_MousePositionDown = Input.mousePosition;

            bool bInsideIgnore = false;
            if (m_BackIgnore1 != null && IsPointInRT(m_MousePositionDown, m_BackIgnore1.GetComponent<RectTransform>()))
            {
                bInsideIgnore = true;
            }
            if (m_BackIgnore2 != null && IsPointInRT(m_MousePositionDown, m_BackIgnore2.GetComponent<RectTransform>()))
            {
                bInsideIgnore = true;
            }
            if (IsPointInRT(m_MousePositionDown, m_Back.GetComponent<RectTransform>()) && bInsideIgnore == false)
            {
                m_bDragging = true;
            }
        }
        if (Input.GetMouseButton(0) && m_bDragging)
        {
            offset = Input.mousePosition.x - m_MousePositionDown.x;
            offset /= Screen.width;
        }

        m_CurPage = (int)-m_Value;

       // Debug.Log("SlidingPages Offset: " + offset);
        menu.anchorMin = new Vector2(0.0f + offset + m_Value, 1.0f);
        menu.anchorMax = new Vector2(0.0f + offset + m_Value, 1.0f);

        if(m_bDragging && (offset > m_SlidingThreshold || offset < -m_SlidingThreshold)) {
            CanvasGroup group = m_Viewport.GetComponent<CanvasGroup>();
            group.blocksRaycasts = false;
        } else {
            CanvasGroup group = m_Viewport.GetComponent<CanvasGroup>();
            group.blocksRaycasts = true;
        }
    }

}
