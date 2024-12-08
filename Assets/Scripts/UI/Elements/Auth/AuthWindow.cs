using System;
using System.Threading.Tasks;
using EventBus.Subscribers.MenuUI.Auth;
using TMPro;
using UnityEngine;

namespace UI.Elements
{
    [RequireComponent(typeof(Canvas))]
    public class AuthWindow : MonoBehaviour, IAuthSuccessfullySubscriber, IDisposable
    {
        [SerializeField] private bool registrationWindow;
        [field: SerializeField] public Canvas Canvas { get; private set; }
        [field: SerializeField] public TMP_InputField UsernameInput { get; private set; }
        [field: SerializeField] public TMP_InputField PasswordInput { get; private set; }
        [field: SerializeField] public TextMeshProUGUI MessageTMP { get; private set; }
        
        [field: SerializeField] public TextMeshProUGUI Header { get; private set; }
        
        [SerializeField] private SimpleAnimatedButton toggleAuthType;

        [SerializeField] private SimpleAnimatedButton submitAnimatedButton;

        private void OnValidate() => Canvas = GetComponent<Canvas>();

        public void Start()
        {
            EventBus.EventBus.SubscribeToEvent<IAuthSuccessfullySubscriber>(this);

            submitAnimatedButton.OnClick.AddListener(() =>
            {
               if(!ValidateInputs())
                   return; 
               
               RequestAuth();
            });
            
            toggleAuthType.OnClick.AddListener(ChangeAuthType);
        }

        private void ChangeAuthType()
        {
            registrationWindow = !registrationWindow;
            //submitAnimatedButton = registrationWindow ? "" TODO update text on button
            Header.text = registrationWindow ? "Регистрация" : "Вход";
        }

        private bool ValidateInputs()
        {
            return !string.IsNullOrEmpty(UsernameInput.text) && !string.IsNullOrEmpty(PasswordInput.text);
        }

        private void RequestAuth()
        {
            if(registrationWindow)
            {
                EventBus.EventBus.RaiseEvent<ISignUpRequestedSubscriber>(sub =>
                    sub.Handle(UsernameInput.text, PasswordInput.text));
            }
            else
            {
                EventBus.EventBus.RaiseEvent<ISignInRequestedSubscriber>(sub =>
                    sub.Handle(UsernameInput.text, PasswordInput.text));
            }
        }

        private void Toggle(bool i) => gameObject.SetActive(i);

        public void Dispose() => submitAnimatedButton.OnClick.RemoveAllListeners();

        private void OnDestroy()
        {
            submitAnimatedButton.OnClick.RemoveAllListeners();
            EventBus.EventBus.UnsubscribeFromEvent<IAuthSuccessfullySubscriber>(this);
        }

        public async Task HandleAuthSuccess(AuthResult authResult)
        {
            if (authResult.Success)
                Toggle(false);

            MessageTMP.text = authResult.Message;
        }
    }
}