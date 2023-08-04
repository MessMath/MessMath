using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.SceneManagement;
//using static Define;

public class Managers : MonoBehaviour
{
    public static Managers s_instance = null;
    public static Managers Instance { get { return s_instance; } }

    public static WJ_Connector s_connector = null;
    public static WJ_Connector Connector { get { return s_connector; } }

    private static UserManager s_userManager = new UserManager();
    private static GraceManager s_graceManager = new GraceManager();
    private static DebuffManager s_debuffManager = new DebuffManager();
    private static DataManager s_dataManager = new DataManager();
    private static GameManagerEx s_gameManagerEx = new GameManagerEx();
    private static ResourceManager s_resourceManager = new ResourceManager();
    private static SceneManagerEx s_SceneManager = new SceneManagerEx();
    private static SoundManager s_soundManager = new SoundManager();
    private static TextEffectManager s_textEffectManager = new TextEffectManager();
    private static SceneEffectManager s_sceneEffectManager = new SceneEffectManager();
    private static UIManager s_uiManager = new UIManager();
    private static CoinManager s_coinManager = new CoinManager();
    private static DatabaseManager s_dbManager = new DatabaseManager();
    private static GoogleSignInManager s_gogleSigninManager = new GoogleSignInManager();

    public static UserManager UserMng { get { Init(); return s_userManager; } }
    public static GraceManager Grace { get { Init(); return s_graceManager; } }
    public static DebuffManager Debuff { get { Init(); return s_debuffManager; } }
    public static DataManager Data { get { Init(); return s_dataManager; } }
    public static GameManagerEx Game { get { Init(); return s_gameManagerEx; } }
    public static ResourceManager Resource { get { Init(); return s_resourceManager; } }
    public static SceneManagerEx Scene { get { Init(); return s_SceneManager; } }
    public static SoundManager Sound { get { Init(); return s_soundManager; } }
    public static TextEffectManager TextEffect { get { Init(); return s_textEffectManager; } }
    public static SceneEffectManager SceneEffect { get { Init(); return s_sceneEffectManager; } }
    public static UIManager UI { get { Init(); return s_uiManager; } }
    public static CoinManager Coin { get { Init(); return s_coinManager; } }
    public static DatabaseManager DBManager {get { Init(); return s_dbManager; } }
    public static GoogleSignInManager GoogleSignIn { get { Init(); return s_gogleSigninManager; } }

    public static string GetText(int id)
    {
        if (Managers.Data.Texts.TryGetValue(id, out TextData value) == false)
        {
            Debug.Log("Not Data");
            return "";
        }

        return value.kor.Replace("{userName}", Managers.Game.Name);
    }

    private void Start()
    {
        Init();
    }

    private static void Init()
    {
        if (s_instance == null)
        {
            //PlayerPrefs.DeleteAll();
            //PlayerPrefs.SetInt("WatchedStory", -2);
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
                go = new GameObject { name = "@Managers" };

            GameObject connectorGo = GameObject.Find("@Connector");
            if (connectorGo == null)
                connectorGo = new GameObject { name = "@Connector" };

            s_connector = Utils.GetOrAddComponent<WJ_Connector>(connectorGo);
            s_instance = Utils.GetOrAddComponent<Managers>(go);
            DontDestroyOnLoad(go);
            DontDestroyOnLoad(connectorGo);

            //s_userManager.Init();
            s_resourceManager.Init();
            s_soundManager.Init();
            s_dataManager.Init();
            s_connector.Init();
            s_dbManager.Init();
            s_gogleSigninManager.Init();

            Application.targetFrameRate = 60;
        }
    }
}