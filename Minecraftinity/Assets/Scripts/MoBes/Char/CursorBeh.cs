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
}