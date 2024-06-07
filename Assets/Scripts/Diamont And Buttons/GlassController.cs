using UnityEngine;

public class GlassController : MonoBehaviour
{
    public static GlassController Instance;

    [SerializeField] private ButtonController[] buttons;
    [SerializeField] private GameObject glassObject;
    [SerializeField] private GameObject teleporter;

    private bool allButtonsActive = false;

    private void Awake()
    {
        Instance = this;
    }

    public void CheckButtons()
    {
        if (allButtonsActive)
            return;

        allButtonsActive = true;
        foreach (ButtonController button in buttons)
        {
            if (!button.IsActive)
            {
                allButtonsActive = false;
                break;
            }
        }

        if (allButtonsActive)
        {
            Destroy(glassObject);
            teleporter.SetActive(true);
        }
    }
}