using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App
{
    public interface IState
    {
        public void Enter();

        public void Update();

        public void Exit();

    }

}
