using UnityEngine;

public class RainScript : MonoBehaviour
{
    [SerializeField] GameObject rain;

    private Transform arrowTransform;
    private Vector3 rainPosition;
    private FxColor color;
    private bool colorAssigned;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        arrowTransform = transform;
        rainPosition = rain.GetComponent<Transform>().position;
        if (!colorAssigned) SetColor(AttackColor.Roll());
    }
    
   public void SetColor(FxColor c)
    {
        color = c;
        colorAssigned = true;
        var arrowColorizer = GetComponent<FxColorizer>();
        if (arrowColorizer != null) arrowColorizer.SetColor(c);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Animation Event on the arrow
    public void SpawnRain()
    {
        rainPosition.x = arrowTransform.position.x;
        GameObject spawned = Instantiate(rain, rainPosition, arrowTransform.rotation);

        spawned.GetComponent<FxColorizer>().SetColor(color);
        PickRainType(spawned.GetComponent<Animator>());
        Destroy(gameObject);
    }

    void PickRainType(Animator anim)
    {
        switch (Random.Range(0, 3))
        {
            case 0: anim.SetTrigger("0bit"); break;
            case 1: anim.SetTrigger("1bit"); break;
            default: anim.SetTrigger("column"); break;
        }
    }
}
