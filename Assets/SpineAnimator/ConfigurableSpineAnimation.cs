using System;
using Spine.Unity;

namespace SpineAnimator
{
  [Serializable]
  public class ConfigurableSpineAnimation<TEnum>
    where TEnum : Enum
  {
    public TEnum Name;
    public AnimationReferenceAsset Asset;
    public bool IsLoop;
  }
}