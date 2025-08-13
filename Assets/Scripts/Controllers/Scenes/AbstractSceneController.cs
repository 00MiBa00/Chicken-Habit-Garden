using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Values;

namespace Controllers.Scenes
{
    public abstract class AbstractSceneController : MonoBehaviour
    {
        [SerializeField] 
        private Text _balanceText;
        /*[SerializeField] 
        private SoundsController _soundsController;
        [SerializeField]
        private SceneSounds _sceneSounds;

        private MusicController _musicController;*/

        private void OnEnable()
        {   
            //_musicController = GameObject.FindGameObjectWithTag("Music")?.GetComponent<MusicController>();
            
            //_sceneSounds.SetAudioClip();
            
            Initialize();
            Subscribe();
            OnSceneEnable();
            
            UpdateBalanceText();

            Wallet.OnChangedMoney += UpdateBalanceText;
        }

        private void Start()
        {
            //PlayMusic();
            OnSceneStart();
        }

        private void OnDisable()
        {   
            Unsubscribe();
            OnSceneDisable();
            
            Wallet.OnChangedMoney -= UpdateBalanceText;
        }

        protected abstract void OnSceneEnable();
        protected abstract void OnSceneStart();
        protected abstract void OnSceneDisable();
        protected abstract void Initialize();
        protected abstract void Subscribe();
        protected abstract void Unsubscribe();

        protected void LoadScene(string sceneName)
        {
            SetClickClip();
            
            StartCoroutine(DelayLoadScene(sceneName));
        }

        protected void SetClickClip()
        {
            //PlaySound(AudioNames.ClickClip.ToString());
        }

        /*protected AudioClip GetAudioClip(string clipName)
        {
            return _sceneSounds.GetAudioClipByName(clipName);
        }*/

        /*protected void PlaySound(string clipName)
        {
            AudioClip clip = GetAudioClip(clipName);
            
           _soundsController.TryPlaySound(clip);
        }*/

        /*protected void PlayMusic()
        {
            string clipName = SceneManager.GetActiveScene().name == NameScenes.Game.ToString()
                ? AudioNames.GameClip.ToString()
                : AudioNames.MenuClip.ToString();

            _musicController.TryPlayMusic(GetAudioClip(clipName));
        }*/

        private void UpdateBalanceText()
        {
            if (_balanceText == null)
            {
                return;
            }

            _balanceText.text = Wallet.Money.ToString();
        }

        private IEnumerator DelayLoadScene(string sceneName)
        {
            yield return new WaitForSecondsRealtime(0.3f);

            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }

            SceneManager.LoadScene(sceneName);
        }
    }
}