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

        void Start()
        {
            speechRecognizer.OnRecognizedAsObservable()
                .Where(x => x.Contains("回って"))
                .Subscribe(_ =>
                {
                    _animator.SetTrigger("Mawaru");
                    speechRecognizer.StopRecord();
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
                .Do(x => _animator.SetBool("Hearing", !x));
        }
    }
}

