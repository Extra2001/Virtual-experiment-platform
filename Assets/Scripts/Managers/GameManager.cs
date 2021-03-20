using HT.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBehaviorManager<GameManager>
{

    private void Start()
    {
        Main.m_Event.Subscribe<Sitdown>(WhenSitdown);
    }

    private void WhenSitdown(object sender, EventHandlerBase handler)
    {        
        Main.m_Procedure.SwitchProcedure<OnChair>();
    }

}
