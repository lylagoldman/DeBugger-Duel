using UnityEngine;
using UnityEngine.U2D.Animation;

public class FxColorizer : MonoBehaviour
{
    [SerializeField] SpriteLibrary library;
    [SerializeField] SpriteResolver resolver;
    [SerializeField] SpriteLibraryAsset purple, green, red;
    [SerializeField] FxColor fallback = FxColor.Purple;

    public FxColor Color { get; private set; }
    public bool Parryable => Color == FxColor.Red;

    SpriteLibraryAsset Get(FxColor c) => c switch {
        FxColor.Green => green, FxColor.Red => red, _ => purple };

    public void SetColor(FxColor c)
    {
        if (Get(c) == null) c = fallback;
        Color = c;
        library.spriteLibraryAsset = Get(c);
        resolver.ResolveSpriteToSpriteRenderer();
        gameObject.tag = Parryable ? "Parryable" : "Hazard";
    }

    public void EndOfLife()
    {
        Destroy(gameObject);
    }
}