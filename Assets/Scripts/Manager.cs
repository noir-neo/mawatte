using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Mawatte
{
    public class Manager : MonoBehaviour
    {
        [SerializeField] private SpeechRecognizer speechRecognizer;
        [SerializeField] private Animator _animator;
        [SerializeField] private Renderer _renderer;

        private readonly IReadOnlyDictionary<string, string> keywordTriggerMap = new Dictionary<string, string>
        {
            {"回って" , "Mawaru"},
            {"ユニティーちゃん" , "Tewofuru"},
        };

        void Start()
        {
            speechRecognizer.OnRecognizedAsObservable()
                .Subscribe(x =>
                {
                    var pair = keywordTriggerMap.FirstOrDefault(m => x.Contains(m.Key));
                    if (!pair.Equals(new KeyValuePair<string, string>()))
                    {
                        _animator.SetTrigger(pair.Value);
                        speechRecognizer.StopRecord();
                    }
                })
                .AddTo(this);

            _renderer.OnBecameVisibleAsObservable()
                .SelectMany(_ => StartHearingAsObservable())
                .Subscribe()
                .AddTo(this);

            _renderer.OnBecameInvisibleAsObservable()
                .SelectMany(_ => StopHearingAsObservable())
                .Subscribe()
                .AddTo(this);

            var animatorStateMachine = _animator.GetBehaviour<ObservableStateMachineTrigger>();
            animatorStateMachine.OnStateEnterAsObservable()
                .Where(x => x.StateInfo.IsName("Mawatta"))
                .Where(_ => _renderer.isVisible)
                .SelectMany(_ => StartHearingAsObservable())
                .Subscribe()
                .AddTo(this);

        }

        private IObservable<bool> StartHearingAsObservable()
        {
            return Observable.ReturnUnit()
                .Select(_ => speechRecognizer.StartRecord())
                .Do(x => _animator.SetBool("Hearing", x));
        }

        private IObservable<bool> StopHearingAsObservable()
        {
            return Observable.ReturnUnit()
                .Select(_ => speechRecognizer.StopRecord())
                .Do(_ => _animator.SetBool("Hearing", false));
        }
    }
}

