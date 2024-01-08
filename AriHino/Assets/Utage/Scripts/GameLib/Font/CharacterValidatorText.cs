using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;

namespace Utage
{
    //文字化けを防ぐために、想定外の文字が含まれていないかチェックするクラス
    public class CharacterValidatorText : CharacterValidator
    {
        public TextAsset TextAsset { get; }
        public override Object TargetAsset => TextAsset;

        public CharacterValidatorText(TextAsset textAsset)
        {
            TextAsset = textAsset;
            AddEnableCharacters(TextAsset.text);
        }

#if false
        //テキストファイルに不足文字をマージする
        public void MergeCharacterFile()
        {
#if UNITY_EDITOR
            string addText = "";
            foreach (var keyValue in ErrorCharacters)
            {
                addText += FontUtil.UnicodeToCharacter(keyValue.Value.Unicode);
            }
            string path = UnityEditor.AssetDatabase.GetAssetPath(TextAsset);
            using StreamWriter writer = new StreamWriter(path, true);
            writer.Write(addText);
            writer.Close();
            UnityEditor.EditorUtility.SetDirty(TextAsset);
            Debug.Log($"Merge to {TextAsset.name}\n{addText} ");
#endif
        }
#endif
    }
}
