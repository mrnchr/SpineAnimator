using System;
using System.Collections.Generic;

namespace SpineAnimator
{
  public class SpineAnimationState<TEnum>
    where TEnum : Enum
  {
    public readonly List<SpineAnimationTransition<TEnum>> Transitions;
    public readonly ConfigurableSpineAnimation<TEnum> Animation;

    public SpineAnimationState(ConfigurableSpineAnimation<TEnum> animation)
    {
      Transitions = new List<SpineAnimationTransition<TEnum>>();
      Animation = animation;
    }

    public SpineAnimationState() : this(null)
    {
    }

    public SpineAnimationTransition<TEnum> FindFirstCompletedCondition() =>
      Transitions.Find(x => x.Condition());
  }
}