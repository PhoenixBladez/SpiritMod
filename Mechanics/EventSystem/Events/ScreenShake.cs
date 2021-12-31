using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using SpiritMod.Utilities;
using SpiritMod.Mechanics.EventSystem.Controllers;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.EventSystem.Events
{
	public class ScreenShake : Event
	{
		public ScreenShake(float strength, float time)
		{
			// create the camera controller
			var camera =
				new CameraController(0f, new RandomRelativePoint(strength))
				.AddPoint(time, new RandomRelativePoint(0f), EaseFunction.EaseQuadOut);

			AddToQueue(camera);
		}

		/// <summary>
		/// Custom value grabber for generating a random value relative to the current screen position based on distance
		/// </summary>
		private class RandomRelativePoint : CameraController.IValueGrabber<CameraController.CameraPointData>
		{
			private float _distance;

			public Func<CameraController.CameraPointData> Method => () =>
			{
				return new CameraController.CameraPointData(
					(Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f) + // center screen +
					Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi).ToRotationVector2() * _distance * ModContent.GetInstance<SpiritClientConfig>().ScreenShake); // random offset
			};

			public RandomRelativePoint(float distance)
			{
				_distance = distance;
			}
		}
	}
}
