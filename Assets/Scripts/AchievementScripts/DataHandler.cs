using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;
using UnityEngine.SceneManagement;


public class DataHandler : MonoBehaviour
{
    public GameObject AllDonePanel;
    public GameObject CompletedPanel;
    public Animator CompletedAnim;
    public Text CompletedTxt;
    public Text SetText;

    //For Animations
    int ResetHashId;
    int OpenHashId;
    int CloseHashId;

    //UI elements in LIST
    public List<Text> TitleTxt = new List<Text>();
    public List<Text> DoneTxt = new List<Text>();
    public List<Text> TargetTxt = new List<Text>();

    //Json DATA variables
    public Player playData = new Player();
    public string JsonString;
    public JsonData JsonDatas;
    
    //Delegates
    public delegate void JumpDelegate(bool Val);
    public delegate void AttackDelegate(bool Val);
    public delegate void LevelDelegate(bool Val);
    public delegate void DieDelegate(bool Val);
    public delegate void KillDelegate(bool Val);

    //events
    public event JumpDelegate JumpEvent;
    public event AttackDelegate AttackEvent;
    public event LevelDelegate LevelEvent;
    public event DieDelegate DieEvent;
    public event KillDelegate KillEvent;

    // JumpEvent += new JumpDelegate(JumpMethod);

    public int JumpTarget;
    public int LevelTarget;
    public int AttackTarget;
    public int DieTarget;
    public int KillTarget;

    //
    public int JumpId;
    public int LevelId;
    public int AttackId;
    public int DieId;
    public int KillId;

    public bool JumpBool;
    public bool LevelBool;
    public bool AttackBool;
    public bool DieBool;
    public bool KillBool;
    //
    public int Counter;

    //
    void Start()
    {
        JumpBool = false;
        LevelBool = false;
        AttackBool = false;
        DieBool = false;
        KillBool = false;

        ResetHashId = Animator.StringToHash("Reset");
        OpenHashId = Animator.StringToHash("Open");
        CloseHashId = Animator.StringToHash("Close");

        //
        //Data
        JsonString = File.ReadAllText(Application.dataPath + "/Resources/Text/GameData.json");
        JsonToObject();
        //ObjectToJson();

        FindAchieve();

        //
    }

    //
    //deserialization
    public void JsonToObject()
    {
        playData = JsonMapper.ToObject<Player>(JsonString);
        Debug.Log("===> " + playData.achievements.Count.ToString());
    }
    
    //serialization
    public void ObjectToJson()
    {
        JsonDatas = JsonMapper.ToJson(playData);
        File.WriteAllText(Application.dataPath + "/Resources/Text/GameData.json", JsonDatas.ToString());
    }

    public void FindAchieve()
    {
        Counter = 0;
        int TrueVal = 0;//ARE ALL 5 TRUE ?
        int No = 0;

        for (int i = 0; i < playData.achievements.Count; i++)
        {
            No++;

            if (No <= 5)
            {
                if (playData.achievements[i].State == false)
                {
                    Counter++;
                    SetAchieves(Counter);
                    break;
                }
                else
                {
                    TrueVal++;
                    if(TrueVal == 5)
                    {
                        Counter++;
                        TrueVal = 0;
                    }
                }

                if(No == 5)
                {
                    No = 0;
                }
            }
        }
    }


    public void SetAchieves(int Num)//Setting Achieves in UI
    {
        int j = 0;
        int EndVal = (Num * 5) - 1;//(1*3)-1 => 2
        int InitVal = EndVal - 4;//2-2=0

        Debug.Log(InitVal + "  " + EndVal);
        
        for(int i = InitVal; i <= EndVal; i++)
        {

            Debug.Log("SET_ACHIEVE " + i);
            TitleTxt[j].text = "" + playData.achievements[i].Title;
            TargetTxt[j].text = "" + playData.achievements[i].Target;
            DoneTxt[j].text = "" + playData.achievements[i].Done;


            if (playData.achievements[i].State == false)
            {
                SubscribeMethod(playData.achievements[i].Type, i);
            }

            j++;
        }

        //
        SetText.text = "SET " + Counter.ToString();
    }

    //
    public void LoadAchievements()
    {
        LoadAchieves(Counter);
    }

