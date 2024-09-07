using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cookie : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //点击后的大小
    public float scale=1.1f;

    public float rotationSpeed = 45.0f;

    /// <summary>
    /// 鼠标按下
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * scale;
    }

    /// <summary>
    /// 鼠标抬起
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
    }

    public void Update()
    {
        transform.Rotate(Vector3.forward * -rotationSpeed * Time.deltaTime);
    }   
}
