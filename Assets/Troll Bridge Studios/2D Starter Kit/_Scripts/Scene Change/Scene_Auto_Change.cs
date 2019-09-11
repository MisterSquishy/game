using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Auto_Change : MonoBehaviour
{
    public float delay = 3;
    public string NewLevel = "Field";
    void Start()
    {
        StartCoroutine(LoadLevelAfterDelay(delay));
    }

    IEnumerator LoadLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(NewLevel);
    }
}