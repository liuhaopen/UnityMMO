using System.Collections.Generic;
using UnityEngine;
using UnityMMO.Component;

namespace UnityMMO
{    
    public class LocomotionStateStack : MonoBehaviour 
    {
        public Stack<LocomotionState> Stack;
        public LocomotionStateStack()
        {
            Stack = new Stack<LocomotionState>();
        }
    }
}