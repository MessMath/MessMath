using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class UI_PvpMatchingScene : UI_Scene
{
    Player[] playerList;

    Player OppPlayer;
    string OppPlayersCloth;

    public string PlayerClothes;

    enum Buttons
    {
        BackBtn,
    }

    enum Images
    {
        PlayerImage,
        EnemyImage,
        FightImage,
    }

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        // 서버에 연결!
        Managers.Network.Connect();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        GetButton((int)Buttons.BackBtn).gameObject.BindEvent(toMain);

        //GetImage((int)Images.DecoImage).gameObject.SetActive(false);

        Managers.Sound.Clear();

        Managers.Sound.Play("MatchingBgm", Define.Sound.Bgm);

        StartCoroutine(SceneChangeAnimation_Out());
        

        return true;
    }

    public void toMain()
    {
        // Sound
        Managers.Sound.Play("ClickBtnEff");

        PhotonNetwork.Disconnect();
        PhotonNetwork.AutomaticallySyncScene = false;

        StartCoroutine(SceneChangeAnimation(Define.Scene.LobbyScene));
        Time.timeScale = 1.0f;
    }

    #region
    IEnumerator SceneChangeAnimation_In_Lobby()
    {
        // Ani
        UI_LockTouch uI_LockTouch = Managers.UI.ShowPopupUI<UI_LockTouch>();
        SceneChangeAnimation_In anim = Managers.Resource.Instantiate("Animation/SceneChangeAnimation_In").GetOrAddComponent<SceneChangeAnimation_In>();
        anim.transform.SetParent(this.transform);
        anim.SetInfo(Define.Scene.LobbyScene, () => { Managers.Scene.ChangeScene(Define.Scene.LobbyScene); });

        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator SceneChangeAnimation_Out()
    {
        // Ani
        UI_LockTouch uI_LockTouch = Managers.UI.ShowPopupUI<UI_LockTouch>();
        SceneChangeAnimation_Out anim = Managers.Resource.Instantiate("Animation/SceneChangeAnimation_Out").GetOrAddComponent<SceneChangeAnimation_Out>();
        anim.transform.SetParent(this.transform);
        anim.SetInfo(Define.Scene.Fight1vs1GameScene, () => { });

        yield return new WaitForSeconds(0.3f);
        Managers.UI.ClosePopupUI(uI_LockTouch);
    }

    #endregion

    IEnumerator MatchingAni()
    {
        Managers.Sound.Play("챙3");
        GetImage((int)Images.PlayerImage).gameObject.GetOrAddComponent<Animator>().Play("MatchingPlayerImageAni");
        yield return new WaitForSeconds(0.3f);
        GetImage((int)Images.EnemyImage).gameObject.GetOrAddComponent<Animator>().Play("MatchingAnemyImageAni");
        yield return new WaitForSeconds(0.5f);
        GetImage((int)Images.FightImage).gameObject.GetOrAddComponent<Animator>().Play("MatchingFight");
        yield return new WaitForSeconds(0.15f);
        Managers.Sound.Play("FightEff");
        yield return new WaitForSeconds(0.5f);
        Managers.Sound.Play("DuongEff");

        PhotonNetwork.LoadLevel("PvpGameScene");
    }

    IEnumerator SceneChangeAnimation(Define.Scene Scene)
    {
        // Ani
        UI_LockTouch uI_LockTouch = Managers.UI.ShowPopupUI<UI_LockTouch>();
        SceneChangeAnimation_Out anim = Managers.Resource.Instantiate("Animation/SceneChangeAnimation_In").GetOrAddComponent<SceneChangeAnimation_Out>();
        anim.transform.SetParent(this.transform);
        anim.SetInfo(Scene, () => { });

        yield return new WaitForSeconds(0.3f);
        Managers.UI.ClosePopupUI(uI_LockTouch);

        Managers.Sound.Play("ClickBtnEff");
        Managers.Scene.ChangeScene(Scene);
    }

    public async void WhenMatched()
    {
        playerList = PhotonNetwork.PlayerList;
        OppPlayer = GetOppPlayer();

        OppPlayersCloth = await  Managers.DBManager.ReadDataAsync(OppPlayer.NickName, "myClothes");
        PlayerClothes = await Managers.DBManager.GetMyClothes(Managers.GoogleSignIn.GetUID());

        GetImage((int)Images.PlayerImage).sprite = Managers.Resource.Load<Sprite>("Sprites/Clothes/" + PlayerClothes + "_full");
        GetImage((int)Images.EnemyImage).sprite = Managers.Resource.Load<Sprite>("Sprites/Clothes/" + OppPlayersCloth + "_full");

        StartCoroutine("MatchingAni");
    }

    Player GetOppPlayer()
    {
        for (int i = 0; i <= playerList.Count(); i++)
        {
            Debug.Log("All __" + playerList[i]);
            if (playerList[i].ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
            {
                Debug.Log(playerList[i]);
                return playerList[i];
            }
        }
        return null;
    }

}
