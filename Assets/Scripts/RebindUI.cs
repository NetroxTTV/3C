using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class RebindUI : MonoBehaviour
{
    public InputActionReference actionReference;
    public bool rebindCompositeSequence = false;
    public CompositePart targetDirection = CompositePart.None;

    [Header("UI Elements")]
    public TMP_Text bindingNameText;
    public Button rebindButton;
    public GameObject waitingForInputOverlay;
    public TMP_Text overlayStatusText;

    private InputActionRebindingExtensions.RebindingOperation rebindOperation;
    private int bindingIndex = -1;

    private readonly List<CompositePart> sequenceOrder = new List<CompositePart>
    {
        CompositePart.Up,
        CompositePart.Left,
        CompositePart.Down,
        CompositePart.Right
    };

    private int currentSequenceIndex = 0;

    public enum CompositePart { None, Up, Down, Left, Right }

    private const string SaveKey = "rebinds";

    private void Start()
    {
        string rebinds = PlayerPrefs.GetString(SaveKey);
        if (!string.IsNullOrEmpty(rebinds))
        {
            actionReference.action.LoadBindingOverridesFromJson(rebinds);
        }

        if (!rebindCompositeSequence)
        {
            bindingIndex = FindBindingIndex(targetDirection);
            UpdateUI(bindingIndex);
        }
        else
        {
            UpdateUI(-1);
        }
    }

    private int FindBindingIndex(CompositePart direction)
    {
        if (actionReference == null) return -1;

        InputAction action = actionReference.action;

        for (int i = 0; i < action.bindings.Count; i++)
        {
            InputBinding binding = action.bindings[i];

            if (direction != CompositePart.None)
            {
                if (binding.isPartOfComposite && string.Equals(binding.name, direction.ToString(), System.StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }
            else
            {
                if (!binding.isComposite) return i;
            }
        }

        return -1;
    }

    public void StartRebinding()
    {
        rebindButton.interactable = false;
        if (waitingForInputOverlay) waitingForInputOverlay.SetActive(true);

        actionReference.action.Disable();

        if (rebindCompositeSequence)
        {
            currentSequenceIndex = 0;
            StartSequenceStep();
        }
        else
        {
            if (bindingIndex != -1)
            {
                UpdateOverlayText($"Press {targetDirection}...");
                PerformRebind(bindingIndex);
            }
            else
            {
                Debug.LogError("Invalid binding index.");
                EndRebinding();
            }
        }
    }

    private void StartSequenceStep()
    {
        if (currentSequenceIndex >= sequenceOrder.Count)
        {
            EndRebinding();
            return;
        }

        CompositePart currentPart = sequenceOrder[currentSequenceIndex];
        int index = FindBindingIndex(currentPart);

        if (index != -1)
        {
            UpdateOverlayText($"Press {currentPart}...");

            rebindOperation = actionReference.action.PerformInteractiveRebinding(index)
                .WithControlsExcluding("Mouse")
                .OnMatchWaitForAnother(0.1f)
                .OnComplete(op =>
                {
                    op.Dispose();
                    currentSequenceIndex++;
                    StartSequenceStep();
                })
                .OnCancel(op =>
                {
                    op.Dispose();
                    EndRebinding();
                })
                .Start();
        }
        else
        {
            currentSequenceIndex++;
            StartSequenceStep();
        }
    }

    private void PerformRebind(int index)
    {
        rebindOperation = actionReference.action.PerformInteractiveRebinding(index)
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(op =>
            {
                op.Dispose();
                EndRebinding();
            })
            .OnCancel(op =>
            {
                op.Dispose();
                EndRebinding();
            })
            .Start();
    }

    private void EndRebinding()
    {
        actionReference.action.Enable();

        if (waitingForInputOverlay) waitingForInputOverlay.SetActive(false);
        rebindButton.interactable = true;

        string rebinds = actionReference.action.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(SaveKey, rebinds);
        PlayerPrefs.Save();

        if (!rebindCompositeSequence)
        {
            UpdateUI(bindingIndex);
        }
        else
        {
            UpdateUI(-1);
        }

        Debug.Log("Rebind Complete and Saved.");
    }

    private void UpdateUI(int index)
    {
        if (actionReference == null || bindingNameText == null) return;

        if (rebindCompositeSequence)
        {
            List<string> keys = new List<string>();
            foreach (var part in sequenceOrder)
            {
                int partIndex = FindBindingIndex(part);
                if (partIndex != -1)
                {
                    keys.Add(actionReference.action.GetBindingDisplayString(partIndex));
                }
            }
            bindingNameText.text = string.Join(" / ", keys);
        }
        else
        {
            if (index != -1)
                bindingNameText.text = actionReference.action.GetBindingDisplayString(index);
        }
    }

    private void UpdateOverlayText(string message)
    {
        if (bindingNameText != null) bindingNameText.text = message;

        if (overlayStatusText != null) overlayStatusText.text = message;
        else if (waitingForInputOverlay != null)
        {
            var text = waitingForInputOverlay.GetComponentInChildren<TMP_Text>();
            if (text) text.text = message;
        }
    }
}