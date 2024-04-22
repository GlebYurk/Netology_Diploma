/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldMode;

public class BasicAtackActionLegacy : IRadiusAction
{
    private float atackDelay;
    private List<GeneralLiveForm> targetList;
    private GeneralLiveForm _actionObject;


    public BasicAtackActionLegacy(GeneralLiveForm generalLiveForm, float atackDelay)
    {
        this.atackDelay = atackDelay;
        this._actionObject = generalLiveForm;
        targetList = new List<GeneralLiveForm>();
    }


    public void OnRadiusAction(GeneralLiveForm OnRadiusLive)
    {
        targetList.Add(OnRadiusLive);

    }
    IEnumerator Atack()
    {
        while (targetList.Count > 0)
        {
            yield return new WaitForSeconds(atackDelay);

            for (int i = 0; i < targetList.Count; i++)
            {
                if (_actionObject.GetAtackType())
                    targetList[i].GetMagDamage(_actionObject.GetAtack(), 0);
                else
                    targetList[i].GetPhysDamage(_actionObject.GetAtack(), 0);
            }
        }

    }

}
*/