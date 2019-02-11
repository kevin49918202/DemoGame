using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour {

    public Animator animator;
    Text damageText;
    Transform target;
    Vector3 randomPosition;
    Vector2 screenPoint;

    void OnEnable()
    {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, clipInfo[0].clip.length);

        damageText = animator.GetComponent<Text>();

        randomPosition = new Vector3(Random.Range(-10, 10), Random.Range(50, 70));
    }

    void Update()
    {
        if(target != null)
        {
            UpdatePosition();
        }
    }

    public void Set(string text, Transform transform)
    {
        damageText.text = text;
        target = transform;

        UpdatePosition();
    }

    void UpdatePosition()
    {
        screenPoint = Camera.main.WorldToScreenPoint(target.position) + randomPosition;
        transform.position = screenPoint;
    }
}
