using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Dusts
{
	public class Wind : ModDust
	{
		public static int _type;

		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = true;
			dust.velocity = Vector2.Zero;
		}

		public override bool Update(Dust dust)
		{
			if (dust.customData == null)
				dust.active = false;

			if (!(dust.customData is WindAnchor))
				return true;

			if (dust.alpha > 253)
			{
				dust.active = false;
				return false;
			}

			WindAnchor data = (WindAnchor)dust.customData;
			data.anchor += dust.velocity;
			dust.position = data.anchor + (data.offsetDir * data.offset).RotatedBy(dust.rotation);
			dust.rotation += data.turnRate;

			dust.alpha += 5;
			dust.scale *= .98f;
			dust.velocity *= .95f;
			data.offset += 0.4f;
			data.turnRate *= .92f;
			return false;
		}
	}

	internal class WindAnchor
	{
		public float turnRate;
		public float offset;
		public Vector2 offsetDir;
		public Vector2 anchor;

		public WindAnchor(Vector2 origin, Vector2 velocity, Vector2 position)
		{
			float length = velocity.Length();
			velocity = velocity * (1f / length);
			if (velocity.HasNaNs())
				velocity = new Vector2(0, -1);
			bool left = (position - origin).LeftOf(velocity);
			turnRate = 0.06f + Main.rand.NextFloat(0.04f); 
			turnRate *= length > 4 ? length : 4;
			if (left)
			{
				turnRate = -turnRate;
				offsetDir = -velocity.TurnLeft();
			}
			else
				offsetDir = -velocity.TurnRight();
			offset = 2 + Main.rand.NextFloat(2);
			anchor = offsetDir * offset;
			anchor += position;
		}

		public WindAnchor(Vector2 origin, Vector2 position)
		{
			bool left = position.X - origin.X < 0;
			turnRate = 0.06f + Main.rand.NextFloat(0.04f);
			turnRate *= 6;
			if (left)
			{
				turnRate = -turnRate;
				offsetDir = new Vector2(1, 0);
			}
			else
				offsetDir = new Vector2(-1, 0);
			offset = 2 + Main.rand.NextFloat(2);
			anchor = offsetDir * offset;
			anchor += position;
		}
	}
}