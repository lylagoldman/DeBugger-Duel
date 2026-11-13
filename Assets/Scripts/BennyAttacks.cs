using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BennyAttacks : MonoBehaviour
{
    [SerializeField] GameObject keyPrefab, binaryPrefab, laserPrefab, rainArrowPrefab;

    [SerializeField] float rainSpawnY = 3.5f;
    [SerializeField] int rainSpawnCount = 15;
    [SerializeField] float rainMinX = -12f;
    [SerializeField] float rainMaxX = 12f;

    [SerializeField, Range(0f, 1f)] float redChance = 0.1f;

    [Header("Laser")]
    [SerializeField] Vector2 laserSpawnPos = new Vector2(0f, 0f);
    [SerializeField] float laserSustainTime = 1.2f;
    GameObject activeLaser;

    [Header("Keys")]
    [SerializeField] Transform keySpawnPoint;
    [SerializeField] int keysPerVolley = 2;
    [SerializeField] float timeBetweenKeys = 0.5f;
    [SerializeField] KeyType[] keyTypes;

    bool spawning;
    public bool IsBusy => spawning || AttackObject.ActiveCount > 0;

    FxColor current;
    public void BeginAttack() => current = AttackColor.Roll(redChance);


    public void SpawnKeyVolley()
    {
        if (!spawning) StartCoroutine(KeyVolley());
    }

    IEnumerator KeyVolley()
    {
        spawning = true;
        for (int i = 0; i < keysPerVolley; i++)
        {
            SpawnKey();
            yield return new WaitForSeconds(timeBetweenKeys);
        }
        spawning = false;
    }

    public void SpawnKey()
    {
        var go = Instantiate(keyPrefab, keySpawnPoint.position, Quaternion.identity);
        var key = go.GetComponent<KeyScript>();
        key.Setup(keyTypes[Random.Range(0, keyTypes.Length)], Random.value < 0.1f ? FxColor.Red : FxColor.Purple);
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
        if (activeLaser != null) return;   // never two beams at once

        activeLaser = Instantiate(laserPrefab, laserSpawnPos, Quaternion.identity);
        activeLaser.GetComponent<FxColorizer>().SetColor(current);
        activeLaser.GetComponent<LaserScript>().Init(laserSustainTime);
    }
}