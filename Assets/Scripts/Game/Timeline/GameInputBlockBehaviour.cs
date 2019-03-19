using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityMMO;

// A behaviour that is attached to a playable
public class GameInputBlockBehaviour : PlayableBehaviour
{
    public bool IsBlock=false;
    private bool lastBlockState;
    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {
        
    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable)
    {
       
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        lastBlockState = GameInput.GetInstance().IsBlock;
        Debug.Log("IsBlock : "+IsBlock.ToString()+" time"+Time.time.ToString());
        GameInput.GetInstance().IsBlock = IsBlock;
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        Debug.Log("lastBlockState : "+lastBlockState.ToString()+" time"+Time.time.ToString());
        GameInput.GetInstance().IsBlock = lastBlockState;
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        
    }
}
