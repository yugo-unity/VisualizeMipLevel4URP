using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RendererShaderChange : EditorWindow
{
    [MenuItem("Tools/RendererShaderChange")]
    public static void Create()
    {
        RendererShaderChange.CreateWindow<RendererShaderChange>();
    }

    private void OnGUI()
    {
        if (!EditorApplication.isPlaying)
        {
            EditorGUILayout.LabelField("再生中のみ有効です");
            return;
        }
        if (GUILayout.Button("Execute"))
        {
            this.Execute();
        }
    }
    private void Execute() {
        var shader = Shader.Find("Unlit/MipmapShader");
        var renderers = Resources.FindObjectsOfTypeAll<Renderer>();
        foreach(var r in renderers)
        {
            if(PrefabUtility.IsPartOfPrefabAsset(r)){
                continue;
            }
            var materials = r.materials;
            if(materials == null) { continue; }
            foreach(var material in materials)
            {
                material.shader = shader;
            }
        }
    }
}
