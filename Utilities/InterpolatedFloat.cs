using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiritMod.Utilities
{
	public class InterpolatedFloat
	{
		public float ExposedValue { get; private set; }
		private float LastValue { get; set; }
		private float TargetValue { get; set; }
		private float Time { get; set; }
		private float MaxTime { get; set; }
		private Func<float, float> Interpolation { get; set; }

		public InterpolatedFloat(float value, float maxTime) : this(value, value, value, 0, maxTime, f => f) { }

		public InterpolatedFloat(float value, float maxTime, Func<float, float> interpolation) : this(value, value, value, 0, maxTime, interpolation) { }

		private InterpolatedFloat(float exposedValue, float lastValue, float targetValue, float time, float maxTime, Func<float, float> interpolation)
		{
			ExposedValue = exposedValue;
			LastValue = lastValue;
			TargetValue = targetValue;
			Time = time;
			MaxTime = maxTime;
			Interpolation = interpolation;
		}

		public void Process(float deltaTime)
		{
			Time += deltaTime;

			if (Time > MaxTime) Time = MaxTime;

			float by = Time / MaxTime;
			by = Interpolation(by);
			ExposedValue = TargetValue * by + LastValue * (1 - by);
		}

		public void Set(float value)
		{
			Time = 0;

			LastValue = ExposedValue;
			TargetValue = value;
			Process(0);
		}

		public static InterpolatedFloat operator +(InterpolatedFloat iF, float f)
		{
			return new InterpolatedFloat(iF.ExposedValue + f, iF.LastValue + f, iF.TargetValue + f, iF.Time, iF.MaxTime, iF.Interpolation);
		}

		public static InterpolatedFloat operator -(InterpolatedFloat iF, float f)
		{
			return new InterpolatedFloat(iF.ExposedValue - f, iF.LastValue - f, iF.TargetValue - f, iF.Time, iF.MaxTime, iF.Interpolation);
		}

		public static implicit operator float(InterpolatedFloat iF) => iF.ExposedValue;

		// INTERPOLATION METHODS
		// taken right out from https://easings.net/

		public static float EaseInOut(float x) => x < 0.5 ? 4 * x * x * x : 1 - (float)Math.Pow(-2 * x + 2, 3) / 2;

		public static float EaseInOutBack(float x)
		{
			float c1 = 1.70158f;
			float c2 = c1 * 1.525f;

			return x < 0.5
			  ? ((float)Math.Pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2)) / 2
			  : ((float)Math.Pow(2 * x - 2, 2) * ((c2 + 1) * (x * 2 - 2) + c2) + 2) / 2;
		}
	}
}
