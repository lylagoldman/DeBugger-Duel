using UnityEngine;
using UnityEngine.U2D.Animation;

[CreateAssetMenu(menuName = "Benny/Key Type")]
public class KeyType : ScriptableObject
{
    public SpriteLibraryAsset purple;
    public SpriteLibraryAsset red;
    public Vector2 colliderSize = new Vector2(0.6f, 0.5f);
    [Range(0f, 1f)] public float bounciness = 0.6f;
}