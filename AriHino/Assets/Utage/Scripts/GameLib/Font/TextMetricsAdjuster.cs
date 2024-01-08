using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore;

namespace Utage
{
    //TextMeshProのフォントアセットに設定するTextMetricsを調整するためのプリセット
    [CreateAssetMenu(menuName = "Utage/Font/" + nameof(TextMetricsAdjuster))]
    public class TextMetricsAdjuster : ScriptableObject
    {
        [SerializeField] TMP_FontAsset baseFont = null;
        [SerializeField] List<TMP_FontAsset> fonts = new ();

        [Serializable]
        class DisableCharacterSettings
        {
            public float maxAscent = 0;
            public float minDescend = 0;
            public bool disableCombiningMark = true;
            public string disableCharacters = "〱";
        }

        [SerializeField,UnfoldedSerializable] DisableCharacterSettings disableCharacterSettings = new();

        [SerializeField, Button(nameof(MakeTextMetrics), nameof(DisableMakeTextMetrics), false)]
        string makeTextMetrics;
        [SerializeField] TextMetrics textMetrics = new ();
        
 
        [SerializeField, Button(nameof(ApplyToFontAssets), nameof(DisableApplyToFontAssets), false)]
        string applyToFontAssets;


        bool DisableMakeTextMetrics()
        {
            if (baseFont == null) return true;
            if (fonts.Count <= 0) return true;
            return fonts.Exists(x => x.faceInfo.pointSize != baseFont.faceInfo.pointSize);
        }

        void MakeTextMetrics()
        {
            textMetrics = new TextMetrics(baseFont.faceInfo);

            List<FontAssetTextMetrics> fontAssetTextMetrics = new() { new FontAssetTextMetrics(baseFont, this) };
            foreach (var font in fonts)
            {
                fontAssetTextMetrics.Add(new FontAssetTextMetrics(font, this));
            }

            //対象フォントアセット内の全ての収録グリフの最大位置を取得
            float ascender = fontAssetTextMetrics.Select(x => x.AscenderGlyph.Ascent).Max();
            //対象フォントアセット内の全ての収録グリフの最小位置を取得
            float descender = fontAssetTextMetrics.Select(x => x.DecentGlyph.Descent).Min();
            textMetrics.AdjustLine(ascender,descender);
        }

        bool DisableApplyToFontAssets()
        {
            if( !DisableMakeTextMetrics() ) return false;
            return textMetrics.DisableApplyToFontAssets(baseFont);
        }


        void ApplyToFontAssets()
        {
            textMetrics.ApplyToFontAsset(baseFont);
            foreach (var font in fonts)
            {
                textMetrics.ApplyToFontAsset(font);
            }
        }
        
        class FontAssetTextMetrics
        {
            TextMetricsAdjuster Settings { get; }
            TMP_FontAsset Font { get; }
//          public TextMetrics TextMetrics { get; }
            public GlyphAndCharacters DecentGlyph{ get; }
            public GlyphAndCharacters AscenderGlyph { get; }
            List<UnicodeCharacter> DisableCharacters { get; } = new List<UnicodeCharacter>();

            //グリフ情報
            //グリフと文字の紐づけを行う
            public class GlyphAndCharacters
            {
                public Glyph Glyph { get; }
                public float Descent { get; }
                public float Ascent { get; }
                public List<TMP_Character> Characters { get; } = new List<TMP_Character>();
                public CheckResult Result { get; set; } 
                
                public enum CheckResult
                {
                    Enable,
                    DescentOver,
                    AscentOver,
                    CombiningMark,
                    DisableCharacters,
                }
                public GlyphAndCharacters(Glyph glyph)
                {
                    Glyph = glyph;
                    Descent = glyph.metrics.horizontalBearingY - glyph.metrics.height;
                    Ascent = glyph.metrics.horizontalBearingY;
                }

                //条件に従って有効なグリフか判別する
                public CheckResult Check(FontAssetTextMetrics metrics)
                {
                    Result = CheckSub(metrics);
                    return Result;
                }

