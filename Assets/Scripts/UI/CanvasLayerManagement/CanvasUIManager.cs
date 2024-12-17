using System;
using System.Collections.Generic;
using UI.Base;
using UI.Controllers.AdminUIController;
using UI.Controllers.AuthUIController;
using UI.Controllers.GameTitleUIController;
using UI.Controllers.LobbiesUIController;
using UI.Controllers.NotificationsUIController;
using UI.Controllers.PointsUIController;
using UI.Controllers.SprintUIController;
using Zenject;

namespace UI.CanvasLayerManagement
{
    public class CanvasUIManager
    {
        public static Dictionary<Type, int> Layers = new()
        {
            {typeof(AdminUIController), 0},   
            {typeof(AuthUIController), 10},   
            {typeof(GameTitleUIController), 0},   
            {typeof(LobbiesUIController), 0},   
            {typeof(NotificationsUIController), 100},
            {typeof(PointsUIController), 100},
            {typeof(SprintUIController), 100}
        };
    }
}