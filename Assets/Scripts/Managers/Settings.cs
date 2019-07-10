using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PL
{
    public static class Settings
    {
        private static ResourcesManager _resourceManager;

        public static ResourcesManager GetResourcesManager()
        {
            if (_resourceManager == null)
            {
                _resourceManager = Resources.Load("ResourcesManager") as ResourcesManager;
            }

            return _resourceManager;
        }
    }
}
