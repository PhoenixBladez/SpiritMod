using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using SpiritMod.Utilities;
using SpiritMod.Mechanics.EventSystem.Controllers;

using Terraria;

namespace SpiritMod.Mechanics.EventSystem.Cutscenes
{
	public class FollowProjectileThenReturn : Event
	{
		private int _type;
		private bool _setReturn;

		public FollowProjectileThenReturn(Projectile projectile, float lerpTime, float zoom = 1f)
		{
			_type = projectile.type;
			
			// create the camera controller
			var camera = 
				new CameraController(0f, 1f, new CameraController.EntityRelativePoint(Main.LocalPlayer, Vector2.Zero))
				.AddPoint(lerpTime, new CameraController.EntityRelativePoint(projectile, Vector2.Zero) { Zoom = new Vector2(zoom) }, EaseFunction.EaseQuadInOut);
			camera.Centered = true;
			camera.CheckIfCompleted = () =>
			{
				if (_setReturn) return _currentTime >= lerpTime;

				// the projectile is dead
				if (!projectile.active || _type != projectile.type)
				{
					_currentTime = 0f;

					// reset camera and make it a different one, this one set to return to the player
					CameraController.CameraPointData data = camera.CurrentData;
					camera.Reset();
					camera.AddPoint(0f, new CameraController.StaticPoint(data), EaseFunction.Linear)
						.AddPoint(lerpTime, new CameraController.EntityRelativePoint(Main.LocalPlayer, Vector2.Zero), EaseFunction.EaseQuadInOut);

					_setReturn = true;
				}
				return false;
			};
			_controllers.AddLast(camera);
		}

		public override bool Update(float deltaTime)
		{
			return base.Update(deltaTime);
		}
	}
}
