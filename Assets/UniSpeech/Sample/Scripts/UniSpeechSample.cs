using UnityEngine;

namespace UniSpeech.Sample
{
    public class UniSpeechSample : MonoBehaviour, ISpeechRecognizer
    {
        void Start()
        {
            SpeechRecognizer.CallbackGameObjectName = gameObject.name;
            SpeechRecognizer.RequestRecognizerAuthorization();
        }

        public void OnRecognized(string transcription)
        {
            Debug.Log("OnRecognized: " + transcription);
        }

        public void OnError(string description)
        {
            Debug.Log("OnError: " + description);
        }

        public void OnAuthorized()
        {
            Debug.Log("OnAuthorized");
            SpeechRecognizer.StartRecord();
        }

        public void OnUnauthorized()
        {
            Debug.Log("OnUnauthorized");
        }

        public void OnAvailable()
        {
            Debug.Log("OnAvailable");
        }

        public void OnUnavailable()
        {
            Debug.Log("OnUnavailable");
        }
    }
}

