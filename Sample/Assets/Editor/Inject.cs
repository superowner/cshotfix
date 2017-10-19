using UnityEngine;
using UnityEditor;
using System.IO;

public static class Inject
{

    [MenuItem("Tool/Inject")]
	public static void EditorTest()
	{
        if (EditorApplication.isCompiling || Application.isPlaying)
        {
            Debug.Log("请等待编辑器结束编译或者停止播放");
            return;
        }

        System.Diagnostics.Process process = new System.Diagnostics.Process();
        process.StartInfo.FileName = Path.GetFullPath("../Inject.bat");
        process.StartInfo.UseShellExecute = true;
        process.Start();
        process.WaitForExit();
        Debug.Log("Inject Over");

    }
}
