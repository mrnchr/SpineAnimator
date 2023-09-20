using System;

namespace SpineAnimator
{
  public class SpineAnimationTransition<TEnum>
    where TEnum : Enum
  {
    public SpineAnimationState<TEnum> Destination;
    public bool IsHold;
    public Func<bool> Condition;
  }
}