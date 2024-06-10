using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Text;

public class ParticleEffectManager : MonoBehaviour
{
    private static ParticleEffectManager instance;
    public static ParticleEffectManager Instance {

        get {
            if (instance == null) instance = GameObject.FindObjectOfType<ParticleEffectManager>();

            if (instance == null) {
                GameObject singletonObject = new GameObject(typeof(ParticleEffectManager).Name);
                instance = singletonObject.AddComponent<ParticleEffectManager>();
            }
            return instance;
        }
    }

    private const string prefabPath = "Prefabs/Effect/ParticleEffect/{0}";

    [SerializeField] private Transform uiRoot;
    [SerializeField] private Transform frontRoot;

    private Dictionary<int, GameObject> effectDict = new();

    public void SetParticleEffect(int effectId, int rootType) {

        var mstEffect = MasterDataManager.Instance.GetMasterData<MstEffectData>(effectId);
        var prefab = Resources.Load<GameObject>(ZString.Format(prefabPath, mstEffect.FileName));

        // var obj = Instantiate(prefab, GetEffectRoot((ParticleEffectRootType)rootType));
        // effectDict.Add(effectId, obj);
    }

    private Transform GetEffectRoot(ParticleEffectRootType rootType) {

        switch (rootType) {
            case ParticleEffectRootType.FRONT:
                return frontRoot;
            case ParticleEffectRootType.UI:
                return uiRoot;
            default:
                return null;
        }
    }

    public void RemoveEffect(int effectId) {

        if(effectDict.TryGetValue(effectId, out GameObject removeObj)){
            effectDict.Remove(effectId);
            Destroy(removeObj);
        }
    }

    private enum ParticleEffectRootType {
        FRONT = 0,
        UI = 1,
    }

    

    
}
