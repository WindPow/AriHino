using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CsvHelper.Configuration.Attributes;

public class ScenarioExportData
{
    /// <summary>
    /// �f�[�^�i���o�[
    /// </summary>
    [Index(0)]
    public int No { get; set; }
    
    /// <summary>
    /// �L�����l�[��
    /// </summary>
    [Index(1)]
    public string CharaName { get; set; }

    /// <summary>
    /// �L�����̃A�o�^�[ID
    /// </summary>
    [Index(2)]
    public int CharaAvatarId { get; set; }

    /// <summary>
    /// �\���e�L�X�g
    /// </summary>
    [Index(3)]
    public string Text { get; set; }

    /// <summary>
    /// ���b�Z�[�W�̃^�C�v
    /// 0 = �L�������b�Z�[�W
    /// 1 = �g����
    /// 2 = ���̑�
    /// </summary>
    [Index(4)]
    public int MessageType { get; set; }
        
}
