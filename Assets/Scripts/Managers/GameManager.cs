using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager {
    private static ResourcesManager _resourceManager;

    public static ResourcesManager GetResourcesManager()
    {
        if(_resourceManager == null)
        {
            _resourceManager = Resources.Load("ResourcesManager") as ResourcesManager;
        }

        return _resourceManager;
    }
}
