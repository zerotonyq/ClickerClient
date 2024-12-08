﻿using System.Threading.Tasks;
using EventBus.Subscribers.Base;

namespace EventBus.Subscribers.Lobbies
{
    public interface IGetLobbiesSubscriber : IGlobalSubscriber
    {
        Task HandleGetLobbies();
    }
}