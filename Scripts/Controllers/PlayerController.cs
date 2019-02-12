using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GameObject focus;

    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                SetFocus(hit.collider.gameObject);
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            RemoveFocus();
        }
    }

    void SetFocus(GameObject newFocus)
    {
        Interactable newInteractable = newFocus.GetComponent<Interactable>();
        if (newInteractable != null)
        {
            
            if (newFocus != focus)
            {
                if (focus != null)
                {
                    Interactable oldInteractable = focus.GetComponent<Interactable>();
                    oldInteractable.OnDefocused();
                }
                focus = newFocus;
            }
            AvatarManager.instance.Target(newFocus);
            newInteractable.OnFocused(transform);
            newInteractable.Interact();
        }

    }

    void RemoveFocus()
    {
        if(focus != null)
        {
            Interactable oldInteractable = focus.GetComponent<Interactable>();
            oldInteractable.OnDefocused();
            AvatarManager.instance.UnTarget(focus);
        }
        focus = null;
    }

    //bool IsMouseOverUIWithIgnores()
    //{
    //    PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
    //    pointerEventData.position = Input.mousePosition;

    //    List<RaycastResult> raycastResultList = new List<RaycastResult>();
    //    EventSystem.current.RaycastAll(pointerEventData, raycastResultList);

    //    for (int i = 0; i < raycastResultList.Count; i++)
    //    {
    //        if (raycastResultList[i].gameObject.GetComponent<MouseUIClickThrough>() != null)
    //        {
    //            raycastResultList.RemoveAt(i);
    //            i--;
    //        }
    //    }
    //    return raycastResultList.Count > 0;
    //}
}
