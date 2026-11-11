using UnityEditor.Animations;
using UnityEngine;

public class RainScript : MonoBehaviour
{
    [SerializeField] GameObject rainArrow;
    [SerializeField] GameObject rain;
    private AnimatorController rainAnimator;
    private Transform arrowTransform;
    private Vector3 rainPosition;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        arrowTransform = rainArrow.GetComponent<Transform>();
        rainPosition = rain.GetComponent<Transform>().position;
        rainAnimator = rain.GetComponent<AnimatorController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnRain()
    {
        float xSpawnPosition = arrowTransform.position.x;
        rainPosition.x = xSpawnPosition;
        PickRainType();
        Instantiate(rain, rainPosition, arrowTransform.rotation);
    }

    public void PickRainType()
    {
        int randomNum = Random.Range(0,3);

        if (randomNum == 0)
        {
            rainAnimator.SetTrigger("0bit");
        } else if (randomNum == 1)
        {
            rainAnimator.SetTrigger("1bit");
        } else
        {
            rainAnimator.SetTrigger("column");
        }
    }
}
