using UniRx;
using UniSpeech;
using UnityEngine;

namespace Mawatte
{
    public class SpeechRecognizer : MonoBehaviour, ISpeechRecognizer
    {
        private readonly ISubject<string> onRecognizedSubject = new Subject<string>();

        void Start()
        {
            UniSpeech.SpeechRecognizer.CallbackGameObjectName = gameObject.name;
            UniSpeech.SpeechRecognizer.RequestRecognizerAuthorization();
        }

        public IObservable<string> OnRecognizedAsObservable()
        {
            return onRecognizedSubject;
        }

        public void OnRecognized(string transcription)
        {
            Debug.Log("OnRecognized: " + transcription);
            onRecognizedSubject.OnNext(transcription);
        }

        public void OnError(string description)
        {
            Debug.Log("OnError: " + description);
        }

        public void OnAuthorized()
        {
            Debug.Log("OnAuthorized");
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

        public bool StartRecord()
        {
            return UniSpeech.SpeechRecognizer.StartRecord();
        }

        public bool StopRecord()
        {
            return UniSpeech.SpeechRecognizer.StopRecord();
        }
    }
}

