using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    [Header("Loading UI")]
    public GameObject loadingPanel;
    public Image loadingCircle;

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }
    void Update()
{
    if (loadingPanel.activeSelf)
    {
        loadingCircle.transform.Rotate(0, 0, -200 * Time.deltaTime);
    }
}

    IEnumerator LoadSceneAsync(int sceneIndex)
    {
        loadingPanel.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            loadingCircle.fillAmount = progress;

            if (operation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.5f); // Optional
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}