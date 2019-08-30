using UnityEditor;
using UnityEngine;

namespace JEM.UnityEditor
{
    public class SavedVector2
    {
        Vector2 m_Value;
        string m_Name;
        bool m_Loaded;

        public SavedVector2(string name, Vector2 value)
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
            m_Value = new Vector2(EditorPrefs.GetFloat(m_Name + ".x"), EditorPrefs.GetFloat(m_Name + ".y"));
        }

        public Vector2 value
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
                EditorPrefs.SetFloat(m_Name + ".x", value.x);
                EditorPrefs.SetFloat(m_Name + ".y", value.y);
            }
        }
    }
}
