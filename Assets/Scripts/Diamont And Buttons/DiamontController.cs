using UnityEngine;

public class DiamontController : MonoBehaviour
{
    public static DiamontController Instance;

    [SerializeField] private ButtonController[] buttons;

    private Renderer rend;
    private bool allButtonsActive = false;

    private void Awake()
    {
        Instance = this;
        rend = GetComponent<Renderer>();
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
            rend.material.color = Color.green;
        }
    }
}