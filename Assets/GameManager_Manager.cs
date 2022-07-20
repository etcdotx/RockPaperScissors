using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Enum;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Random = UnityEngine.Random;

[System.Serializable]
public class c_Event : UnityEvent { 

}


public class GameManager_Manager : MonoBehaviourPunCallbacks,IOnEventCallback{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static GameManager_Manager m_Instance;
    //===== STRUCT =====
    //===== PUBLIC =====
    public e_Mode m_GameMode;
    public e_AIDifficulty m_Level;
    public Player_Gameobject m_Player;
    public c_Event m_OnFinishEvent;
    public Animator m_AnimPlayer;
    public Animator m_AnimEnemy;
    public int m_HostScore = 0;
    public int m_ClientScore = 0;
    //===== PRIVATES =====
    private float m_Timer = 30f;
    private int m_Lives = 3;
    private int m_Score = 0;
    private int m_Highscore = 0;

    private const byte m_WinEvent = 1;
    private const byte m_LoseEvent = 2;
    private const byte m_SendChooseEvent = 3;
    private const byte m_StartShuffling = 4;
    private const byte m_UpdateRoundEvent = 5;
    //=====================================================================
    //				MONOBEHAVIOUR METHOD 
    //=====================================================================
    void Awake(){
        m_Instance = this;
    }

    void Start(){
        m_Highscore = PlayerPrefs.GetInt("HighScore");
    }

    void Update(){
        if (GameLogic_Manager.m_Instance.m_ChooseState != e_ChooseState.NotChoosing)
        {
            if (m_Timer >= 0)
            {
                m_Timer -= Time.deltaTime;
                UI_Manager.m_Instance.m_Timer.fillAmount = m_Timer / 30f;
                if (m_Timer <= 0 && GameLogic_Manager.m_Instance.m_ChooseState != e_ChooseState.NotChoosing)
                {
                    m_Player.f_ChooseState(Random.Range(0, 3));
                }
            }
            UI_Manager.m_Instance.m_ScoreText.text = "Score:" + m_Score.ToString();
            UI_Manager.m_Instance.m_LiveText.text = "Lives:" + m_Lives.ToString();
        }

        UI_Manager.m_Instance.m_Highscore.text = "Highscore: " + m_Highscore.ToString("00");
        UI_Manager.m_Instance.m_EndgameScore.text = m_Score.ToString();
            UI_Manager.m_Instance.m_P1Score.text = m_HostScore.ToString();
            UI_Manager.m_Instance.m_P2Score.text = m_ClientScore.ToString();

    }
    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================
    public void f_CreateRoom() {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.JoinRandomOrCreateRoom(null, 2, MatchmakingMode.FillRoom, null, null, null, roomOptions, null);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            f_GameStart();
            UI_Manager.m_Instance.f_IamP1();
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined");
        if (!PhotonNetwork.IsMasterClient)
        {
            f_GameStart();
            UI_Manager.m_Instance.f_IamP2();
        }
    }
    public void f_SetMode(int p_Mode) {
        m_GameMode = (e_Mode)p_Mode;   
    }
    public void f_GameStart()
    {
        GameLogic_Manager.m_Instance.m_ChooseState = e_ChooseState.Choosing;
        if (m_GameMode == e_Mode.Single)
        {
            m_Lives = 3;
            UI_Manager.m_Instance.m_OnSinglePlayer?.Invoke();
        }
        else
        {
            m_HostScore = 0;
            m_ClientScore = 0;
            UI_Manager.m_Instance.m_OnMultiPlayer?.Invoke();
        }
        UI_Manager.m_Instance.m_OnStart?.Invoke();
    }

    public void f_DecreaseLive() {
        m_Lives--;
        if (m_Lives <= 0)
        {
            UI_Manager.m_Instance.f_GameOver();
            m_GameMode = e_Mode.Lobby;
            if (m_Score > m_Highscore) m_Highscore = m_Score;PlayerPrefs.SetInt("HighScore", m_Score);
        }
        else m_OnFinishEvent?.Invoke();
    }

    public void f_IncreaseScore() {
        m_Score++;
        m_OnFinishEvent?.Invoke();
    }

    public void f_Quit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif    
    }

        public void f_ResetTime() {
        m_Timer = 30f;
    }
    public void f_WinEvent() {
        object[] content = new object[0];
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(m_WinEvent, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void f_LoseEvent() {
        object[] content = new object[0];
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(m_LoseEvent, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void f_UpdateRound() {
        object[] content = new object[] { m_HostScore, m_ClientScore };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(m_UpdateRoundEvent, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void f_ChooseEvent(e_State p_State) {
        object[] content = new object[] { (int)p_State};
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(m_SendChooseEvent, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void f_StartShuffling() {
        object[] content = new object[0];
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(m_StartShuffling, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void f_Win() {
        m_GameMode = e_Mode.Lobby;
        UI_Manager.m_Instance.m_OnWin?.Invoke();
    }

    public void f_Lose() {
        m_GameMode = e_Mode.Lobby;
        UI_Manager.m_Instance.m_OnLose?.Invoke();
    }

    public void OnEvent(EventData p_PhotonEvent) {
        if (p_PhotonEvent.Code == m_LoseEvent)
        {
            f_Lose();
        }
        else if (p_PhotonEvent.Code == m_WinEvent)
        {
            f_Win();
        }
        else if (p_PhotonEvent.Code == m_UpdateRoundEvent)
        {
            object[] data = (object[])p_PhotonEvent.CustomData;
            m_HostScore = (int) data[0];
            m_ClientScore = (int)data[1];
            GameLogic_Manager.m_Instance.m_AfterShuffle?.Invoke();
            if (m_HostScore < 3 && m_ClientScore < 3)
            {
                m_OnFinishEvent?.Invoke();
            }
        }
        else if (p_PhotonEvent.Code == m_SendChooseEvent)
        {
            object[] data = (object[])p_PhotonEvent.CustomData;
            e_State t_State = (e_State) data[0];
            GameLogic_Manager.m_Instance.f_GetPlayerInput(2, t_State);
        }
        else if (p_PhotonEvent.Code == m_StartShuffling) {
            GameLogic_Manager.m_Instance.m_WaitEvent?.Invoke();
            UI_Manager.m_Instance.m_Timer.gameObject.SetActive(false);
        }
    }

}
