using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INode
{
    void SetChild(INode node);
    void TrySetParent(INode node);
}
