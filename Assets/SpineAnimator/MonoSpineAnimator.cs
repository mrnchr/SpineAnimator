using System;
using Spine.Unity;
using UnityEngine;

namespace SpineAnimator
{
  public class MonoSpineAnimator<TEnum> : MonoBehaviour
  where TEnum : Enum
  {
    public SkeletonAnimation Skeleton;

    protected SpineAnimator<TEnum> _animator;
    protected bool _needCheck;
    
    protected virtual void SetVariable<T>(T value, ref T variable)
    {
      _needCheck = _needCheck || !value.Equals(variable);
      variable = value;
    }
    
    protected virtual void Start()
    {
      _animator.StartAnimate();
    }

    protected virtual void Update()
    {
      if (_needCheck)
      {
        _animator.CheckTransition();
        ClearTriggers();
        _needCheck = false;
      }
    }

    protected virtual void ClearTriggers()
    {
    }
  }
}