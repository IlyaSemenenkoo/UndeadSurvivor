using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class JoystickHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private Image _joystickBackground;
    [SerializeField] private Image _joystick;

    private Vector2 _joystickBackgroundStartPosition;

    protected Vector2 InputVector;
    

    private void Start()
    {
        _joystickBackgroundStartPosition = _joystickBackground.rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 joystickPosition;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_joystickBackground.rectTransform,
                eventData.position, null, out joystickPosition))
        {
            joystickPosition.x = (joystickPosition.x * 2 / _joystickBackground.rectTransform.sizeDelta.x);
            joystickPosition.y = (joystickPosition.y * 2 / _joystickBackground.rectTransform.sizeDelta.y);
            
            InputVector = new Vector2(joystickPosition.x, joystickPosition.y);
            
            InputVector = InputVector.magnitude > 1 ? InputVector.normalized : InputVector;
            
            _joystick.rectTransform.anchoredPosition = new Vector2(InputVector.x * (_joystickBackground.rectTransform.sizeDelta.x / 2), InputVector.y * (_joystickBackground.rectTransform.sizeDelta.y / 2));
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    { }

    public void OnPointerUp(PointerEventData eventData)
    {
        _joystickBackground.rectTransform.anchoredPosition = _joystickBackgroundStartPosition;
        
        InputVector = Vector2.zero;
        _joystick.rectTransform.anchoredPosition = Vector2.zero;
    }
}
