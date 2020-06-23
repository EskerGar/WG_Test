using System.Linq;
using UnityEngine;

public interface IState
{
    void StateLogic();
    void StateExit();
    void StateEnter();
    
}