using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] float playerDeathDelay = 1.375f;
    [SerializeField] float bennyDefeatDelay = 1.5f;

    bool resetting;

    // Hook to PlayerHealth → On Game Over
    public void PlayerDied()
    {
        if (resetting) return;
        StartCoroutine(ResetAfter(playerDeathDelay));
    }

    // Hook to BennyHealth → On Defeated
    public void BennyDefeated()
    {
        if (resetting) return;
        StartCoroutine(ResetAfter(bennyDefeatDelay));
    }

    // Can also be called from a "Retry" button
    public void ResetGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator ResetAfter(float delay)
    {
        resetting = true;
        yield return new WaitForSecondsRealtime(delay);
        ResetGame();
    }
}