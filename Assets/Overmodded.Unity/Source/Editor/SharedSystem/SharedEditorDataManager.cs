//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.UnityEditor;
using System.Collections.Generic;
using UnityEditor;

namespace Overmodded.Unity.Editor.SharedSystem
{
    /// <summary/>
    public static partial class SharedEditorDataManager
    {
        /// <summary>
        ///     Try to refresh database at start of GUI life.
        ///     This will only invoke once.
        /// </summary>
        public static void TryRefreshDatabaseAtStart()
        {
            if (_isFirstRefresh)
                return;

            _isFirstRefresh = true;

            RefreshDatabases();
        }

        /// <summary>
        ///     Refresh SharedEditorData assets.
        /// </summary>
        [MenuItem("Overmodded/Refresh Databases")]
        public static void RefreshDatabases()
        {
            var allAssets = AssetDatabase.FindAssets($"t:{nameof(SharedEditorData)}");
            LoadedEditorsData.Clear();

            foreach (var guid in allAssets)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = (SharedEditorData) AssetDatabase.LoadAssetAtPath(path, typeof(SharedEditorData));
                if (asset == null) continue;
                LoadedEditorsData.Add(asset);
            }
        }

        /// <summary>
        ///     Gets list of all SharedEditorData assets without local one.
        /// </summary>
        public static IList<SharedEditorData> GetListOfExternalEditorData()
        {
            var localData = GetLocalSharedData();
            if (localData == null) return LoadedEditorsData;

            var list = new List<SharedEditorData>(LoadedEditorsData);
            if (list.Contains(localData))
            {
                list.Remove(localData);
            }

            return list;
        }

        /// <summary>
        ///     Gets local shared data.
        ///     Can return null.
        /// </summary>
        public static SharedEditorData GetLocalSharedData() => new SavedObject<SharedEditorData>(SharedConfigWindow.LocalSharedDataName, null).value;

        /// <summary>
        ///     List of loaded all editors data.
        /// </summary>
        public static List<SharedEditorData> LoadedEditorsData { get; } = new List<SharedEditorData>();

        private static bool _isFirstRefresh;
    }
}
