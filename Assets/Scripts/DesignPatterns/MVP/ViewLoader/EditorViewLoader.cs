#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;

namespace DesignPatterns.MVP.ViewLoader
{
    public sealed class EditorViewLoader : IViewLoader
    {
        public void LoadView<T>(Action<T> action) where T : View
        {
            T view = null;
            string path = ViewPathAttribute.GetPath<T>();
            if (string.IsNullOrEmpty(path))
            {
                OnLoadError("View path is null or empty");
            }
            else
            {
                string fullPath = System.IO.Path.Combine("Assets", path);
                if (!fullPath.EndsWith(".preafb"))
                {
                    fullPath += ".prefab";
                }

                string guid = AssetDatabase.AssetPathToGUID(fullPath);
                if (string.IsNullOrEmpty(guid))
                {
                    OnLoadError($"View path not found in asset path, path: {path}");
                }
                else
                {
                    T asset = AssetDatabase.LoadAssetAtPath<T>(fullPath);
                    if (asset == null)
                    {
                        OnLoadError($"Asset is null object, path: {path}");
                    }
                    else
                    {
                        view = UnityEngine.Object.Instantiate(asset);
                        if (view == null)
                        {
                            OnLoadError("Instantiate error, view is null object");
                        }
                        else
                        {
                            view.gameObject.SetActive(false); // invisible first
                        }
                    }
                }
            }

            action(view);
        }

        private void OnLoadError(string message)
        {
            Debug.LogError($"[{nameof(EditorViewLoader)}] {message}");
        }
    }
}

#endif