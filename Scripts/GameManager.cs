using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text description;
    public int buttonID;
    public Animator TextAnim;
    public GameObject[] button;
    public GameObject Aid;
    public Sprite[] Icon;
    public Sprite[] SoulSprite;
    public SpriteRenderer[] LeftOrRightIcon;
    public SpriteRenderer Soul;
    public int[] LeftAndRightEvent;
    public int[] BlassedID;
    public int[] CursedID;
    public int[] SacrificeID;
    public int[] AidID;
    public int[] QuestID;
    public bool Win;
    int PriseForQuest;
    int PermMove;
    int Cave;
    int MaxFreshness = 4;
    int AidBuf;
    public int LeftMove;
    public int DownMove;
    bool CaveShow;
    List<int> Quest;

    int SacrificeCount;
    TextAsset txtAsset;
    List<Action> AllID;
    List<Action> RandomID;
    List<MovePerTurn> MPT;
    int number;
    int freshness;
    int debufMove;
    int totalMove;
    int TotalMove
    {
        get
        {
            if (PermMove != 0)
            {
                return PermMove;
            }
            else
                return totalMove;
        }
        set
        {
            totalMove = value + debufMove;
            if (totalMove < 1)
            {
                totalMove = 1;
            }
            else if (totalMove > 3)
            {
                totalMove = 3;
            }
        }
    }
    public int Freshness
    {
        get
        {
            return freshness;
        }
        set
        {
            if (value <= 0)
            {
                Soul.sprite = null;
                freshness = 0;
            }
            else if (value > MaxFreshness)
            {
                freshness = MaxFreshness;
            }
            else
            {
                freshness = value;
                if (freshness >= 4)
                {
                    Soul.sprite = SoulSprite[3];
                }
                else
                {
                    Soul.sprite = SoulSprite[freshness - 1];
                }

            }
        }
    }
    void Start()
    {
        txtAsset = (TextAsset)Resources.Load(("Action"), typeof(TextAsset));
        RandomID = new List<Action>();
        AllID = new List<Action>();
        Quest = new List<int>();
        MPT = new List<MovePerTurn>();
        Freshness = 4;
        ReadAction();
        ShowText();
        setButton();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))  // если нажата клавиша Esc (Escape)
        {
            Application.Quit();
        }
        else if (Input.GetKeyDown(KeyCode.A))  // если нажата клавиша A
        {
            if (button[1].activeSelf)
            {
                button[1].GetComponent<Button>().Press();
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))  // если нажата клавиша S
        {
            if (button[0].activeSelf)
            {
                button[0].GetComponent<Button>().Press();
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))  // если нажата клавиша D
        {
            if (button[2].activeSelf)
            {
                button[2].GetComponent<Button>().Press();
            }

        }
    }
    void ReadAction()
    {
        Action CJ; //
        for (int i = 1; ; i++)
        {
            string CardChildrenJson = JsonHelper<Action>.GetJsonObject(txtAsset.text, i + "_action"); //Спаршенный json

            CJ = JsonUtility.FromJson<Action>(CardChildrenJson);
            if (CJ != null)
            {
                AllID.Add(CJ);
                if (!CJ.Random)
                {
                    RandomID.Add(CJ);
                }

            }
            else
            {
                break;
            }
        } //Перебор карт лагеря
    }
    public void ShowText()
    {
        for (int i = 0; i < MPT.Count; i++)
        {
            MPT[i].Turn--;
            if (MPT[i].Turn <= 0)
            {
                debufMove -= MPT[i].Move;
                MPT.RemoveAt(i);
            }
        }
        number = Random.Range(0, RandomID.Count);
        TextAnim.Play("Show");

    }
    public void ShowUIText()
    {
        if (Quest.Count != 0)
        {
            if (Quest[0] == buttonID)
            {
                Debug.Log("Квест активен");
                Quest.RemoveAt(0);
                if (Quest.Count == 0)
                {
                    Debug.Log("Квест сдан");
                    description.text = AllID[PriseForQuest - 1].Description;
                    ChangeParameter();
                    return;
                }
            }
            else
            {
                Debug.Log("Квест Проваен");
                Quest.Clear();
            }
        }

        if (buttonID == 2 && LeftAndRightEvent[0] != 0)
        {
            SpecialId(LeftAndRightEvent[0]);
        }
        else if (buttonID == 3 && LeftAndRightEvent[1] != 0)
        {
            SpecialId(LeftAndRightEvent[1]);
        }
        else
        {
            description.text = RandomID[number].Description;
            ChangeParameter();
            RandomID.RemoveAt(number);
            if (RandomID.Count == 0)
            {
                ReadAction();
            }
        }
        buttonID = 0;

    }
    public void setButton()
    {
        LeftOrRightIcon[0].sprite = null;
        LeftOrRightIcon[1].sprite = null;
        LeftAndRightEvent[0] = 0;
        LeftAndRightEvent[1] = 0;
        bool QuestButton = false;
        int NumberOfNumbers = Random.Range(1, 4);
        TotalMove = NumberOfNumbers;
        for (int i = 0; i < TotalMove; i++)
        {
            List<int> NumberButton = new List<int> { 0, 1, 2 };
            if (Quest.Count != 0 && !QuestButton)
            {
                button[Quest[0] - 1].SetActive(true);
                NumberButton.Remove(Quest[0]);
                QuestButton = true;
            }
            else
            {
                int DeleteButton = Random.Range(0, NumberButton.Count);
                button[NumberButton[DeleteButton]].SetActive(true);
                NumberButton.RemoveAt(DeleteButton);
            }

            if (button[1].activeSelf)
            {
                if (Random.Range(0, 100) <= 25 + Cave)
                {
                    int z = Random.Range(0, Icon.Length - 1);

                    if (Random.Range(0, 101) <= 1 + AidBuf)
                    {
                        LeftAndRightEvent[0] = 5;
                        z = 4;
                    }
                    else
                    {
                        LeftAndRightEvent[0] = z + 1;
                    }
                    if (!CaveShow)
                    {
                        LeftOrRightIcon[0].sprite = Icon[z];
                    }
                }
            }
            if (button[2].activeSelf)
            {
                if (Random.Range(0, 100) <= 25 + Cave)
                {
                    int z = Random.Range(0, Icon.Length - 1);
                    if (Random.Range(0, 101) <= 1 + AidBuf)
                    {
                        LeftAndRightEvent[1] = 5;
                        z = 4;
                    }
                    else
                    {
                        LeftAndRightEvent[1] = z + 1;
                    }

                    if (!CaveShow)
                    {
                        LeftOrRightIcon[1].sprite = Icon[z];
                    }
                }
            }
        }
    }
    void SpecialId(int LeftAndRightEvent)
    {
        int[] CurrentID = new int[0];
        switch (LeftAndRightEvent)
        {
            case 1:
                CurrentID = BlassedID;
                break;
            case 2:
                CurrentID = CursedID;
                break;
            case 3:
                CurrentID = SacrificeID;
                break;
            case 4:
                CurrentID = QuestID;
                break;
            case 5:
                CurrentID = AidID;
                break;
        }

        if (LeftAndRightEvent == 3)
        {
            SacrificeCount++;
            description.text = AllID[CurrentID[SacrificeCount - 1] - 1].Description;
            ChangeParameter(CurrentID[SacrificeCount - 1] - 1);
        }
        else if (LeftAndRightEvent == 5)
        {
            int r = Random.Range(0, CurrentID.Length);
            description.text = AllID[CurrentID[r] - 1].Description;
            Aid.SetActive(true);
            Win = true;
        }
        else
        {
            int r = Random.Range(0, CurrentID.Length);
            description.text = AllID[CurrentID[r] - 1].Description;
            ChangeParameter(CurrentID[r] - 1);
        }

    }
    void ChangeParameter()
    {
        if (RandomID[number].Freshness != 0)
        {
            Freshness += RandomID[number].Freshness;
        }
        if (RandomID[number].Move != 0 && RandomID[number].Turn != 0)
        {
            MPT.Add(new MovePerTurn(RandomID[number].Turn, RandomID[number].Move));
            debufMove += RandomID[number].Move;
        }
        if (RandomID[number].F != 0)
        {
            Quest.Clear();

            Quest.Add(RandomID[number].F);
            Quest.Add(RandomID[number].S);
            Quest.Add(RandomID[number].T);
            Debug.Log("/" + Quest.Count);
        }
        if (RandomID[number].Cave != 0)
        {
            Cave += RandomID[number].Cave;
        }
        if (RandomID[number].AidBuf != 0)
        {
            AidBuf += RandomID[number].AidBuf;
        }
        if (RandomID[number].PermMove != 0)
        {
            PermMove += RandomID[number].PermMove;
        }
        if (RandomID[number].MaxFreshness != 0)
        {
            MaxFreshness += RandomID[number].MaxFreshness;
        }
        if (RandomID[number].DownMove != 0)
        {
            DownMove += RandomID[number].DownMove;
        }
        if (RandomID[number].LeftMove != 0)
        {
            LeftMove += RandomID[number].LeftMove;
        }
        if (RandomID[number].CaveShow != false)
        {
            CaveShow = true;
        }
    }
    void ChangeParameter(int z)
    {
        if (AllID[z].Freshness != 0)
        {
            Freshness += AllID[z].Freshness;
        }
        if (AllID[z].Move != 0 && AllID[z].Turn != 0)
        {
            MPT.Add(new MovePerTurn(AllID[z].Turn, AllID[z].Move));
            debufMove += AllID[z].Move;
        }
        if (AllID[z].F != 0)
        {
            Quest.Clear();

            Quest.Add(AllID[z].F);
            Quest.Add(AllID[z].S);
            Quest.Add(AllID[z].T);
            PriseForQuest = AllID[z].Prise;
            Debug.Log("/" + Quest.Count);
        }
        if (AllID[z].Cave != 0)
        {
            Cave += AllID[z].Cave;
        }
        if (AllID[z].AidBuf != 0)
        {
            AidBuf += AllID[z].AidBuf;
        }
        if (AllID[z].PermMove != 0)
        {
            PermMove += AllID[z].PermMove;
        }
        if (AllID[z].MaxFreshness != 0)
        {
            MaxFreshness += AllID[z].MaxFreshness;
        }
        if (AllID[z].DownMove != 0)
        {
            DownMove += AllID[z].DownMove;
        }
        if (AllID[z].LeftMove != 0)
        {
            LeftMove += AllID[z].LeftMove;
        }
        if (AllID[z].CaveShow != false)
        {
            CaveShow = true;
        }
    }
}
class Action
{
    public string Description;
    public bool Random;
    public int F;
    public int S;
    public int T;
    public int Freshness;
    public int Turn;
    public int Move;
    public int Prise;
    public int Cave;
    public int AidBuf;
    public int PermMove;
    public int MaxFreshness;
    public int LeftMove;
    public int DownMove;
    public bool CaveShow;
}
class MovePerTurn
{
    public MovePerTurn(int turn, int move)
    {
        Turn = turn;
        Move = move;
    }
    public int Turn;
    public int Move;
}