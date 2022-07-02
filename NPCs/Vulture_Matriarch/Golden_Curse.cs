using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace SpiritMod.NPCs.Vulture_Matriarch
{
	public class Golden_Curse : ModBuff
	{
		public int dustTimer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Golden Curse");
			Description.SetDefault("Increased fall speed and damage");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
			Terraria.ID.BuffID.Sets.LongerExpertDebuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<Vulture_Matriarch_Player>().goldified = true;
			player.gravity = player.gravity * 2.2f * player.gravDir;
			float num2 = 60f;		
			++dustTimer;
			float num3 = dustTimer / num2;
			Vector2 spinningpoint = new Vector2(60f, -10f);
			spinningpoint = spinningpoint.RotatedBy((double)num3 * 1.5 * 6.28318548202515, new Vector2()) * new Vector2(0.25f, 0.25f);
			for (int index1 = 0; index1 < 4; ++index1)
			{
				Vector2 vector2 = Vector2.Zero;
				float num4 = 1f;
				if (index1 == 1)
				{
					vector2 = Vector2.UnitY * -5f;
					num4 = 0.9f;
				}
				spinningpoint *= -1f;
				int index3 = Dust.NewDust(player.Center, 0, 0, DustID.GoldFlame, 0.0f, 0.0f, 100, new Color(), 0.9f);
				Main.dust[index3].noGravity = true;
				Main.dust[index3].fadeIn = 0.4f;
				Main.dust[index3].position = new Vector2(player.Center.X, player.Center.Y - 30) + spinningpoint * num4 + vector2;
				Main.dust[index3].velocity = Vector2.Zero;
			}
			for (int index1 = 0; index1 < 4; ++index1)
			{
				Vector2 vector2 = Vector2.Zero;
				float num4 = 1f;
				if (index1 == 1)
				{
					vector2 = Vector2.UnitY * -5f;
					num4 = 0.9f;
				}
				spinningpoint *= -1f;
				int index3 = Dust.NewDust(player.Center, 0, 0, DustID.GoldFlame, 0.0f, 0.0f, 100, new Color(), 0.9f);
				Main.dust[index3].noGravity = true;
				Main.dust[index3].fadeIn = 0.4f;
				Main.dust[index3].position = new Vector2(player.Center.X, player.Center.Y + 30) + spinningpoint * num4 + vector2;
				Main.dust[index3].velocity = Vector2.Zero;
			}
			for (int index1 = 0; index1 < 4; ++index1)
			{
				Vector2 vector2 = Vector2.Zero;
				float num4 = 1f;
				if (index1 == 1)
				{
					vector2 = Vector2.UnitY * -5f;
					num4 = 0.9f;
				}
				spinningpoint *= -1f;
				int index3 = Dust.NewDust(player.Center, 0, 0, DustID.GoldFlame, 0.0f, 0.0f, 100, new Color(), 0.9f);
				Main.dust[index3].noGravity = true;
				Main.dust[index3].fadeIn = 0.4f;
				Main.dust[index3].position = new Vector2(player.Center.X - 30, player.Center.Y) + spinningpoint * num4 + vector2;
				Main.dust[index3].velocity = Vector2.Zero;
			}
			for (int index1 = 0; index1 < 4; ++index1)
			{
				Vector2 vector2 = Vector2.Zero;
				float num4 = 1f;
				if (index1 == 1)
				{
					vector2 = Vector2.UnitY * -5f;
					num4 = 0.9f;
				}
				spinningpoint *= -1f;
				int index3 = Dust.NewDust(player.Center, 0, 0, DustID.GoldFlame, 0.0f, 0.0f, 100, new Color(), 0.9f);
				Main.dust[index3].noGravity = true;
				Main.dust[index3].fadeIn = 0.4f;
				Main.dust[index3].position = new Vector2(player.Center.X + 30, player.Center.Y) + spinningpoint * num4 + vector2;
				Main.dust[index3].velocity = Vector2.Zero;
			}
		}
	}
}
