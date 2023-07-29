using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SignIn : UI_Scene
{

    enum Buttons
    {
        SignInButton,
        SignOutButton,
    }

    private void Start()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));

        GetButton((int)Buttons.SignInButton).gameObject.BindEvent(OnClickedSignIn);
        GetButton((int)Buttons.SignOutButton).gameObject.BindEvent(OnClickedSignOut);
        return true;
    }

    void OnClickedSignIn()
    {
        Managers.GoogleSignIn.SignInWithGoogle();
        Debug.Log("로그인");
        Managers.UI.ShowPopupUI<UI_TestInfo>();
    }

    void OnClickedSignOut()
    {
        Managers.GoogleSignIn.SignOutFromGoogle();
        Debug.Log("로그아웃");
    }
}
