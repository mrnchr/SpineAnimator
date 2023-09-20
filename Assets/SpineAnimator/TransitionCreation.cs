using System;
using System.Collections.Generic;

namespace SpineAnimator
{
  public class TransitionCreation<TEnum>
  where TEnum : Enum
  {
    private readonly SpineAnimator<TEnum> _animator;
    private readonly List<TEnum> _from = new List<TEnum>();
    private readonly List<TEnum> _to = new List<TEnum>();

    public TransitionCreation(SpineAnimator<TEnum> animator)
    {
      _animator = animator;
    }

    public TransitionCreation<TEnum> From(params TEnum[] froms)
    {
      _from.AddRange(froms);
      return this;
    }
    
    public TransitionCreation<TEnum> To(params TEnum[] tos)
    {
      _to.AddRange(tos);
      return this;
    }

    public SpineAnimator<TEnum> End(Func<bool> when, bool isHold = false)
    {
      foreach (TEnum from in _from)
      {
        foreach (TEnum to in _to)
        {
          _animator.AddTransition(from, to, when, isHold);
        }
      }

      return _animator;
    }
  }
}