using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScenePresenter
{
    public void Initialize(ISceneModel model);

    public void Display();

    public void Delete();
    
}
