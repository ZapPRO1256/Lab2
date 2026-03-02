using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Toggles instruction panel on button click.
/// </summary>
public class Hint : MonoBehaviour
{
    [SerializeField] GameObject instructionPanel;
    [SerializeField] Button button;

    bool _isVisible;

    void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (button != null)
            button.onClick.AddListener(Toggle);

        if (instructionPanel != null)
            instructionPanel.SetActive(false);
    }

    void Toggle()
    {
        if (instructionPanel == null)
            return;

        _isVisible = !_isVisible;
        instructionPanel.SetActive(_isVisible);
    }
}