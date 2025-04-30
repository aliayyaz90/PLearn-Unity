using UnityEngine;
using UnityEngine.UI;

namespace ToastWrapper
{
	public class Toast : MonoBehaviour
	{
		//public static Toast instance;

		private Text _text;
		private CanvasGroup _canvasGroup;

		private void Awake()
		{
            //	if ((instance != null) && (instance != this))
            //	{
            //		Destroy(gameObject);
            //	}
            //	else
            //	{
            //		instance = this;
            //		DontDestroyOnLoad(gameObject);
            //	}
            _canvasGroup = GetComponent<CanvasGroup>();
            _text = GetComponentInChildren<Text>();
            _canvasGroup.alpha = 0;
        }
        private void Start()
        {
			
        }

        public void Show(string message, float hideDelay)
		{
			_text.text = message;
			_canvasGroup.alpha = 1;
			CancelInvoke(nameof(Hide));
			Invoke(nameof(Hide), hideDelay);
		}

        private void Hide()
		{
			_canvasGroup.alpha = 0;
		}
	}
}