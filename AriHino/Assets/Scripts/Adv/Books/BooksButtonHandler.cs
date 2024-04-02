using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Linq;

public class BooksButtonHandler : MonoBehaviour
{

    [SerializeField] private MultiObjectSwitcher contentsSwitcher;

    [SerializeField] private ObjectActivator[] stickyNoteActivators;

    [SerializeField] private Animation[] pageNextAnims;

    [SerializeField] private Animation[] pagePrevAnims;

    private int indexNow = 0;
    private float pageAnimDeley = 0.1f;

    /// <summary>
    /// 付箋タップ処理
    /// </summary>
    /// <param name="index"></param>
    public void OnTapStikcyNote(int index) {

        if(index == indexNow) return;

        // 付箋の表示切替を行う
        for(int i = 0; i < stickyNoteActivators.Length; i++) {
            stickyNoteActivators[i].ActiveChangeObject(i == index);
        }

        // 表示物を非アクティブにする
        contentsSwitcher.SetObjectsActive(indexNow, false);

        UniTask.Void(async () => {

            // アニメージョン開始
            await PlayPageMultiAnim(index > indexNow);

            // ページ内容物を表示
            contentsSwitcher.SwitchObject(index);
        });
        
        indexNow = index;
    }

    /// <summary>
    /// ページめくりアニメーション（単体）
    /// </summary>
    /// <param name="isNext"></param>
    private void PlayPageSingleAnim(bool isNext) {
        
        if(isNext) pageNextAnims[0].Play();
        else pagePrevAnims[0].Play();
    }

    /// <summary>
    /// ページめくりアニメーション（付箋）
    /// </summary>
    /// <param name="isNext"></param>
    /// <returns></returns>
    private async UniTask PlayPageMultiAnim(bool isNext) {

        if(isNext) {
            
            foreach(var anim in pageNextAnims) {
                anim.Play();

                await UniTask.Delay((int)(pageAnimDeley * 1000f));
            }

            await UniTask.WhenAll(pageNextAnims.Select(anim => anim.WaitForCompletionAsync()));
        }

        else{

            foreach(var anim in pagePrevAnims) {
                anim.Play();

                await UniTask.Delay((int)(pageAnimDeley * 1000f));
            }

            await UniTask.WhenAll(pagePrevAnims.Select(anim => anim.WaitForCompletionAsync()));
        }
    }
}
