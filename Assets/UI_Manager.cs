using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static UI_Manager m_Instance;
    //===== STRUCT =====
    //===== PUBLIC =====
    public c_Event m_OnGameOver;
    public c_Event m_OnWin;
    public c_Event m_OnLose;
    public c_Event m_OnStart;
    public c_Event m_OnSinglePlayer;
    public c_Event m_OnMultiPlayer;
    public GameObject m_PlayerInput;
    public GameObject m_Gameover;
    public GameObject m_Win;
    public GameObject m_Lose;
    public TextMeshProUGUI m_ScoreText;
    public TextMeshProUGUI m_LiveText;
    public TextMeshProUGUI m_P1Text;
    public TextMeshProUGUI m_P2Text;
    public TextMeshProUGUI m_P1Score;
    public TextMeshProUGUI m_P2Score;
    public TextMeshProUGUI m_EndgameScore;
    public TextMeshProUGUI m_Highscore;
    public Image m_Timer;
    //===== PRIVATES =====

    //=====================================================================
    //				MONOBEHAVIOUR METHOD 
    //=====================================================================
    void Awake(){
        m_Instance = this;
    }

    void Start(){
        
    }

    void Update(){
        
    }
    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================
    public void f_ResetPlayerInput() {  
        m_PlayerInput.SetActive(true);
    }

    public void f_GameOver() {
        m_OnGameOver?.Invoke();
    }
    public void f_IamP1() {
        m_P1Text.text += "(You)";
    }

    public void f_IamP2()
    {
        m_P2Text.text += "(You)";
    }

}
