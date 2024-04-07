using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InstructionInterpreter
{
    public static OperationType[] Interpreter(string rawString, string up, string left, string right, string upleft, string upright)
    {
        List<OperationType> ops = new();
        for (int i = 0; i < rawString.Length;)
        {
            if (up is not null && up.Length!=0 && i + up.Length <= rawString.Length && rawString.Substring(i, up.Length) == up)
            {
                ops.Add(OperationType.MoveUp);
                i += up.Length;
            }
            else if (left is not null && left.Length!=0 && i + left.Length <= rawString.Length && rawString.Substring(i, left.Length) == left)
            {
                ops.Add(OperationType.MoveLeft);
                i += left.Length;
            }
            else if (right is not null && right.Length!=0 && i + right.Length <= rawString.Length && rawString.Substring(i, right.Length) == right)
            {
                ops.Add(OperationType.MoveRight);
                i += right.Length;
            }
            else if (upleft is not null && upleft.Length!=0 && i + upleft.Length <= rawString.Length && rawString.Substring(i, upleft.Length) == upleft)
            {
                ops.Add(OperationType.MoveLeftUp);
                i += upleft.Length;
            }
            else if (upright is not null && upright.Length!=0 && i + upright.Length <= rawString.Length && rawString.Substring(i, upright.Length) == upright)
            {
                ops.Add(OperationType.MoveRightUp);
                i += upright.Length;
            }
            else
            {
                ops.Add(OperationType.None);
                i++;
            }
        }
        return ops.ToArray();
    }
}
