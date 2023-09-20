#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

namespace GitIntegration
{
  [InitializeOnLoad]
  public class SmartMergeRegistrator
  {
    private const string _smartMergeRegistratorEditorPrefsKey = "smart_merge_installed";
    private const int _version = 1;
    private static readonly string _versionKey = $"{_version}_{Application.unityVersion}";

    [MenuItem("Tools/Git/SmartMerge registration")]
    private static void SmartMergeRegister()
    {
      try
      {
        string unityYamlMergePath = EditorApplication.applicationContentsPath + "/Tools" + "/UnityYAMLMerge.exe";
        Utils.ExecuteGitWithParams("config merge.unityyamlmerge.name \"Unity SmartMerge (UnityYamlMerge)\"");
        Utils.ExecuteGitWithParams(
          $"config merge.unityyamlmerge.driver \"\\\"{unityYamlMergePath}\\\" merge -h -p --force --fallback none %O %B %A %A\"");
        Utils.ExecuteGitWithParams("config merge.unityyamlmerge.recursive binary");
        EditorPrefs.SetString(_smartMergeRegistratorEditorPrefsKey, _versionKey);
        Debug.Log($"Successfully registered UnityYAMLMerge with path {unityYamlMergePath}");
      }
      catch (Exception e)
      {
        Debug.Log($"Fail to register UnityYAMLMerge with error: {e}");
      }
    }

    [MenuItem("Tools/Git/SmartMerge unregistration")]
    private static void SmartMergeUnRegister()
    {
      Utils.ExecuteGitWithParams("config --remove-section merge.unityyamlmerge");
      Debug.Log($"Successfully unregistered UnityYAMLMerge");
    }

    //Unity calls the static constructor when the engine opens
    static SmartMergeRegistrator()
    {
      string installedVersionKey = EditorPrefs.GetString(_smartMergeRegistratorEditorPrefsKey);
      if (installedVersionKey != _versionKey)
        SmartMergeRegister();
    }
  }
}
#endif
