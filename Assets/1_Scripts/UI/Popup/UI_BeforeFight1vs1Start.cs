using MessMathI18n;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_BeforeFight1vs1Start : UI_Popup
{
    public enum Buttons
    {
        reqQstsBtn,
    }

    public enum Texts
    {
        GetDiagnosisQuestions,
        reqQsts,
    }

    public UI_Fight1vs1Game UI_Fight1Vs1Game;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));


        GetButton((int)Buttons.reqQstsBtn).gameObject.BindEvent(() => reqQsts());

        GetText((int)Texts.GetDiagnosisQuestions).text = I18n.Get(I18nDefine.GetDiagnosisQuestions);
        GetText((int)Texts.reqQsts).text = I18n.Get(I18nDefine.ReqQsts);

        CoroutineHandler.StartCoroutine(SceneChangeAnimation_Out());
        //Time.timeScale = 0;
        GetComponent<Canvas>().sortingOrder = 10;

        return true;
    }

    void reqQsts()
    {
        Time.timeScale = 1;
        Managers.Connector.Learning_GetQuestion();
        UI_Fight1Vs1Game.StartCoroutine("SetArrowGenerationTime", 1f);
        ClosePopupUI();
        UI_Fight1Vs1Game.Invoke("RefreshUI", 0.2f);
        UI_Fight1Vs1Game.GameStarted = true;
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

}