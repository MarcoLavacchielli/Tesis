using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingLevels : MonoBehaviour
{
    //public Slider loadingBar;
    public string sceneToGo;

    private void Start()
    {
        StartCoroutine(LoadGame(sceneToGo));
    }

    private IEnumerator LoadGame(string nombreEscena)
    {
        AsyncOperation cargaOperacion = SceneManager.LoadSceneAsync(nombreEscena);

        while (!cargaOperacion.isDone)
        {
            float progreso = Mathf.Clamp01(cargaOperacion.progress / 0.9f);
            //loadingBar.value = progreso;
            yield return null;
        }
        SceneManager.LoadScene(nombreEscena);
    }
}
