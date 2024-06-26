using UnityEngine;

public class GlassController : MonoBehaviour
{
    public static GlassController Instance;

    [SerializeField] private ButtonController[] buttons;
    [SerializeField] private GameObject glassObject;

    public bool IsGlassDestroyed { get; private set; } = false;

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
            IsGlassDestroyed = true;
        }
    }
}