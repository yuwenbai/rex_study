using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class ExportWindow : EditorWindow
{
    [UnityEditor.MenuItem("Export/ExportWindow")]
    static void Init()
    {
        ExportWindow window = (ExportWindow)EditorWindow.GetWindow(typeof(ExportWindow));
        window.minSize = new Vector2(400, 430);
        window.maxSize = new Vector2(400, 430);
        window.name = "ExportWindow";
        window.Show();
    }
}
