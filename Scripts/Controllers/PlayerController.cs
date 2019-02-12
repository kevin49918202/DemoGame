using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Interactable focus;

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
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    CharacterStats characterStats = hit.collider.GetComponent<CharacterStats>();    
                    SetFocus(interactable, characterStats);
                    focus.Interact();
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            RemoveFocus();
        }
    }

    void SetFocus(Interactable newFocus, CharacterStats newFocusStats)
    {
        if(newFocus != focus)
        {
            if (focus != null)
                focus.OnDefocused();
            focus = newFocus;
        }
        TargetAvatar.instance.Target(newFocusStats);
        newFocus.OnFocused(transform);
    }

    void RemoveFocus()
    {
        if(focus != null)
        {
            focus.OnDefocused();
            TargetAvatar.instance.UnTarget();
        }
            
        focus = null;
    }

    bool IsMouseOverUIWithIgnores()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResultList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResultList);

        for (int i = 0; i < raycastResultList.Count; i++)
        {
            if (raycastResultList[i].gameObject.GetComponent<MouseUIClickThrough>() != null)
            {
                raycastResultList.RemoveAt(i);
                i--;
            }
        }
        return raycastResultList.Count > 0;
    }
}
