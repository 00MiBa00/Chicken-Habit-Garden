using Types;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers.Scenes
{
    public class InitSceneController : MonoBehaviour
    {
        private void Awake()
        {
            SceneManager.LoadScene(SceneType.MenuScene.ToString());
        }
    }
}