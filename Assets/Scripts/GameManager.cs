using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    private SettingLevels settingLevels;

    [SerializeField]
    private CanvasGroup gameCanvasGroup;

    [SerializeField]
    private Image image;
    [SerializeField]
    private Sprite imageSprite;

    [SerializeField]
    private string wordToFind;

    [HideInInspector]
    public List<Word> words;
    [HideInInspector]
    public List<Letter> letters;

    [SerializeField]
    private Transform pointRespawnWord;
    [SerializeField]
    private Word prefabWordElement;

    [SerializeField]
    private Transform pointRespawnLetter;
    [SerializeField]
    private Letter prefabLetterElement;

    [SerializeField]
    private TextMeshProUGUI wrongText;

    [SerializeField]
    private GameObject winPanel;

    [SerializeField]
    private GameObject endGamePanel;

    [SerializeField]
    private CanvasGroup lettersCanvasGroup;

    private Save save;

    [SerializeField]
    private TextMeshProUGUI coinsText;
   
    void Start()
    {

        LoadSaveGame();
        InitGame();
    }
    public void InitGame()
    {
        if(settingLevels.levels.Count > save.Level)
        {
            coinsText.text = save.Coins.ToString();
            InitWord();
            InitLetters();
            SetImage();
        }
        else
        {
            endGamePanel.SetActive(true);
        }
    }
    public void SetImage()
    {
        image.sprite = settingLevels.levels[save.Level].sprite;
    }

    private void InitWord()
    {

        foreach (var word in words)
        {
            word.OnClick -= ClearWord;
            Destroy(word.gameObject);
        }
        words.Clear();

        wordToFind = settingLevels.levels[save.Level].Word.ToUpper();

        for (int i = 0; i < wordToFind.Length; i++)
        {
            Word word = Instantiate(prefabWordElement, pointRespawnWord);
            word.OnClick += ClearWord;
            words.Add(word);
        }
    }
    public void ClearWord(Word w)
    {
        SetWrongState(false);
    }
    private void InitLetters()
    {
        foreach(var letter in letters)
        {
            letter.OnClick -= SetLetter;
            Destroy(letter.gameObject);
        }
        letters.Clear();

        for (int i = 0; i < 12; i++)
        {
            Letter letter = Instantiate(prefabLetterElement, pointRespawnLetter);
            letter.OnClick += SetLetter;
            if (i < wordToFind.Length)
            {
                letter.SetLetter(wordToFind[i]);
            } 
            else
            {
                letter.SetLetter((char)Random.Range(65, 91));
            }
            letters.Add(letter);
        }

        Shuffle();
    }

    private void Shuffle()
    {
        for (int i = 0; i < letters.Count; i++)
        {
            int rand = Random.Range(0, letters.Count);
            char temp = letters[i].letter;
            letters[i].SetLetter(letters[rand].letter);
            letters[rand].SetLetter(temp);
        }
    }

    public void SetLetter(Letter letter)
    {

        foreach (var item in words)
        {
            if (item.IsEmpty)
            {
                item.SetLetter(letter);
                break;
            }
        }
        CheckWinState();
    }
    public void SetWrongState(bool flag)
    {

        lettersCanvasGroup.interactable = !flag;
        wrongText.enabled = flag;
    }
    public void CheckWinState()
    {
        StringBuilder str = new StringBuilder();
        foreach (var item in words) {
            if (item.IsEmpty)
                return;
            str.Append(item.letter.letter);
        }
        if (wordToFind.Equals(str.ToString()))
        {
            save.Level++;
            save.Coins += 50;
            SaveGame();
            SetWinLoseState(true);
        } 
        else
        {
            SetWrongState(true);
        }
    }
    public void SetWinLoseState(bool flag)
    {  
        gameCanvasGroup.interactable = !flag;
        winPanel.SetActive(flag);
    }
    public void LoadNextLevel()
    {
        SetWinLoseState(false);
        InitGame();
    }
    void LoadSaveGame()
    {
        save = new Save();
        if (PlayerPrefs.HasKey("Save"))
        {
            save = JsonUtility.FromJson<Save>(PlayerPrefs.GetString("Save"));
            coinsText.text = save.Coins.ToString();
        }
        else
        {
            save.Level = 0;
            save.Coins = 100;
            coinsText.text = save.Coins.ToString();
            PlayerPrefs.SetString("Save", JsonUtility.ToJson(save));
        }
    }
    public void SaveGame()
    {
        PlayerPrefs.SetString("Save", JsonUtility.ToJson(save));
    }
    public void RestartGame()
    {
        save.Level = 0;
        endGamePanel.SetActive(false);
        InitGame();
    }

    public void Hint()
    {
        if(save.Coins >= 50)
        {
            save.Coins -= 50;
            coinsText.text = save.Coins.ToString();

            for(int i = 0; i < words.Count; i++)
            {
                if(words[i].IsEmpty)
                {
                    foreach(var letter in letters)
                    {
                        if(letter.letter.Equals(wordToFind[i]))
                        {
                            words[i].SetLetter(letter);
                            return;
                        }
                    }
                    
                }
            }
        }
    }
    void Update()
    {

    }
}

public struct Save
{
    public int Level;
    public int Coins;
}