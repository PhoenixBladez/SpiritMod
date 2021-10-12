using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GranitechSet.GranitechGun
{
	public class GranitechGunDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = true;

			dust.scale = Main.rand.NextFloat(1f, 1.6f);
			dust.noLight = true;
		}

		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity;

			int frame = dust.frame.X / 14;

			if (frame < 2)
				dust.rotation = dust.velocity.ToRotation();
			else 
				dust.rotation += 0.01f;

			if (frame == 3)
				dust.velocity *= 0.9f;
			else if (frame == 2)
				dust.velocity *= 0.94f;
			else if (frame == 1)
				dust.velocity *= 0.96f;
			else
				dust.velocity *= 0.98f;

			if (dust.fadeIn <= 0)
			{
				dust.scale -= 0.02f;
				dust.alpha = System.Math.Min((int)(dust.scale / 1.2f * 255), 255);

				if (dust.scale <= 0)
					dust.active = false;
			}
			else
			{
				if (dust.scale < dust.fadeIn)
					dust.scale += 0.06f * (dust.fadeIn);
				else
				{
					dust.scale -= 0.02f;
					dust.fadeIn -= 0.02f;
				}
			}

			return false;
		}

		public static void RandomizeFrame(Dust dust)
		{
			int frame = Main.rand.Next(3);
			dust.frame = new Microsoft.Xna.Framework.Rectangle(frame * 14, Main.rand.Next(2) * 12, 14, 12);
		}

		public static void SetFrame(Dust dust, int frame) => dust.frame = new Microsoft.Xna.Framework.Rectangle(frame * 14, Main.rand.Next(2) * 12, 14, 12);
	}
}