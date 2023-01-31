using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Word : MonoBehaviour
{
    public Action<Word> OnClick;
    private Button btn;
    public Letter letter;
    public bool IsEmpty;
    
    [SerializeField] private TextMeshProUGUI textLetter;
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(ClearWord);
        textLetter.text = " ";
        IsEmpty = true;
        btn.interactable = false;
    }
    public void SetLetter(Letter selectedLetter) 
    {
        btn.interactable = true;
        this.letter = selectedLetter;
        textLetter.text = selectedLetter.letter.ToString();
        IsEmpty = false;
    }
    public void ResetWord()
    {
        btn.interactable= false;

        letter = null;
        textLetter.text = " ";
        IsEmpty = true;
    }
    public void ClearWord()
    {

        letter.SetInterecation(true);
        ResetWord();
        OnClick?.Invoke(this);
    }
}
