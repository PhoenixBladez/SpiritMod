using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Graphics.Effects;

namespace SpiritMod.NPCs.Cystal
{
	public class Crystallization : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cystallization");
			Description.SetDefault("Rapidly losing life and reduced movement speed");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
			Terraria.ID.BuffID.Sets.LongerExpertDebuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) => player.GetModPlayer<CrystalDebuffPlayer>().crystallization = true;
	}

	internal class CrystalDebuffPlayer : ModPlayer
	{
		public bool crystallization;
		public override void ResetEffects() => crystallization = false;

		public override void UpdateDead() => crystallization = false;

		public override void UpdateBadLifeRegen()
		{
			if (crystallization && Player.statLife > 4)
			{
				if (Player.lifeRegen > 0)
					Player.lifeRegen = 0;
				Player.lifeRegenTime = 0;
				Player.lifeRegen = Player.lifeRegen - 4;
			}

			if (!crystallization)
			{
				Filters.Scene.Deactivate("CystalTower", Player.position);
				Filters.Scene.Deactivate("CystalBloodMoon", Player.position);
			}
		}

		public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
		{
			if (drawInfo.drawPlayer.GetModPlayer<CrystalDebuffPlayer>().crystallization) {
				if (drawInfo.shadow == 0f) {
					int index = Dust.NewDust(new Vector2(Player.getRect().X, Player.getRect().Y), Player.getRect().Width, Player.getRect().Height, DustID.TeleportationPotion, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index].scale = 1.5f;
					Main.dust[index].noGravity = true;
					Main.dust[index].velocity *= 1.1f;
					drawInfo.DustCache.Add(index);
				}
				r *= 0.149f;
				g *= 0.142f;
				b *= 0.207f;
				fullBright = true;
			}
		}
	}
}
