using UnityEditor;

namespace JEM.UnityEditor
{
    public class SavedString
    {
        string m_Value;
        string m_Name;
        bool m_Loaded;

        public SavedString(string name, string value)
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
            m_Value = EditorPrefs.GetString(m_Name, m_Value);
        }

        public string value
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
                EditorPrefs.SetString(m_Name, value);
            }
        }
    }
}
