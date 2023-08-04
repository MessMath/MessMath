using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_SignIn : UI_Scene
{
    bool isExist = false;
    enum Images
    {
        BG
    }

    enum Buttons
    {
        SignInButton,
        SignOutButton,
        AddCoinButton,
        TestButton,
    }

    enum Texts
    {
        LogTMP,
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
        BindText(typeof(Texts));

        GetButton((int)Buttons.SignInButton).gameObject.BindEvent(OnClickedSignIn);
        GetButton((int)Buttons.SignOutButton).gameObject.BindEvent(OnClickedSignOut);
        GetButton((int)Buttons.AddCoinButton).gameObject.BindEvent(AddCoin);
        GetButton((int)Buttons.TestButton).gameObject.BindEvent(OnclickedTest);

        GetImage((int)Images.BG).gameObject.BindEvent(OnClickBG);
        return true;
    }

    void OnClickedSignIn()
    {
        Managers.GoogleSignIn.SignInWithGoogle();
        Managers.DBManager.CreateNewUser("test");

        //Managers.DBManager.CreateNewUser("test");
        /*GetText((int)Texts.LogTMP).text += "\nSignInWithGoogle\n" + Managers.GoogleSignIn.GetUID();
        GetText((int)Texts.LogTMP).text += "\nDBUid\n" + Managers.DBManager.ReadData(Managers.GoogleSignIn.GetUID(), "UID");
        GetText((int)Texts.LogTMP).text += "\nDBNickName: " + Managers.DBManager.ReadData(Managers.GoogleSignIn.GetUID(), "nickname");
        if(Managers.GoogleSignIn.GetUID() == Managers.DBManager.ReadData(Managers.GoogleSignIn.GetUID(), "UID"))
        {
            isExist = true;
            GetText((int)Texts.LogTMP).text += "\nExistUser";
        }*/
        Debug.Log("로그인");
    }

    void OnclickedTest()
    {
        /*GetText((int)Texts.LogTMP).text += "\nOnClickedTest\n" + Managers.GoogleSignIn.GetUID();
        if(isExist)
        {
            GetText((int)Texts.LogTMP).text += "\n Exist User";
            Managers.UserMng.SetExistingUser();
            GetButton((int)Buttons.AddCoinButton).gameObject.GetComponentInChildren<TextMeshProUGUI>().text = Managers.UserMng.user.coin.ToString();
        }
        else
        {
            GetText((int)Texts.LogTMP).text += "\n New User";
            Managers.DBManager.CreateNewUser("test");
        }*/
    }

    void OnClickedSignOut()
    {
        Managers.GoogleSignIn.SignOutFromGoogle();
        Debug.Log("로그아웃");
    }

    void OnClickBG()
    {
        if(Managers.GoogleSignIn.isLogin() == true)
            Managers.UI.ShowPopupUI<UI_GetNicknamePopup>();
    }

    void AddCoin()
    {
        Managers.DBManager.SetCoin(3);
        GetButton((int)Buttons.AddCoinButton).gameObject.GetComponentInChildren<TextMeshProUGUI>().text = Managers.UserMng.user.coin.ToString();
    }
}
