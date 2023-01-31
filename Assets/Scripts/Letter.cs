using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Letter : MonoBehaviour
{
    public Action<Letter> OnClick;
    public char letter;
    private Button btn;
    private Image img;
    [SerializeField]
    private TextMeshProUGUI textLetter;
    
    void Start()
    {
        img= gameObject.GetComponent<Image>();
        btn = GetComponent<Button>();
        btn.onClick.AddListener(ClickLetter);
        textLetter.text = letter.ToString();
    }
    public void SetLetter(char v)
    {
        letter = v;
        textLetter.text = v.ToString();
    }
    public void ResetLetter()
    {

        btn.interactable = true;
    }
    public void SetInterecation(bool flag)
    {
        btn.interactable= flag;
        textLetter.enabled = flag;
        img.enabled = flag;
    }
    public void ClickLetter()
    {
        SetInterecation(false);
        OnClick?.Invoke(this);
    }
    
}
