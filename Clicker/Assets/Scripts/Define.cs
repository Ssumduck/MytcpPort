using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public static string IPADDRESS = "49.163.25.60";

    public enum Table
    {
        account,
        playerinfo,
        playerstat,
        exp,
        monsterdrop,
    }

    public enum Port
    {
        Login = 1666,
        Data = 1667,
        Chat = 1668,
    }
    public enum LoginSendType
    {
        Login = 0,
        Register = 1,
        Logout = 2,
    }

    public enum NetworkState
    {
        Success = 0,
        Error = 1,
    }


    public enum Chat
    {
        Connect,
        Chat,
    }

    public enum Data
    {
        idx,
        gold,
        diamond,
        name,
        level,
        exp,
    }

    public enum Database
    {
        NONE,
        Player,
        GameData,
    }

    public enum ItemType
    {
        Weapon = 0,
        Armor,
        Helmat,
        Glove,
        Use,
        ETC,
        Fragment,
    }

    public enum StatType
    {
        Player,
        Monster,
    }

    public enum State
    {
        Idle,
        Move,
        Attack,
        Skill,
        Hit,
        Die,
        Win,
    }
}
