using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SignIn : UI_Scene
{
    enum Images
    {
        BG
    }

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
        BindImage(typeof(Images));

        GetButton((int)Buttons.SignInButton).gameObject.BindEvent(OnClickedSignIn);
        GetButton((int)Buttons.SignOutButton).gameObject.BindEvent(OnClickedSignOut);

        GetImage((int)Images.BG).gameObject.BindEvent(OnClickBG);
        return true;
    }

    void OnClickedSignIn()
    {
        Managers.GoogleSignIn.SignInWithGoogle();
        Managers.DBManager.CreateNewUser("test");
        Debug.Log("로그인");
    }

    void OnClickedSignOut()
    {
        Managers.GoogleSignIn.SignOutFromGoogle();
        Debug.Log("로그아웃");
    }

    void OnClickBG()
    {
        if(Managers.GoogleSignIn.isLogin() == true)
            Managers.UI.ShowPopupUI<UI_TestInfo>();
    }
}
