using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OperationType
{
    //角色移动相关
    MoveLeft,
    MoveRight,
    MoveUp,
    MoveLeftUp,
    MoveRightUp,
    Jump,
    //条移动相关
    Invert,
    Replay,

    //其他
    Die,
    None,

    //弃用
    JumpLeft,
    JumpRight,
}
