using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Dusts
{
	public class VoidDust : ModDust
	{
		public static int _type;

		private const int ANIMATION_TIME = 40;
		private const float SCALE = 1f / ANIMATION_TIME;
		private static readonly Vector3 LIGHT_COLOR = new Vector3(0x42, 0x1d, 0x60)/255;

		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = true;
			dust.noLight = false;
			dust.scale = 2f;
			dust.alpha = 100;
		}

		public override bool Update(Dust dust)
		{
			if (!dust.noLight)
				Lighting.AddLight((int)(dust.position.X) >> 4, (int)(dust.position.Y) >> 4, LIGHT_COLOR.X, LIGHT_COLOR.Y, LIGHT_COLOR.Z);
			
			if (dust.customData == null)
				return true;

			if (dust.customData is Entity)
			{
				Entity e = (Entity)dust.customData;
				dust.customData = new
					VoidDustAnchor
				{
					anchor = e,
					offset = dust.position - e.Center
				};
			}
			if (dust.customData is VoidDustAnchor)
			{
				VoidDustAnchor follow = (VoidDustAnchor)dust.customData;
				if (follow.counter >= ANIMATION_TIME)
				{
					dust.active = false;
					return false;
				}
				else if (!follow.anchor.active)
				{
					dust.customData = null;
					return true;
				}

				float s = 1 - follow.counter * SCALE;
				dust.scale = 1.6f * s;
				dust.position = follow.anchor.Center + (s * follow.offset);
				follow.counter++;
				return false;
			}
			return true;
		}
	}

	internal class VoidDustAnchor
	{
		internal int counter;
		internal Entity anchor;
		internal Vector2 offset;
	}
}