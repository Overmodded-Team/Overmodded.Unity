using UnityEditor;
using UnityEngine;

namespace JEM.UnityEditor
{
    public class SavedObject<T> where T : Object
    {
        T m_Value;
        string m_Name;
        bool m_Loaded;

        public SavedObject(string name, T value)
        {
            m_Name = name;
            m_Loaded = false;
            m_Value = value;
        }

        void Load()
        {
            if (m_Loaded)
                return;

            m_Loaded = true;
            if (!EditorPrefs.HasKey(m_Name))
                m_Value = null;
            else
            {
                var assetPath = EditorPrefs.GetString(m_Name);
                m_Value = string.IsNullOrEmpty(assetPath) ? null : AssetDatabase.LoadAssetAtPath<T>(assetPath);
            }
        }

        public T value
        {
            get
            {
                Load();
                return m_Value;
            }
            set
            {
                Load();
                if (m_Value == value)
                    return;
                m_Value = value;

                if (m_Value != null)
                    EditorPrefs.SetString(m_Name, AssetDatabase.GetAssetPath(m_Value));
                else
                    EditorPrefs.SetString(m_Name, string.Empty);
            }
        }
    }
}
