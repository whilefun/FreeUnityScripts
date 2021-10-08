using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
// MyLayerHelper
//
// The idea here is cache frequently fetched layermasks for efficiency and consistency. If your LayerMask 
// for SomeLayers needs to change for gameplay rule reasons, just updated here once and every user of that
// LayerMask will be updated for free
//
// To use, call MyLayerHelper.SomeLayers which will give you the layer mask as defined in SomeLayers.
//
public class MyLayerHelper
{

    private static int _someLayers = -1;
    

    public static int SomeLayers {

        get {

            if (_someLayers == -1)
            {
			
                _someLayers = 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("SomeOtherLayer") | 1 << LayerMask.NameToLayer("AndYetAnotherLayer");
            }

            return _someLayers;

        }

    }
}
