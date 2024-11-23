using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour
{

    [SerializeField] private GameObject[] panels;

    private readonly Stack<int> order = new Stack<int>();

    public AudioManager audioManager;

    private void Start()
    {
        SwitchPanel(0);
    }

    public void SwitchPanel(int index)
    {

        audioManager.PlaySfx(0);

        if (order.Count > 0)
        {
            int previousIndex = order.Peek();
            panels[previousIndex].SetActive(false);
        }

        panels[index].SetActive(true);
        order.Push(index);
    }

    public void GoBack()
    {
        audioManager.PlaySfx(0);

        if (order.Count > 1)
        {
            int currentIndex = order.Pop();
            panels[currentIndex].SetActive(false);

            int previousIndex = order.Peek();
            panels[previousIndex].SetActive(true);
        }
    }

    public void LoadLevel(string level)
    {

        audioManager.PlaySfx(0);

        SceneManager.LoadScene(level);
    }

    public void QuitGame()
    {

        audioManager.PlaySfx(0);

        Debug.Log("salio");
        Application.Quit();
    }

    public void PlaySonido()
    {
        audioManager.PlaySfx(0);
    }
}
