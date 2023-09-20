using System;
using System.Collections.Generic;
using System.Linq;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace SpineAnimator
{
  public class SpineAnimatorLayer<TEnum>
  where TEnum : Enum
  {
    public int Id;
    public SpineAnimationState<TEnum> Start;
    public SpineAnimationState<TEnum> Current;
    public SpineAnimationState<TEnum> Next;
    public List<SpineAnimationState<TEnum>> States = new List<SpineAnimationState<TEnum>>();
    public readonly SkeletonAnimation Skeleton;

    public SpineAnimatorLayer(int id, SkeletonAnimation skeleton)
    {
      Id = id;
      Skeleton = skeleton;
    }
    
    public void ChangeAnimation(SpineAnimationState<TEnum> to)
    {
      Current = to;
      if (CheckTransition()) return;
      // UnityEngine.Debug.Log($"{typeof(TEnum)} Change to {Current.Animation.Name}");
      if (Current.Animation.Asset)
        Skeleton.state.SetAnimation(Id, Current.Animation.Asset, Current.Animation.IsLoop);
      else
        Skeleton.state.SetEmptyAnimation(Id, 0);
    }

    public bool CheckTransition()
    {
      ClearNext();
      var transition = Current?.FindFirstCompletedCondition();
      // UnityEngine.Debug.Log($"It has {Current?.Transitions?.Count(x => x.Condition()) ?? 0} transitions");
      if (transition == null) return false;
      if (transition.IsHold)
      {
        // UnityEngine.Debug.Log($"{typeof(TEnum)} Transition now {Current.Animation.Name}");
        // UnityEngine.Debug.Log($"{typeof(TEnum)} Transition hold from {transition.Destination.Animation.Name}");
        DelayAnimation(transition.Destination);
        return false;
      }
      
      // UnityEngine.Debug.Log($"{typeof(TEnum)} Transition change {transition.Destination.Animation.Name}");
      ChangeAnimation(transition.Destination);
      return true;
    }

    public void ClearNext()
    {
      Next = null;
      Skeleton.state.Complete -= OnAnimationCompleted;
    }

    public void DelayAnimation(SpineAnimationState<TEnum> to)
    {
      // UnityEngine.Debug.Log($"{typeof(TEnum)} Delay from {Skeleton.state.Tracks.Items[0].Animation.Name}");
      // UnityEngine.Debug.Log($"{typeof(TEnum)} Delay to {to.Animation.Name}");
      Next = to;
      Skeleton.state.Complete += OnAnimationCompleted;
    }

    public void OnAnimationCompleted(TrackEntry trackEntry)
    {
      if (trackEntry.Animation != Current.Animation.Asset.Animation || trackEntry.TrackIndex != Id) return;
      
      // UnityEngine.Debug.Log($"{typeof(TEnum)} Complete {trackEntry.Animation.Name}");
      // UnityEngine.Debug.Log($"{typeof(TEnum)} Transit to {Next.Animation.Name}");
      ChangeAnimation(Next);
      Skeleton.state.Complete -= OnAnimationCompleted;
    }
  }
}