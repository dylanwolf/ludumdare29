using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class CameraLimiterBox : MonoBehaviour
{
    [System.NonSerialized]
    public Rect Bounds;

    public void Awake()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
		Vector3 topLeft = transform.position;
		topLeft.x -= ((0.5f * boxCollider.size.x) - boxCollider.center.x) * boxCollider.transform.lossyScale.x;
		topLeft.y -= ((0.5f * boxCollider.size.y) - boxCollider.center.y) * boxCollider.transform.lossyScale.y;

        Bounds = new Rect(
				topLeft.x,
				topLeft.y,
                boxCollider.transform.lossyScale.x * boxCollider.size.x,
				boxCollider.transform.lossyScale.y * boxCollider.size.y
            );
		boxCollider.enabled = false;
    }
}
