using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Enum{
    public enum e_State { 
        Choosing = -1,
        Rock = 0,
        Paper = 1,
        Scissors = 2,
    }

    public enum e_Mode { 
        Lobby = -1,
        Single = 0,
        Multi =1,
    }

    public enum e_Result { 
        Lose = -1,
        Draw = 0,
        Win = 1
    }

    public enum e_AIDifficulty
    {
        Easy = 0,
        Medium = 1,
        Hard =2,
    }

    public enum e_ChooseState { 
        Choosing = 0,
        NotChoosing =1,
    }
}

