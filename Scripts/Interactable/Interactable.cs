using UnityEngine;

public class Interactable : MonoBehaviour {

    public float radius = 3f;

    public Transform interactTransform;

    Transform player;

    public virtual void Interact(){
        
    }

    public virtual void OnFocused(Transform playerTransform)
    {
        player = playerTransform;
    }

    public virtual void OnDefocused()
    {
        player = null;
    }

    public bool Close()
    {
        return (player.position - transform.position).magnitude < radius;
    }

    void OnDrawGizmosSelected()
    {
        if (interactTransform == null)
            interactTransform = transform; 
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
