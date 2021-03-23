using System;
using System.Collections.Generic;
using System.Text;


public enum Table
{
    account,
    playerinfo,
    playerstat,
    exp,
    monsterdrop,
}

public enum Chat
{
    Connect = 0,
    Chat = 1,
}

public enum Database
{
    NONE,
    Player,
    GameData
}

public enum Data
{
    idx,
    gold,
    diamond,
    name,
    login,
    exp,
    level,
    hp,
    maxhp,
    dmg,
    defense,
    atkTime,
    critical,
    criticaldmg,
}
public enum SendType
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

public enum Stat
{
    idx,
    hp,
    maxhp,
    dmg,
    defense,
    atktime,
    critical,
    criticaldmg,
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