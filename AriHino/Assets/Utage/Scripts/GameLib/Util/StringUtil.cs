using System;

namespace Utage
{
    //文字列操作全般のユーティリティ
    public static class StringUtil
    {
        //指定の文字列が、指定の接尾辞で終わっている場合、その接尾辞を削除した文字列を返す
        public static string RemoveSuffix(string str, string suffix)
        {
            return RemoveSuffix(str, suffix, StringComparison.InvariantCultureIgnoreCase);
        }
        public static string RemoveSuffix(string str, string suffix, StringComparison comparisonType)
        {
            if (str.EndsWith(suffix, comparisonType))
            {
                return str.Substring(0, str.Length - suffix.Length);
            }
            else
            {
                return str;
            }
        }


        //改行文字をエスケープ処理
        public static string EscapeNewlines(string msg)
        {
            msg = msg.Replace("\r\n", @"\n");
            msg = msg.Replace("\n", @"\n");
            msg = msg.Replace("\r", @"\n");
            return msg;
        }


    }
}