                //条件に従って有効なグリフか判別する
                CheckResult CheckSub(FontAssetTextMetrics metrics)
                {
                    DisableCharacterSettings disableSettings = metrics.Settings.disableCharacterSettings;
                    if (disableSettings.disableCombiningMark)
                    {
                        foreach (var character in Characters)
                        {
                            UnicodeCharacter c = new UnicodeCharacter(character.unicode);
                            if (c.IsCombiningMark)
                            {
                                return CheckResult.CombiningMark;
                            }
                        }
                    }

                    if (disableSettings.disableCombiningMark)
                    {
                        foreach (var character in Characters)
                        {
                            if (metrics.DisableCharacters.Exists(x => x.Unicode == character.unicode)) return CheckResult.DisableCharacters;
                        }
                    }

                    if (disableSettings.minDescend < 0 && Descent < disableSettings.minDescend)
                    {
                        return CheckResult.DescentOver;
                    }

                    if (disableSettings.maxAscent > 0 && Ascent > disableSettings.maxAscent)
                    {
                        return CheckResult.DescentOver;
                    }


                    return CheckResult.Enable;
                }
                
                public string GetCharactersString()
                {
                    string result = "";
                    foreach (var character in Characters)
                    {
                        result += FontUtil.UnicodeToCharacter(character.unicode);
                    }
                    return result;
                }
            }
            Dictionary<uint,GlyphAndCharacters> Glyphs { get; } = new ();

            public FontAssetTextMetrics(TMP_FontAsset font, TextMetricsAdjuster settings)
            {
                Settings = settings;
                Font = font;
                foreach (uint unicode in FontUtil.ToUnicodeCharacters(Settings.disableCharacterSettings.disableCharacters))
                {
                    DisableCharacters.Add(new UnicodeCharacter(unicode));
                }
                foreach (Glyph glyph in font.glyphTable)
                {
                    Glyphs.Add(glyph.index,new GlyphAndCharacters(glyph));
                }
                foreach (var character in font.characterTable)
                {
                    if (!Glyphs.TryGetValue(character.glyphIndex, out GlyphAndCharacters glyphAndCharacters))
                    {
                        Debug.LogError($"{font.name} glyphIndexError={character.glyphIndex}/{Glyphs.Count} {FontUtil.UnicodeToCharacter(character.unicode)}",font);
                        continue;
                    }
                    glyphAndCharacters.Characters.Add(character);
                }

                List<GlyphAndCharacters> enableGlyphs = new ();
                List<GlyphAndCharacters> disableGlyphs = new();
                foreach (var keyValue in Glyphs)
                {
                    var glyph = keyValue.Value;
                    if (glyph.Check(this) == GlyphAndCharacters.CheckResult.Enable)
                    {
                        enableGlyphs.Add(glyph);
                    }
                    else
                    {
                        disableGlyphs.Add(glyph);
                    }
                }
                DecentGlyph = GetDecentGlyphByAllCharactersInFontAsset(enableGlyphs);
                AscenderGlyph = GetAscenderGlyphByAllCharactersInFontAsset(enableGlyphs);
                
                //グリフ中の最低位置を持つグリフと、最高位置のグリフを出力
                string overCharacters = "";
                foreach (var glyph in disableGlyphs)
                {
                    switch (glyph.Result)
                    {
                        case GlyphAndCharacters.CheckResult.DescentOver:
                        case GlyphAndCharacters.CheckResult.AscentOver:
                            overCharacters += glyph.GetCharactersString();
                            break;
                        default:
                            break;
                    }
                }

                Debug.Log($"{font.name}  OverCharacters={overCharacters}  \n"
                          + $"Ascent={AscenderGlyph.Ascent} Glyph({AscenderGlyph.Glyph.index}) {AscenderGlyph.GetCharactersString()}  \n"
                          + $"Decent={DecentGlyph.Descent} Glyph({DecentGlyph.Glyph.index}) {DecentGlyph.GetCharactersString()}  \n"
                          );
            }

            //フォントのアセットの収録文字の中で、一番低い位置を持つグリフを取得
            GlyphAndCharacters GetDecentGlyphByAllCharactersInFontAsset(List<GlyphAndCharacters> enableGlyphs)
            {
                return enableGlyphs.OrderBy(x=>x.Descent).FirstOrDefault();
            }
            //フォントのアセットの収録文字の中で、一番高い位置を持つグリフを取得
            GlyphAndCharacters GetAscenderGlyphByAllCharactersInFontAsset(List<GlyphAndCharacters> enableGlyphs)
            {
                return enableGlyphs.OrderByDescending(x=>x.Ascent).FirstOrDefault();
            }
        }

    }
}
