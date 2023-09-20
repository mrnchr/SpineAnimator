using System;
using System.Collections.Generic;
using System.Linq;
using Spine.Unity;

namespace SpineAnimator
{
  public class SpineAnimator<TEnum>
    where TEnum : Enum
  {
    private readonly SpineAnimatorLayer<TEnum>[] _layers;
    private readonly List<SpineAnimationState<TEnum>> _states = new List<SpineAnimationState<TEnum>>();
    private readonly SkeletonAnimation _skeleton;

    public SpineAnimator(SkeletonAnimation skeleton, ConfigurableSpineAnimation<TEnum>[] anims, int layerCount = 1,
      TEnum start = default)
    {
      _skeleton = skeleton;
      _layers = new SpineAnimatorLayer<TEnum>[layerCount];
      for (int i = 0; i < _layers.Length; i++)
        _layers[i] = new SpineAnimatorLayer<TEnum>(i, skeleton);

      foreach (var anim in anims)
        _states.Add(new SpineAnimationState<TEnum>(anim));

      if (layerCount == 1)
        AddAnimationsToLayer(0, start, _states.ToArray());
    }

    public SpineAnimator<TEnum> AddAnimationsToLayer(int index, TEnum start = default,
      params SpineAnimationState<TEnum>[] anims)
    {
      var layer = GetLayer(index);
      layer.Start ??= GetState(start);
      layer.States.AddRange(anims);

      return this;
    }

    public SpineAnimator<TEnum> AddAnimationsToLayer(int index, TEnum start = default,
      params ConfigurableSpineAnimation<TEnum>[] anims) =>
      AddAnimationsToLayer(index, start, _states.Where(x => anims.Contains(x.Animation)).ToArray());

    public SpineAnimator<TEnum> AddAnimationsToLayer(int index, TEnum start = default, params TEnum[] names) =>
      AddAnimationsToLayer(index, start, names.Select(GetState).ToArray());

    public SpineAnimator<TEnum> AddTransition(TEnum from, TEnum to, Func<bool> when, bool isHold = false)
    {
      GetState(from)
        .Transitions
        .Add(
          new SpineAnimationTransition<TEnum>
          {
            Destination = GetState(to),
            IsHold = isHold,
            Condition = when
          });

      return this;
    }


    public TransitionCreation<TEnum> CreateTransition() => new TransitionCreation<TEnum>(this);
    public SpineAnimatorLayer<TEnum> GetLayer(int index) => _layers[index];

    public SpineAnimationState<TEnum> GetState(TEnum type) =>
      _states.Single(x => x.Animation.Name.Equals(type));

    public void SetVariable<T>(T value, ref T variable)
    {
      bool needCheck = !value.Equals(variable);
      variable = value;

      if (needCheck)
        CheckTransition();
    }

    public void StartAnimate()
    {
      foreach (var layer in _layers)
      {
        if (layer.Start == null)
          throw new ArgumentNullException(nameof(layer.Start),
            $"Animator layer by index {layer.Id} has not a start animation");
      }

      foreach (var layer in _layers)
        layer.ChangeAnimation(layer.Start);
    }

    public void CheckTransition()
    {
      foreach (var layer in _layers)
        layer.CheckTransition();
    }
  }
}