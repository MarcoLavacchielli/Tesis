using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlassController : MonoBehaviour
{
    public static GlassController Instance;

    [SerializeField] private ButtonController[] buttons;
    [SerializeField] private GameObject glassObject;
    [SerializeField] private TextMeshProUGUI counterText;

    public bool IsGlassDestroyed { get; private set; } = false;

    private bool allButtonsActive = false;
    private int activeButtonCount = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateCounterText();
    }

    public void CheckButtons()
    {
        if (allButtonsActive)
            return;

        allButtonsActive = true;
        activeButtonCount = 0;

        foreach (ButtonController button in buttons)
        {
            if (button.IsActive)
            {
                activeButtonCount++;
            }
            else
            {
                allButtonsActive = false;
            }
        }

        UpdateCounterText();

        if (allButtonsActive)
        {
            Destroy(glassObject);
            IsGlassDestroyed = true;
        }
    }

    private void UpdateCounterText()
    {
        counterText.text = $"Botones: {activeButtonCount}/{buttons.Length}";
    }
}