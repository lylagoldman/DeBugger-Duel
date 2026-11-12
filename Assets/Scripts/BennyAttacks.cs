using System.Collections.Generic;
using UnityEngine;

public class BennyAttacks : MonoBehaviour
{
    [SerializeField] GameObject keyPrefab, binaryPrefab, laserPrefab, rainArrowPrefab;
    [SerializeField] KeyType[] keyTypes;  

    [SerializeField] float rainSpawnY = 3.5f;
    [SerializeField] int rainSpawnCount = 15;
    [SerializeField] float rainMinX = -12f;
    [SerializeField] float rainMaxX = 12f;

    [SerializeField, Range(0f, 1f)] float redChance = 0.1f;

    [SerializeField] Vector2 pos;


    FxColor current;
    public void BeginAttack() => current = AttackColor.Roll(redChance);

   public void SpawnKey()
    {
        var go = Instantiate(keyPrefab, pos, Quaternion.identity);
        var key = go.GetComponent<KeyScript>();
        key.Setup(keyTypes[Random.Range(0, keyTypes.Length)], current);
        key.Launch(Random.Range(5f, 9f), Random.Range(3f, 7f));
    }

    public void SpawnRainArrow()
    {
        float spacing = (rainMaxX - rainMinX) / (rainSpawnCount - 1);
        int slot = Random.Range(0, rainSpawnCount);
        float x = rainMinX + slot * spacing;

        GameObject arrow = Instantiate(rainArrowPrefab, new Vector3(x, rainSpawnY, 0f), Quaternion.identity);
        arrow.GetComponent<RainScript>().SetColor(current);
    }

    public void SpawnRainWave(int arrowCount)
    {
        float spacing = (rainMaxX - rainMinX) / (rainSpawnCount - 1);
        var slots = new List<int>();
        for (int i = 0; i < rainSpawnCount; i++) slots.Add(i);

        for (int n = 0; n < arrowCount && slots.Count > 0; n++)
        {
            int pick = Random.Range(0, slots.Count);
            float x = rainMinX + slots[pick] * spacing;
            slots.RemoveAt(pick);

            GameObject arrow = Instantiate(rainArrowPrefab, new Vector3(x, rainSpawnY, 0f), Quaternion.identity);
            arrow.GetComponent<RainScript>().SetColor(current);
        }
    }

    public void SpawnLaser()
    {
        
    }
}