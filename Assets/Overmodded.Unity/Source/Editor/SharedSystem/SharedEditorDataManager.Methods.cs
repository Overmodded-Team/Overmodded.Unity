//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using System.Collections.Generic;

namespace Overmodded.Unity.Editor.SharedSystem
{
    /// <summary/>
    public static partial class SharedEditorDataManager
    {
        /// <summary>
        ///     Gets list of level spawn names.
        /// </summary>
        public static List<Tuple<string, string>> GetLevelSpawnNames()
        {
            var d = new List<Tuple<string, string>>();
            foreach (var editor in LoadedEditorsData)
            {
                foreach (var levelName in editor.LevelSpawnNames)
                    d.Add(new Tuple<string, string>(editor.UniqueGUID, levelName));
            }

            return d;
        }

        /// <summary>
        ///     Gets name of Shared Editor Data by GUID.
        /// </summary>
        public static string GUIDToEditorDataName(string guid)
        {
            string name = "Name N/A";
            for (var index = 0; index < LoadedEditorsData.Count; index++)
            {
                var e = LoadedEditorsData[index];
                if (e.UniqueGUID == guid)
                {
                    name = e.name;
                    break;
                }
            }

            return name;
        }
    }
}
