using System;
using System.Threading.Tasks;
using UnityEngine;
using Utils.DI.Config;

namespace Loading
{
    public interface ILoadingEntity
    {
        Action Loaded { get; set; }
    }
}