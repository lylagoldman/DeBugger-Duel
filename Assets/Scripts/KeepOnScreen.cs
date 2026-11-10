using UnityEngine;

public class KeepOnScreen : MonoBehaviour
{
    private Vector2 screenBounds;
    
    [Header("Adjust bounds manually")]
    [Tooltip("Lower values let the player get closer to the edge. Higher values push them away.")]
    public float edgeOffset = 0.5f; 

    void Start()
    {
        // Calculate the screen boundaries in world coordinates
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    void LateUpdate()
    {
        Vector3 viewPos = transform.position;

        // Clamp using your custom manual offset instead of the sprite bounds
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 + edgeOffset, screenBounds.x - edgeOffset);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1 + edgeOffset, screenBounds.y - edgeOffset);

        transform.position = viewPos;
    }
}
