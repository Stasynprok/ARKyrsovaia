using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Vector2Extensions
{
    public static bool IsPointOverUIObject(this Vector2 pos)
    {
        /*bool uiIsTouch = false;
        foreach (Touch touch in Input.touches)
        {
            int id = touch.fingerId;
            if (EventSystem.current.IsPointerOverGameObject(id))
            {
                uiIsTouch = true;
                return uiIsTouch;
            }
        }
        return uiIsTouch;*/

        if (EventSystem.current.IsPointerOverGameObject())
        {
        return false;
        }

        PointerEventData eventPosition = new PointerEventData(EventSystem.current);
        eventPosition.position = new Vector2(pos.x, pos.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventPosition, results);

        return results.Count > 0;
    }
}
