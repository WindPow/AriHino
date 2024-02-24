using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utage;

/// <summary>
/// AdvEngineの拡張機能ラッパークラス（UtageUguiMainGameを継承）
/// </summary>
public class AdvEngineHandler : UtageUguiMainGame
{
    [SerializeField] private BooksViewController booksView;

    /// <summary>
    /// Booksアイコン押下
    /// </summary>
    public void OnTapBooks() {

        // TODO:ブックスの起動処理 
        booksView.Show();

    }

    
}
