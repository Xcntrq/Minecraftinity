using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class CursorBeh : MonoBehaviour
{
#if UNITY_EDITOR
    public static void ForceMousePositionToCenterOfGameWindow()
    {
        // Force the mouse to be in the middle of the game screen
        var game = UnityEditor.EditorWindow.GetWindow(typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.GameView"));
        Vector2 warpPosition = game.rootVisualElement.contentRect.center;  // never let it move
        Mouse.current.WarpCursorPosition(warpPosition);
        InputState.Change(Mouse.current.position, warpPosition);
    }

    private void LateUpdate()
    {
        ForceMousePositionToCenterOfGameWindow();
    }
#endif

#if UNITY_STANDALONE_WIN
    [DllImport("user32.dll")]
    static extern bool ClipCursor(ref RECT lpRect);

    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
#endif

    public void Start()
    {
        RECT cursorLimits;
        cursorLimits.Left = 0;
        cursorLimits.Top = 0;
        cursorLimits.Right = Screen.width - 1;
        cursorLimits.Bottom = Screen.height - 1;
        ClipCursor(ref cursorLimits);
    }
}