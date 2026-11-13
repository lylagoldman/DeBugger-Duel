using UnityEngine;
using UnityEngine.UI;

public class FireballUI : MonoBehaviour
{
    [SerializeField] Image[] fills;

    void Update()
    {
        var m = SpecialMeter.Instance;
        if (m == null) return;

        for (int i = 0; i < fills.Length; i++)
        {
            if (i < m.Charges){
                fills[i].fillAmount = 1f;
            }
            else if (i == m.Charges) {
                fills[i].fillAmount = m.NextChargeProgress;
            }
            else
            {
                fills[i].fillAmount = 0f;
            }
        }
    }
}