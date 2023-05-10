using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class WitchController : MonoBehaviour
{
    public int Hp = 100;
    public Slider HpBar;
    public int QusetionNumber;
    public TextMeshProUGUI QusetionNumberText;

    private void Start()
    {
        HpBar.value = Hp;
        Questioning();
    }

    private void FixedUpdate()
    {
        HpBar.value = Hp;
    }

    public void Questioning()
    {
        QusetionNumber = Random.Range(10, 100);
        QusetionNumberText.text = $"{QusetionNumber}";
    }
}
