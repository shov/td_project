using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCamp : Camp
{
    public override string campTag { get { return GameManager.ENEMY_TAG; } }
}
