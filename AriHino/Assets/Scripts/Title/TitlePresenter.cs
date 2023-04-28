using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePresenter : MonoBehaviour, IScenePresenter
{
    private TitleModel titleModel;

    public void Initialize(ISceneModel sceneModel){

        titleModel = sceneModel as TitleModel;

    }

    public void Display(){

    }

    public void Delete(){
        
    }
}
