﻿using System.Threading.Tasks;
using EventBus.Subscribers.Base;

namespace EventBus.Subscribers.MenuUI.Auth
{
    public interface IAuthSuccessfullySubscriber : IGlobalSubscriber
    {
        Task HandleAuthSuccess(AuthResult result);
    }

    public struct AuthResult
    {
        public bool Success;
        public string Message;
    }
}