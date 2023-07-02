using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolSelector : MonoBehaviour
{
    [SerializeField] private HorizontalLayoutGroup _hotbar;
    [SerializeField] private Sprite _slotActive;
    [SerializeField] private Sprite _slotInactive;

    private ITool _activeTool;
    private Image _activeImage;
    private Dictionary<KeyCode, ITool> _keyCodeToolPairs;
    private Dictionary<KeyCode, Image> _keyCodeImagePairs;

    private void Start()
    {
        List<Image> hotbarImages = new();

        foreach (Transform child in _hotbar.transform)
        {
            if (child.TryGetComponent(out Image image))
            {
                hotbarImages.Add(image);
            }
        }

        _keyCodeImagePairs = new Dictionary<KeyCode, Image>();
        _keyCodeToolPairs = new Dictionary<KeyCode, ITool>();
        ITool[] tools = GetComponentsInChildren<ITool>(true);
        int maxI = Mathf.Min(tools.Length, 9);

        for (int i = 0; i < maxI; i++)
        {
            var key = (KeyCode)(i + 49);
            _activeTool = tools[i];
            _activeImage = hotbarImages[i];
            _keyCodeToolPairs[key] = _activeTool;
            _keyCodeImagePairs[key] = _activeImage;
        }

        EnableTool(KeyCode.Alpha1);

        for (int i = 1; i < tools.Length; i++)
        {
            tools[i].Deactivate();
        }
    }

    private void Update()
    {
        if (!Input.anyKeyDown) return;

        foreach (var hotbarKey in _keyCodeToolPairs.Keys)
        {
            bool isKeyRelevant = Input.GetKeyDown(hotbarKey);
            if (isKeyRelevant)
            {
                EnableTool(hotbarKey);
                break;
            }
        }
    }

    private void EnableTool(KeyCode keyCode)
    {
        Image image = _keyCodeImagePairs[keyCode];
        ITool tool = _keyCodeToolPairs[keyCode];

        if (tool == _activeTool) return;

        _activeTool.Deactivate();
        _activeTool = tool;
        _activeTool.Activate();

        _activeImage.sprite = _slotInactive;
        _activeImage = image;
        _activeImage.sprite = _slotActive;
    }
}