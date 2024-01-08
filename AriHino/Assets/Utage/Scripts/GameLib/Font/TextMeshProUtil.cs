using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Pool;

namespace Utage
{
    public static class TextMeshProUtil
    {
        //フォールバックを含めたフォントアセットを取得
        public static IEnumerable<TMP_FontAsset> GetFontAssetsWithFallback(TMP_FontAsset fontAsset)
        {
            void AddFontAsset(TMP_FontAsset fontAsset, HashSet<TMP_FontAsset> fontSet)
            {
                if (!fontSet.Add(fontAsset)) return;

                foreach (var fallbackFont in fontAsset.fallbackFontAssetTable)
                {
                    AddFontAsset(fallbackFont, fontSet);
                }
            }

            using (HashSetPool<TMP_FontAsset>.Get(out HashSet<TMP_FontAsset> fontSet))
            {
                AddFontAsset(fontAsset, fontSet);
                foreach (var font in fontSet)
                {
                    yield return font;
                }
            }
        }
    }
}
