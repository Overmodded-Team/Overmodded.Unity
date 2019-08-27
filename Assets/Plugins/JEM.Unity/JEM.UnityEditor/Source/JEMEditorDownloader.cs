//
// Unity Editor Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.Core;
using JEM.Core.Debugging;
using JEM.Core.IO;
using JEM.UnityEngine;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace JEM.UnityEditor
{
    /// <summary>
    ///     JEM Editor downloader callback.
    /// </summary>
    public delegate void JEMEditorDownloaderCallback(Texture2D texture);

    /// <summary>
    ///     JEM Editor Downloader.
    /// </summary>
    public class JEMEditorDownloader
    {
        /// <summary>
        ///     URL of request.
        /// </summary>
        public string URL { get; private set; }

        /// <summary>
        ///     WWW access of request.
        /// </summary>
        public WWW www { get; private set; }

        /// <summary>
        ///     Current callback.
        /// </summary>
        public JEMEditorDownloaderCallback Callback { get; private set; }

        /// <summary>
        ///     Creates new request.
        /// </summary>
        public static void NewRequest(string url, JEMEditorDownloaderCallback callBack)
        {
            var local = LoadFromFile(url);
            if (local != null)
            {
                JEMLogger.InternalLog(
                    $"JEMEditor is getting texture from {url}. Local texture exist! WWW download process will be ignored.");
                callBack?.Invoke(local);
            }
            else
            {
                JEMLogger.InternalLog($"JEMEditor is getting texture from {url}");
                var d = new JEMEditorDownloader
                {
                    URL = url,
                    www = new WWW(url),
                    Callback = callBack
                };

                EditorApplication.update += d.Update;
            }
        }

        private void Update()
        {
            if (www.isDone)
            {
                JEMLogger.InternalLog($"Texture {URL} has been received.");
                EditorApplication.update -= Update;
                if (www.texture == null)
                    return;

                SaveFile(URL, www.texture);
                Callback?.Invoke(www.texture);
            }
        }

        /// <summary>
        ///     Saves given Texture2D to file.
        /// </summary>
        public static void SaveFile(string url, Texture2D texture2D)
        {
            var hash = JEMMD5.Hash(url);
            var file = ResolveFilePath(hash);
            var dir = Path.GetDirectoryName(file);
            if (dir != null && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            JEMTexture2D.WriteToFile(file, texture2D);
        }

        /// <summary>
        ///     Reads Texture2D from file.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Texture2D LoadFromFile(string url)
        {
            var hash = JEMMD5.Hash(url);
            var file = ResolveFilePath(hash);
            if (File.Exists(file))
                return JEMTexture2D.ReadFromFile(file);
            return null;
        }

        /// <summary>
        ///     Resolves path to file.
        /// </summary>
        public static string ResolveFilePath(string fileName)
        {
            return $"{Environment.CurrentDirectory}{LocalStoragePath}{fileName}{JEMTexture2D.JEMTextureFileExtension}";
        }

        /// <summary>
        ///     Path to storage of downloaded textures.
        /// </summary>
        public static string LocalStoragePath = $@"{JEMVar.DirectorySeparatorChar}JEM{JEMVar.DirectorySeparatorChar}Temp{JEMVar.DirectorySeparatorChar}";
    }
}