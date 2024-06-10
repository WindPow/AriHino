using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Linq;
using System;
using UniRx;

public class BooksPageButtonHandler : MonoBehaviour
{

    [SerializeField] private MultiObjectSwitcher contentsSwitcher;
    [SerializeField] private ObjectActivator[] stickyNoteActivators;
    [SerializeField] private Animation[] pageNextAnims;
    [SerializeField] private Animation[] pagePrevAnims;
    [SerializeField] private float pageAnimDeley = 0.1f;
    
    public IObservable<int> ChangeIndexObservable => changeIndexSubject;
    private Subject<int> changeIndexSubject = new Subject<int>();

    public bool IsPlayingAnim { get; private set; }
    private int indexNow = 0;
    

    /// <summary>
    /// 付箋タップ処理
    /// </summary>
    /// <param name="index"></param>
    public void OnTapStikcyNote(int index) {

        if(IsPlayingAnim) return;

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
        changeIndexSubject.OnNext(index);
    }

    /// <summary>
    /// ページめくりアニメーション（単体）
    /// </summary>
    /// <param name="isNext"></param>
    public async UniTask PlayPageSingleAnim(bool isNext) {

        IsPlayingAnim = true;
        
        if(isNext) {
            pageNextAnims[0].gameObject.SetActive(true);
            pageNextAnims[0].Play();
            await pageNextAnims[0].WaitForCompletionAsync();
            pageNextAnims[0].gameObject.SetActive(false);
            
        }
        else {
            pagePrevAnims[0].gameObject.SetActive(true);
            pagePrevAnims[0].Play();
            await pagePrevAnims[0].WaitForCompletionAsync();
            pagePrevAnims[0].gameObject.SetActive(false);
            
        }

        IsPlayingAnim = false;
    }

    /// <summary>
    /// ページめくりアニメーション（付箋）
    /// </summary>
    /// <param name="isNext"></param>
    /// <returns></returns>
    public async UniTask PlayPageMultiAnim(bool isNext) {

        IsPlayingAnim = true;

        if(isNext) {
            
            foreach(var anim in pageNextAnims) {
                anim.gameObject.SetActive(true);
                anim.Play();

                await UniTask.Delay((int)(pageAnimDeley * 1000f));
            }

            await UniTask.WhenAll(pageNextAnims.Select(anim => anim.WaitForCompletionAsync()));

            foreach(var anim in pageNextAnims) anim.gameObject.SetActive(false);
            
        }

        else{

            foreach(var anim in pagePrevAnims) {
                anim.gameObject.SetActive(true);
                anim.Play();

                await UniTask.Delay((int)(pageAnimDeley * 1000f));
            }

            await UniTask.WhenAll(pagePrevAnims.Select(anim => anim.WaitForCompletionAsync()));

            foreach(var anim in pagePrevAnims) anim.gameObject.SetActive(false);
            
        }

        IsPlayingAnim = false;
    }
}
