using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamp : Camp
{
    public override string campTag { get { return GameManager.PLAYER_TAG; } }
}
