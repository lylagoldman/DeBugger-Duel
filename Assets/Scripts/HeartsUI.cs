using UnityEngine;
using UnityEngine.UI;

public class HeartsUI : MonoBehaviour
{
    [SerializeField] Image[] hearts;
    [SerializeField] Sprite full, empty;

    public void SetLives(int lives)
    {
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].sprite = i < lives ? full : empty;
    }
}