using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hat : GameUnit
{
    public void SetHatParent(Transform hatPos) {
        TF.SetParent(hatPos);
    }

}
