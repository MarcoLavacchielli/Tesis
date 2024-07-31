using TMPro;
using UnityEngine;

public class GlassController : MonoBehaviour
{
    public static GlassController Instance;

    [SerializeField] private ButtonController[] buttons;
    [SerializeField] private GameObject glassObject;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private TextMeshProUGUI objectiveText;

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
        UpdateObjectiveText("Objective: Disable security");
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
            UpdateObjectiveText("Objective: Take the Diamond");
        }
    }

    private void UpdateCounterText()
    {
        buttonText.text = $"Buttons: {activeButtonCount}/{buttons.Length}";
    }

    public void UpdateObjectiveText(string newObjective)
    {
        objectiveText.text = newObjective;
    }

    public void DestroyButtonText()
    {
        if (buttonText != null)
        {
            Destroy(buttonText.gameObject);
        }
    }
}