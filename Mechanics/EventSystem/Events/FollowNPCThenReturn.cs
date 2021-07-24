using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using SpiritMod.Utilities;
using SpiritMod.Mechanics.EventSystem.Controllers;

using Terraria;

namespace SpiritMod.Mechanics.EventSystem.Events
{
	public class FollowNPCThenReturn : Event
	{
		private readonly int _type;
		private bool _setReturn;

		public FollowNPCThenReturn(NPC npc, float lerpTime, int maxTime, float zoom = 1f)
		{
			_type = npc.type;
			
			// create the camera controller
			var camera = 
				new CameraController(0f, new CameraController.EntityRelativePoint(Main.LocalPlayer, Vector2.Zero) { Zoom = new Vector2(Main.GameZoomTarget) })
				.AddPoint(lerpTime, new CameraController.EntityRelativePoint(npc, Vector2.Zero) { Zoom = new Vector2(zoom) }, EaseFunction.EaseQuadInOut);
			camera.AmICompleted = () =>
			{
				if (_setReturn) return _currentTime >= lerpTime;

				// the npc is dead
				if (!npc.active || _type != npc.type || _currentTime > maxTime)
				{
					_currentTime = 0f;

					// reset camera and make it a different one, this one set to return to the player
					CameraController.CameraPointData data = camera.CurrentData;
					camera.Reset();
					camera.AddPoint(0f, new CameraController.StaticPoint(data), EaseFunction.Linear)
						.AddPoint(lerpTime, new CameraController.EntityRelativePoint(Main.LocalPlayer, Vector2.Zero) { Zoom = new Vector2(Main.GameZoomTarget) }, EaseFunction.EaseQuadInOut);

					_setReturn = true;
				}
				return false;
			};
			AddToQueue(camera);
		}

		public override bool Update(float deltaTime)
		{
			return base.Update(deltaTime);
		}
	}
}
