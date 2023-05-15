using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class JoyStickController : MonoBehaviour
{
    RectTransform handle;
    RectTransform outLine;

    private float deadZone = 0;
    private float hadndleRange = 0.8f;
    private Vector3 input = Vector3.zero;
    private Canvas canvas;

    public float Horizontal { get { return input.x; } }
    public float Vertical { get { return input.y; } }

    private CanvasGroup canvasGroup;
    private bool isTouching = false;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        outLine = transform.Find("JoyStick").GetComponent<RectTransform>();
        handle = transform.Find("JoyStick").Find("Handle").GetComponent<RectTransform>(); ;
        canvasGroup = outLine.GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Input.mousePosition.x <= 1600)
        {
            OnPointerDown();
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnPointerUp();
        }

        if (isTouching)
        {
            OnDrag();
        }

    }

    public void OnPointerDown()
    {
        canvasGroup.alpha = 1f;
        outLine.transform.position = Input.mousePosition;
        isTouching = true;
    }

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    canvasGroup.alpha = 1f;
    //    outLine.transform.position = Input.mousePosition;
    //    OnDrag(eventData);
    //}

    public void OnDrag()
    {
        Vector2 radius = outLine.sizeDelta / 2;
        input = (Input.mousePosition - (Vector3)outLine.anchoredPosition) / (radius * canvas.scaleFactor);
        HandleInput(input.magnitude, input.normalized);
        handle.anchoredPosition = input * radius * hadndleRange / 3;
    }

    //public void OnDrag(PointerEventData eventData)
    //{
    //    Vector2 radius = outLine.sizeDelta / 2;
    //    input = (eventData.position - outLine.anchoredPosition) / (radius * canvas.scaleFactor);
    //    HandleInput(input.magnitude, input.normalized);
    //    handle.anchoredPosition = input * radius * hadndleRange;
    //}

    private void HandleInput(float magnitude, Vector2 normalised)
    {
        if (magnitude > deadZone)
        {
            if (magnitude > 1)
            {
                input = normalised;
            }
        }
        else
        {
            input = Vector2.zero;
        }
    }

    public void OnPointerUp()
    {
        input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
        canvasGroup.alpha = 0f;
        isTouching = false;
    }

    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    input = Vector2.zero;
    //    handle.anchoredPosition = Vector2.zero;
    //    canvasGroup.alpha = 0f;
    //}
    // Update is called once per frame
}