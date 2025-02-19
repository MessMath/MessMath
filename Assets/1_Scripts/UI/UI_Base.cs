using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    protected bool _init = false;

    public virtual bool Init()
    {
        if (_init)
            return false;
        return _init = true;
    }

    private void Start()
    {
        Init();
    }

    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Utils.FindChild(gameObject, names[i], true);
            else
                objects[i] = Utils.FindChild<T>(gameObject, names[i], true);

            if (objects[i] == null)
                Debug.Log($"Failed to bind({names[i]})");
        }
    }


    protected void BindObject(Type type) { Bind<GameObject>(type); }
    protected void BindImage(Type type) { Bind<Image>(type); }
    protected void BindText(Type type) { Bind<TextMeshProUGUI>(type); }
    protected void BindButton(Type type) { Bind<Button>(type); }

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
    protected TextMeshProUGUI GetText(int idx) { return Get<TextMeshProUGUI>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }

    public static void BindEvent(GameObject go, Action action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_EventHandler evt = Utils.GetOrAddComponent<UI_EventHandler>(go);

        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Define.UIEvent.Pressed:
                evt.OnPressedHandler -= action;
                evt.OnPressedHandler += action;
                break;
            case Define.UIEvent.PointerDown:
                evt.OnPointerDownHandler -= action;
                evt.OnPointerDownHandler += action;
                break;
            case Define.UIEvent.PointerUp:
                evt.OnPointerUpHandler -= action;
                evt.OnPointerUpHandler += action;
                break;
        }
    }

    protected void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError("DatabaseError: " + args.DatabaseError.Message);
            //return;
        }
        if(args == null)
        {
            return;
        }
        if (args.Snapshot != null && args.Snapshot.Exists)
        {
            string newNickname = args.Snapshot.Child("nickname").Value.ToString();
            string newCoin = args.Snapshot.Child("coin").Value.ToString();
            string newIsCompletedDiagnosis = args.Snapshot.Child("isCompletedDiagnosis").Value.ToString();
            string newIsCompletedStory = args.Snapshot.Child("isCompletedStory").Value.ToString();
            string newIsTutorial = args.Snapshot.Child("isCompletedTutorial").Value.ToString();
            string newMessage = args.Snapshot.Child("message").Value.ToString();
            string newScore = args.Snapshot.Child("score").Value.ToString();
            string newStoryGrace1 = "";
            string newStoryGrace2 = "";
            string newStoryGrace3 = "";
            string newOneOnOneGrace1 = "";
            string newOneOnOneGrace2 = "";
            string newOneOnOneGrace3 = "";
            if (args.Snapshot.Child("StoryGrace").Exists)
            {
                newStoryGrace1 = args.Snapshot.Child("StoryGrace").Child("1").Value.ToString();
                newStoryGrace2 = args.Snapshot.Child("StoryGrace").Child("2").Value.ToString();
                newStoryGrace3 = args.Snapshot.Child("StoryGrace").Child("3").Value.ToString();
            }
            if (args.Snapshot.Child("StoryGrace").Exists)
            {
                newOneOnOneGrace1 = args.Snapshot.Child("OneOnOneGrace").Child("1").Value.ToString();
                newOneOnOneGrace2 = args.Snapshot.Child("OneOnOneGrace").Child("2").Value.ToString();
                newOneOnOneGrace3 = args.Snapshot.Child("OneOnOneGrace").Child("3").Value.ToString();
            }
            string newObtainedClothes = args.Snapshot.Child("Inventory").Child("ObtainedClothes").Value.ToString();
            string newObtainedCollections = args.Snapshot.Child("Inventory").Child("ObtainedCollections").Value.ToString();
            string newObtainedGraces = args.Snapshot.Child("Inventory").Child("ObtainedGraces").Value.ToString();

            //string newObtainedClothes = args.Snapshot.Child("obtainedClothes").Value.ToString();
            //string newObtainedCollections = args.Snapshot.Child("obtainedCollections").Value.ToString();
            //string newObtainedGraces = args.Snapshot.Child("obtainedGraces").Value.ToString();

            Managers.UserMng.nickname = newNickname;
            Managers.UserMng.coin = int.Parse(newCoin);
            Managers.UserMng.isCompletedDiagnosis = bool.Parse(newIsCompletedDiagnosis);
            Managers.UserMng.isCompletedStory = bool.Parse(newIsCompletedStory);
            Managers.UserMng.isCompletedTutorial = bool.Parse(newIsTutorial);
            Managers.UserMng.message = newMessage;
            Managers.UserMng.score = int.Parse(newScore);
            Managers.UserMng.storyModeGrace.grace1 = newStoryGrace1;
            Managers.UserMng.storyModeGrace.grace2 = newStoryGrace2;
            Managers.UserMng.storyModeGrace.grace3 = newStoryGrace3;
            Managers.UserMng.oneOnOneModeGrace.grace1 = newOneOnOneGrace1;
            Managers.UserMng.oneOnOneModeGrace.grace2 = newOneOnOneGrace2;
            Managers.UserMng.oneOnOneModeGrace.grace3 = newOneOnOneGrace3;
            Managers.UserMng.inventory.obtainedClothes = newObtainedClothes;
            Managers.UserMng.inventory.obtainedCollections = newObtainedCollections;
            Managers.UserMng.inventory.obtainedGraces = newObtainedGraces;
        }
        else
        {
            Debug.LogWarning("Data not found in the database.");
        }
    }
}
