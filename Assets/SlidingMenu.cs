using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingMenu : MonoBehaviour
{
    public RectTransform menu;
    public GameObject m_Back;
    public float m_Value = 0.0f;
    public float m_SlidingSpeed = 0.01f;
    public float m_SlidingSpeed2 = 0.01f;

    public bool m_bSlidingDownEnabled = true;
    public bool m_bSlidingUpEnabled = true;

    public int m_Id = 0;
    public GameObject m_Handler = null;

    public float m_SliddingThreshold = 10.0f;

    bool m_bSliding = false;
    float m_SlidingToValue = 0.0f;


    public bool m_bBackIgnoreTouch = false;
    public GameObject m_BackIgnoreTouch = null;


    // Use this for initialization
    void Start()
    {

    }

    public bool m_DraggingEnabled = false;

    Vector3 m_MousePositionDown;
    bool m_bDragging = false;
    bool m_bDraggingOverThreshold = false;

    public bool m_bVertical = true;

    bool IsPointInRT(Vector3 point, RectTransform rt)
    {

        Vector2 point2 = new Vector2(point.x, point.y);
        return UnityEngine.RectTransformUtility.RectangleContainsScreenPoint(rt, point2);
        /*
        // Get the rectangular bounding box of your UI element
        Rect rect = rt.rect;

        Debug.Log("Point x: " + point.x + " y: " + point.y);

        // Get the left, right, top, and bottom boundaries of the rect
        float leftSide = rt.anchoredPosition.x - rect.width / 2;
        float rightSide = rt.anchoredPosition.x + rect.width / 2;
        float topSide = rt.anchoredPosition.y + rect.height / 2;
        float bottomSide = rt.anchoredPosition.y - rect.height / 2;

        Debug.Log("left: " + leftSide + " right: " + rightSide + " top: " + topSide + " bottom: " + bottomSide);

        // Check to see if the point is in the calculated bounds
        if (point.x >= leftSide &&
            point.x <= rightSide &&
            point.y >= bottomSide &&
            point.y <= topSide)
        {
            return true;
        }
        return false;*/
    }

    public bool getSliding()
    {
        return m_bSliding;
    }

    public bool getDragging()
    {
        return m_bDragging;
    }
    public bool getDraggingOverThreshold()
    {
        return m_bDraggingOverThreshold;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!GameObject.activeSelf)
        {
            Debug.Log("Sliding menu not active");
            return;
        }*/
      //  Debug.Log("Update sliding menu id: " + m_Id);
        // Debug.Log("m_bSlidingUpEnabled: " + m_bSlidingUpEnabled + " m_bSlidingDownEnabled: " + m_bSlidingDownEnabled);
        if (m_bSliding)
        {
            m_Value = ((m_SlidingToValue - m_Value) * m_SlidingSpeed2) + m_Value;
            float dif = (m_SlidingToValue - m_Value);
            if (dif < 0.0f) dif *= -1.0f;
            if (dif < 0.02f)
            {
                m_Value = m_SlidingToValue;
                m_bSliding = false;

                DemoMap map = m_Handler.GetComponent<DemoMap>();
                map.SlidingMenuFinished(m_Id);
            }/**/
        }
        float ratio = (float)Screen.width / (float)Screen.height;

        if (m_bVertical == false)
        {
            ratio = 1.0f;//(float)Screen.height / (float)Screen.width;
        }

        float value = m_Value;
        float offset = 0.0f;

        m_bDraggingOverThreshold = false;

        if (m_DraggingEnabled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("#### Sliding Menu down #####");
                m_MousePositionDown = Input.mousePosition;

                if (IsPointInRT(m_MousePositionDown, m_Back.GetComponent<RectTransform>()) &&
                    (!m_bBackIgnoreTouch || m_BackIgnoreTouch == null || !IsPointInRT(m_MousePositionDown, m_BackIgnoreTouch.GetComponent<RectTransform>())))
                {
                    m_bDragging = true;
                }
            }
            if (Input.GetMouseButton(0) && m_bDragging)
            {
                offset = Input.mousePosition.y - m_MousePositionDown.y;
                if (m_bVertical == false)
                {
                    float vertical = offset;

                    offset = Input.mousePosition.x - m_MousePositionDown.x;
                    if(vertical > 70.0f) {
                        offset = 0.0f;
                        m_MousePositionDown.y = -100000;
                    }
                }
                float vecx = Input.mousePosition.x - m_MousePositionDown.x;
                float vecy = Input.mousePosition.y - m_MousePositionDown.y;
                if (vecx < 0.0f) vecx *= -1.0f;
                if (vecy < 0.0f) vecy *= -1.0f;
                if(m_bVertical == false) {
                    if (vecy > vecx) offset = 0.0f;
                } else {
                    if (vecx > vecy) offset = 0.0f;
                }

                float offsetsign = offset;
                if (m_bVertical)
                {
                    offset /= Screen.height;
                } else {
                    offset /= Screen.width;
                }

                if (m_bSlidingUpEnabled == false)
                {
                    if (offset > 0.0f)
                    {
                        offset = 0.0f;
                        offsetsign = 0.0f;
                    }
                }
                if (m_bSlidingDownEnabled == false)
                {
                    if (offset < 0.0f)
                    {
                        offset = 0.0f;
                        offsetsign = 0.0f;
                    }
                }

               // Debug.Log("Offset menu dragging: " + offsetsign);

                if (offsetsign < 0.0f) offsetsign *= -1.0f;
                if (offsetsign > 10.0f)
                {
                    m_bDraggingOverThreshold = true;
                    DemoMap map = m_Handler.GetComponent<DemoMap>();
                    map.SlidingMenu();
                }

                if (offsetsign < m_SliddingThreshold)
                {
                    offset = 0.0f;
                }
            }
            else // Stopped dragging
            {
                if (m_bDragging)
                {
                    float vecx = Input.mousePosition.x - m_MousePositionDown.x;
                    float vecy = Input.mousePosition.y - m_MousePositionDown.y;

                    offset = Input.mousePosition.y - m_MousePositionDown.y;
                    if (m_bVertical == false)
                    {
                        float vertical = offset;
                       // if (vertical < 0.0f) vertical *= -1.0f;
                        offset = Input.mousePosition.x - m_MousePositionDown.x;
                        if (vertical > 70.0f)
                        {
                            offset = 0.0f;
                            m_MousePositionDown.y = -100000;
                        }
                    }
                   
                    if (vecx < 0.0f) vecx *= -1.0f;
                    if (vecy < 0.0f) vecy *= -1.0f;
                    if (m_bVertical == false)
                    {
                        Debug.Log("Sliding menu release vertical== false vecx: " + vecx + " vecy: " + vecy);
                        if (vecy > vecx)
                        {
                            offset = 0.0f;
                            Debug.Log("offset = 0.0");
                        }
                    }
                    else
                    {
                        Debug.Log("Sliding menu release vertical==true vecx: " + vecx + " vecy: " + vecy);
                        if (vecx > vecy)
                        {
                            offset = 0.0f;
                            Debug.Log("offset = 0.0");
                        }
                    }

                    if (offset > m_SliddingThreshold)
                    {
                        if (m_bSlidingUpEnabled)
                        {
                            Debug.Log("Close menu");
                            DemoMap map = m_Handler.GetComponent<DemoMap>();
                            map.CloseMenu(m_Id);

                            if (m_bVertical)
                            {
                                offset /= Screen.height;
                            }
                            else
                            {
                                offset /= Screen.width;
                            }
                            offset /= ratio;
                            m_Value -= offset;
                            value -= offset;
                        }
                    }
                    else if (offset < -m_SliddingThreshold)
                    {
                        if (m_bSlidingDownEnabled)
                        {
                            Debug.Log("Open menu");
                            DemoMap map = m_Handler.GetComponent<DemoMap>();
                            map.OpenMenu(m_Id);

                            if (m_bVertical)
                            {
                                offset /= Screen.height;
                            }
                            else
                            {
                                offset /= Screen.width;
                            }
                            offset /= ratio;
                            m_Value -= offset;
                            value -= offset;
                        }
                    }
                }

                offset = 0.0f;
                m_bDragging = false;
            }
        }

        if (m_bVertical)
        {
            menu.anchorMin = new Vector2(0.5f, 1.0f - value * ratio + offset);
            menu.anchorMax = new Vector2(0.5f, 1.0f - value * ratio + offset);
        }
        else
        {
            menu.anchorMin = new Vector2(1.0f - value * ratio + offset, 0.5f);
            menu.anchorMax = new Vector2(1.0f - value * ratio + offset, 0.5f);
        }
    }

    public void SlideToValue(float value)
    {
        m_SlidingToValue = value;
        m_bSliding = true;
    }

    public void SlideToFullScreen()
    {
        float value = (float)Screen.height / (float)Screen.width;

        m_SlidingToValue = value;
        m_bSliding = true;
    }

    public void ForceToValue(float value)
    {
        m_SlidingToValue = value;
        m_Value = value;
        m_bSliding = true;
    }

    public void SetValue(float value)
    {
        m_Value = value;
    }
}