    public void LoadAchieves(int Num)
    {
        int j = 0;
        int EndVal = (Num * 5) - 1;//(1*3)-1 => 2
        int InitVal = EndVal - 4;//2-2=0

        Debug.Log(InitVal + "  " + EndVal);

        for (int i = InitVal; i <= EndVal; i++)
        {

            TitleTxt[j].text = "" + playData.achievements[i].Title;
            TargetTxt[j].text = "" + playData.achievements[i].Target;
            DoneTxt[j].text = "" + playData.achievements[i].Done;

            j++;
        }
    }


    
    //
    public void SubscribeMethod(string AchieveType,int Id)
    {
        switch (AchieveType)
        {
            case "Jump":

                if (playData.achievements[Id].State == false)
                {
                    JumpEvent += new JumpDelegate(JumpMethod);
                    JumpBool = true;
                    JumpId = Id;
                    JumpTarget = playData.achievements[Id].Target;

                }
                break;
            case "Level":
                if (playData.achievements[Id].State == false)
                {
                    LevelEvent += new LevelDelegate(LevelMethod);
                    LevelBool = true;
                    LevelId = Id;
                    playData.achievements[LevelId].Done = playData.level;
                    LevelTarget = playData.achievements[Id].Target;
                }
                break;
            case "Attack":
                if (playData.achievements[Id].State == false)
                {
                    AttackEvent += new AttackDelegate(AttackMethod);
                    AttackBool = true;
                    AttackId = Id;
                    AttackTarget = playData.achievements[Id].Target;
                }
                break;
            case "Die":
                if (playData.achievements[Id].State == false)
                {
                    DieEvent += new DieDelegate(DieMethod);
                    DieBool = true;
                    DieId = Id;
                    DieTarget = playData.achievements[Id].Target;
                }
                break;
            case "Kill":
                if (playData.achievements[Id].State == false)
                {
                    KillEvent += new KillDelegate(KillMethod);
                    KillBool = true;
                    KillId = Id;
                    KillTarget = playData.achievements[Id].Target;
                }
                break;
        }
    }

    public void JumpMethod(bool Val)
    {
        Debug.Log("JUMP FROM METHOD" + Val);
        playData.achievements[JumpId].State = Val;
        JumpEvent -= JumpMethod;
        JumpBool = false;
        DoneAnim(JumpId);
    }

    public void LevelMethod(bool Val)
    {
        Debug.Log("LEVEL FROM METHOD" + Val);
        playData.achievements[LevelId].State = Val;
        LevelEvent -= LevelMethod;
        LevelBool = false;
        DoneAnim(LevelId);
    }

    public void AttackMethod(bool Val)
    {
        Debug.Log("ATTACK FROM METHOD" + Val);
        playData.achievements[AttackId].State = Val;
        AttackEvent -= AttackMethod;
        AttackBool = false;
        DoneAnim(AttackId);
    }

    public void DieMethod(bool Val)
    {
        Debug.Log("DIE FROM METHOD" + Val);
        playData.achievements[DieId].State = Val;
        DieEvent -= DieMethod;
        DieBool = false;
        DoneAnim(DieId);
    }

    public void KillMethod(bool Val)
    {
        Debug.Log("KILL FROM METHOD" + Val);
        playData.achievements[KillId].State = Val;
        KillEvent -= KillMethod;
        KillBool = false;
        DoneAnim(KillId);
    }


    //Firing events
    public void FireJump(bool val)
    {
        Debug.Log("JUMP EVENT");
        JumpEvent(val);
    }

    public void FireAttack(bool Val)
    {
        AttackEvent(Val);
    }

    public void FireLevel(bool Val)
    {
        LevelEvent(Val);
    }

    public void FireDie(bool Val)
    {
        DieEvent(Val);
    }

    public void FireKill(bool Val)
    {
        KillEvent(Val);
    }


    //
    //ACHIEVEMENTS COMPLETED ANIMATION
    //
    public void DoneAnim(int Id)
    {
        CompletedTxt.text = "" + playData.achievements[Id].Title;
        CompletedPanel.SetActive(true);
        CompletedAnim.SetTrigger(OpenHashId);
        //Invoke("SetBackAnim", 3f);
        AchievesChecking();
    }

    //
    //ACHIEVE DISPLAY CLOSE BUTTON
    public void SetBackAnim()
    {
        CompletedAnim.SetTrigger(CloseHashId);
        Invoke("ResetMethod", 2f);
    }

    public void ResetMethod()
    {
        CompletedPanel.SetActive(false);
        CompletedAnim.SetTrigger(CloseHashId);
    }
    //


    public void AchievesChecking()
    {
        int Value = 0;

        if (playData.achievements[JumpId].State == true)
        {
            Value++;
        }

        if (playData.achievements[LevelId].State == true)
        {
            Value++;
        }

        if (playData.achievements[AttackId].State == true)
        {
            Value++;
        }

        if (playData.achievements[DieId].State == true)
        {
            Value++;
        }

        if (playData.achievements[KillId].State == true)
        {
            Value++;
        }

        if (Value == 5)
        {
            int Valid = Counter * 5;
            if(Valid < playData.achievements.Count)
            {
                Counter++;
                SetAchieves(Counter);

            }else if(Valid == playData.achievements.Count)
            {
                //Debug.LogError("DONE ALL ACHIEVEMENTS");
                AllDonePanel.SetActive(true);
            }
        }
    }


    public void RetryMethod()
    {

        ResetAchieves();
        Invoke("LoadScene", 1f);
    }

    public void ResetAchieves()
    {
        playData.level = 0;
        int N = playData.achievements.Count;
        for(int i=0; i < N; i++)
        {
            playData.achievements[i].State = false;
            playData.achievements[i].Done = 0;
        }

        ObjectToJson();
    }


    public void LoadScene()
    {
        SceneManager.LoadScene(0);
    }

}


public class Player
{
    public string name;
    public int health;
    public int level;
    public List<Achieve> achievements = new List<Achieve>();
}

public class Achieve
{
    public bool State;
    public string Type;
    public string Title;
    public int Target;
    public int Done;
}


