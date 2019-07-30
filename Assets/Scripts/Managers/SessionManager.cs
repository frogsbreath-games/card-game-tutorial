using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PL
{
    public class SessionManager : MonoBehaviour
    {
        public static SessionManager Singleton;
        public delegate void OnSceneLoaded();
        public OnSceneLoaded onSceneLoaded;

        private void Awake()
        {
            if(Singleton == null){
                Singleton = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        public void LoadGameLevel()
        {
            StartCoroutine("Scene1");
        }

        public void LoadMenu()
        {
            StartCoroutine("Menu");
        }

        IEnumerable LoadLevel(string level)
        {
            yield return SceneManager.LoadSceneAsync(level, LoadSceneMode.Single);
            if(onSceneLoaded != null)
            {
                onSceneLoaded();
                onSceneLoaded = null;
            }
        }
    }
}
