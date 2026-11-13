using UnityEngine;

public class AttackObject : MonoBehaviour
{
    public static int ActiveCount { get; private set; }

    void OnEnable()  => ActiveCount++;
    void OnDisable() => ActiveCount--;
}