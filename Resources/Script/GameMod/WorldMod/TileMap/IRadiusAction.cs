using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldMode;

interface IRadiusAction
{
    void OnRadiusEnterAction(BasicAtackAction OnRadiusLive);
    void OnRadiusExitAction(BasicAtackAction OnRadiusLive);
}
