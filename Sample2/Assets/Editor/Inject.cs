using UnityEngine;
using UnityEditor;
using System.IO;
using HotFixInjector;

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

        InjectApp.Run();
    }
}
