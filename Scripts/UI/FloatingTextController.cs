using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextController : MonoBehaviour {

    public static FloatingTextController instance;

    FloatingText popupText_Player;
    FloatingText popupText_Enemy;
    Transform canvas;

    void Awake()
    {
        instance = this;
        canvas = GameObject.Find("Canvas").transform;

        popupText_Player = Resources.Load<FloatingText>("PopupTextParent_Player");
        popupText_Enemy = Resources.Load<FloatingText>("PopupTextParent_Enemy");
    }

    public void CreatFloatingText(string text, Transform transform, int type)
    {
        FloatingText instance = null;

        switch (type)
        {
            case 0:
                instance = popupText_Player;
                break;
            case 1:
                instance = popupText_Enemy;
                break;
        }

        FloatingText floatingText = Instantiate(instance);
        floatingText.transform.SetParent(canvas, false);
        floatingText.Set(text, transform);
    }
}
