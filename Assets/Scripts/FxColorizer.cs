using UnityEngine;
using UnityEngine.U2D.Animation;

public class FxColorizer : MonoBehaviour
{
    [SerializeField] SpriteLibrary library;
    [SerializeField] SpriteResolver resolver;

    [Header("Leave a slot empty if this effect has no art for that color")]
    [SerializeField] SpriteLibraryAsset purple, green, red;

    public FxColor Color { get; private set; }

    SpriteLibraryAsset Get(FxColor c) => c switch {
        FxColor.Green => green, FxColor.Red => red, _ => purple };

    void FindComponents()
    {
        if (library == null)  library  = GetComponent<SpriteLibrary>();
        if (resolver == null) resolver = GetComponent<SpriteResolver>();
    }

    public void SetColor(FxColor c)
    {
        FindComponents();

        if (Get(c) == null)
        {
            c = green != null ? FxColor.Green : purple != null ? FxColor.Purple : FxColor.Red;
        }
        
        Color = c;
        if (library == null) { Debug.LogWarning($"{name}: no Sprite Library component"); return; }
        if (Get(c) == null)  { Debug.LogWarning($"{name}: no color libraries assigned"); return; }

        library.spriteLibraryAsset = Get(c);
        if (resolver != null) resolver.ResolveSpriteToSpriteRenderer();
    }
}