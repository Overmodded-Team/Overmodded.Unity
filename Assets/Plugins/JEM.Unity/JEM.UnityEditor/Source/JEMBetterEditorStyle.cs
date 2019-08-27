//
// Unity Editor Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

namespace JEM.UnityEditor
{
    public enum JEMBetterEditorHeaderType
    {
        HeaderGroup,
        Classic,
        None
    }

    /// <summary>
    ///     Custom style for better editor components
    /// </summary>
    public struct JEMBetterEditorStyle
    {
        /// <summary>
        ///     Header name.
        /// </summary>
        public string DrawName { get; set; }
        public string EditorName { get; set; }
        public string HeaderInfo { get; set; }

        /// <summary>
        ///     Prefix of the content of item.
        /// </summary>
        public string ItemPrefix { get; set; }

        /// <summary>
        ///     Defines whether the index of list should start from 1 or 0.
        /// </summary>
        public bool ItemFixedIndex { get; set; }

        /// <summary>
        ///     Defines whether the foldout header should be drawn.
        /// </summary>
        public JEMBetterEditorHeaderType FoldoutHeaderType { get; set; }

        /// <summary>
        ///     If true, the target content will be read only.
        /// </summary>
        public bool Readonly { get; set; }
    
        /// <summary>
        ///      Better editor style constructor.
        /// </summary>
        public JEMBetterEditorStyle(string drawName, string editorName)
        {
            DrawName = drawName;
            EditorName = editorName;
            HeaderInfo = string.Empty;
            ItemPrefix = "Element";
            ItemFixedIndex = false;
            FoldoutHeaderType = JEMBetterEditorHeaderType.HeaderGroup;
            Readonly = false;
        }
    }
}