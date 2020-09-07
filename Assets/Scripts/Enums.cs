using UnityEngine;
using System.Collections;




/// <summary>
/// 格子的显示状态
/// 每个格子分为白、黑、x这三种状态
/// </summary>
public enum GridFlag
{
    None, White, Black, Negation,
}

/// <summary>
/// 棋盘的操作状态
/// 目前分为Pen和Negation两种状态
/// 这两种状态是互斥的
/// </summary>
public enum OperationState
{
    None, 
    /// <summary>
    /// 笔状态，此状态下，可将格子变为白或黑
    /// </summary>
    Pen, 
    /// <summary>
    /// 否定状态，此状态下，可将格子变为白和x
    /// </summary>
    Negation,
}


public enum ClassicType
{ 
    None,
    Animals,
    Logo,
    Characters,

    Other,
}

public enum GameState
{
    NotGame,
    IsPuzzled,
    Victory,
}