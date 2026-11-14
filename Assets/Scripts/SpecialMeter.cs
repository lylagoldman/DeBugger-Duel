using UnityEngine;

public class SpecialMeter : MonoBehaviour
{
    public static SpecialMeter Instance { get; private set; }

    [SerializeField] int maxCharges = 3;
    [SerializeField] float pointsPerCharge = 100f;
    [SerializeField] float pointsPerBulletHit = 4f;
    float points;

    public int MaxCharges => maxCharges;
    public int Charges => Mathf.FloorToInt(points / pointsPerCharge);
    public bool HasCharge => Charges > 0;
    public float NextChargeProgress => Charges >= maxCharges ? 1f : points % pointsPerCharge / pointsPerCharge;

    void Awake() => Instance = this;
    public void AddHit() => AddPoints(pointsPerBulletHit);

    void AddPoints(float amount)
    {
        points = Mathf.Min(points + amount, maxCharges * pointsPerCharge);
    }

    public bool TrySpend()
    {
        if (!HasCharge) return false;
        points -= pointsPerCharge;
        return true;
    }
}