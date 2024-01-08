#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;
using UtageExtensions;

namespace Utage
{
    public abstract class EditorSettingsSingleton<T> : ScriptableSingleton<T>
        where T : ScriptableObject
    {
        public static T GetInstance()
        {
            return instance;
        }

        public virtual void OnSave()
        {
            this.Save(true);
        }
    }
}
#endif
