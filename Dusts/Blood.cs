using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Dusts
{
	public class Blood : ModDust
	{
		public static int _type;

		private const int STICK_TIME = 80;
		private const float BASE_SCALE = 0.1f;
		private const float GROW_RATE = (1 - BASE_SCALE) / STICK_TIME;
		

		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = true;
		}

		public override bool Update(Dust dust)
		{
			if (dust.customData != null)
			{
				if (dust.customData is NPC)
				{
					NPC npc = (NPC)dust.customData;
					dust.customData = new
						BloodAnchor
					{
						anchor = npc,
						oldRotation = npc.rotation,
						offset = dust.position - ((NPC)dust.customData).Center
					};
					dust.scale = BASE_SCALE;
					dust.velocity = Vector2.Zero;
				}
				if (dust.customData is BloodAnchor)
				{
					if (Follow(dust, (BloodAnchor)dust.customData))
						return false;
				}
			}
			if (Collision.SolidCollision(dust.position - Vector2.One * 5f, 10, 6) && dust.fadeIn == 0f)
			{
				dust.velocity = Vector2.Zero;
			}
			else
				dust.scale += 0.009f;
			return true;
		}

		private bool Follow(Dust dust, BloodAnchor follow)
		{
			NPC npc = follow.anchor;
			if (follow.counter++ >= STICK_TIME || !npc.active)
			{
				if (npc.active)
				{
					dust.velocity = npc.velocity
						+ 0.5f * (follow.offset.RotatedBy(npc.rotation)
						- follow.offset.RotatedBy(follow.oldRotation));
				}
				dust.customData = null;
				dust.noGravity = false;
				return false;
			}
			Vector2 c = npc.Center;
			dust.scale = BASE_SCALE + follow.counter * GROW_RATE;
			dust.position = c + follow.offset.RotatedBy(npc.rotation);
			follow.oldRotation = (dust.rotation = npc.rotation);
			return true;
		}
	}

	internal class BloodAnchor
	{
		internal int counter;
		internal Vector2 offset;
		internal NPC anchor;
		internal float oldRotation;
	}
}