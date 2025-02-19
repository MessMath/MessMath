﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : UI_Base
{
    public int _hp;
    public Vector2 _inputVec;
    public float _speed = 10;
    Rigidbody2D _rigid;
    Image _image;
    public GameObject _onCalculateBoard;
    public UI_Fight1vs1Game _fight1vs1sceneUi;

    bool _isAlive = true;

    float referenceWidth = 3200f; // 기준 해상도의 너비
    float referenceHeight = 1440f; // 기준 해상도의 높이
    float currentWidth = Screen.width; // 현재 화면의 너비
    float currentHeight = Screen.height; // 현재 화면의 높이

    float widthRatio;
    float heightRatio;
    public float adjustedSpeed;

    void Start()
    {
        _hp = 3;
        _speed = 500;
        _rigid = gameObject.GetOrAddComponent<Rigidbody2D>();
        _image = gameObject.GetOrAddComponent<Image>();

        if(Managers.Scene.CurrentSceneType == Define.Scene.Fight1vs1GameScene)
        {
            _onCalculateBoard = gameObject.transform.parent.GetComponentInChildren<TEXDraw>().gameObject;
            _fight1vs1sceneUi = GameObject.Find("UI_Fight1vs1Game").GetComponent<UI_Fight1vs1Game>();
        }
        else
        {
            _onCalculateBoard = gameObject.transform.parent.GetComponentInChildren<TextMeshProUGUI>().gameObject;
        }

        // 속도를 해상도에 맞춰 조절
        widthRatio = currentWidth / referenceWidth;
        heightRatio = currentHeight / referenceHeight;

        adjustedSpeed = _speed * Mathf.Min(widthRatio, heightRatio);

    }

    void FixedUpdate() 
    {
        Vector2 nextVec = _inputVec * adjustedSpeed * Time.fixedDeltaTime;
        _rigid.MovePosition(_rigid.position + nextVec);

        if (Managers.Game.Horizontal != 0 || Managers.Game.Vertical != 0)
            MoveControl();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어가 화살과 충돌하지 않도록 할 때
        if(Managers.Grace.playerCollisionOff)
        {
            Destroy(collision.gameObject);
            return;
        }

        if (collision.gameObject.tag == "Arrow")
        {
            //Debug.Log("Hit Arrow!");
            Arrow arrow = collision.gameObject.GetOrAddComponent<Arrow>();
            string symbol = arrow.text;
            Destroy(collision.gameObject);
            if(Managers.Grace.pythagorasOn)
            {
                Managers.Grace.pythagorasOn = false;
                return;
            }
            _onCalculateBoard.GetComponent<TextMeshProUGUI>().text += symbol;
            transform.parent.GetComponent<UI_Scene>().Invoke("PreCalculate",0);

        }
        else if(collision.gameObject.tag == "ArrowOnlyin1vs1")
        {
            WJ_Sample1vs1 wJ_Sample1vs1 = _fight1vs1sceneUi.wj_sample1vs1;
            ArrowOnlyin1vs1 arrow = collision.GetComponent<ArrowOnlyin1vs1>();

            int index = wJ_Sample1vs1.currentQuestionIndex;
            string qstCransr = Managers.Connector.cLearnSet.data.qsts[index].qstCransr;

            _fight1vs1sceneUi.wj_sample1vs1.SelectAnswer(arrow.text);

            // 정답을 맞췃으면서, 모드가 DoubleSolve 모드일 때
            if (arrow.text == qstCransr && Managers.Game.CurrentMode == Define.Mode.DoubleSolve)
            {
                index = wJ_Sample1vs1.currentQuestionIndex;
                qstCransr = Managers.Connector.cLearnSet.data.qsts[index].qstCransr;
                wJ_Sample1vs1.SelectAnswer(qstCransr);

                // Sound
                Managers.Sound.Play("AttackEff");
            }

            _fight1vs1sceneUi.Invoke("RefreshUI", 0);
            Destroy(collision.gameObject);
        }
    }

    private void MoveControl()
    {
        gameObject.GetComponent<RectTransform>().position += Vector3.up * adjustedSpeed * Time.deltaTime * Managers.Game.Vertical;
        gameObject.GetComponent<RectTransform>().position += Vector3.right * adjustedSpeed * Time.deltaTime * Managers.Game.Horizontal;
    }

    void OnMove(InputValue value)
    { 
        _inputVec = value.Get<Vector2>();
    }

    public void GameOverPopup()
    {
        if (_hp <= 0 && _isAlive == true)
        {
            Managers.UI.ShowPopupUI<UI_GameOver>();
            _isAlive = false;
        }
    }

    public void BlinkPlayerImg()
    {
        StartCoroutine("Hit");
    }

    IEnumerator Hit()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.15f);
        int countTime = 0;
        while(countTime < 10)
        {
            if (countTime % 2 == 0)
                _image.color = new Color32(255, 255, 255, 90);
            else
                _image.color = new Color32(255, 255, 255, 180);

            yield return waitForSeconds;

            countTime++;
        }

        _image.color = Color.white;
        yield return null;
    }
}
