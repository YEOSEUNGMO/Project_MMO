﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScene : BaseScene
{
    public override void Clear()
    {
        Debug.Log("Login Scene Clear!!");
    }

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Login;

        for (int i = 0; i < 2; i++)
            Managers.Resource.Instantiate("UnityChan");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Managers.Scene.LoadScene(Define.Scene.Game);
        }
    }
}
