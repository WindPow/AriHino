using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooksButtonHandler : MonoBehaviour
{

    [SerializeField] private MultiObjectSwitcher contentsSwitcher;

    [SerializeField] private ObjectActivator[] stickyNoteActivators;

    public void OnTapStikcyNote(int index) {

        contentsSwitcher.SwitchObject(index);

        // 付箋の表示切替を行う
        for(int i = 0; i < stickyNoteActivators.Length; i++) {
            stickyNoteActivators[i].ActiveChangeObject(i == index);
        }
    }
}
