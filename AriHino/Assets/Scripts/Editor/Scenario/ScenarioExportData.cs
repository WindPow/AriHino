using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CsvHelper.Configuration.Attributes;

namespace Arihino.Editor
{
    public class ScenarioExportData
    {
        /// <summary>
        /// データナンバー
        /// </summary>
        [Index(0)]
        public int No { get; set; }

        /// <summary>
        /// キャラネーム
        /// </summary>
        [Index(1)]
        public string CharaName { get; set; }

        /// <summary>
        /// キャラのアバターID
        /// </summary>
        [Index(2)]
        public string CharaAvatarName { get; set; }

        /// <summary>
        /// 表示テキスト
        /// </summary>
        [Index(3)]
        public string Text { get; set; }

        /// <summary>
        /// メッセージのタイプ
        /// 0 = キャラメッセージ
        /// 1 = ト書き
        /// 2 = その他
        /// </summary>
        [Index(4)]
        public string MessageType { get; set; }

        /// <summary>
        /// ト書き用のID
        /// </summary>
        [Index(5)]
        public string ToWriteId { get; set; }

        /// <summary>
        /// 詳細
        /// </summary>
        [Index(6)]
        public string Description { get; set; }

    }
}


