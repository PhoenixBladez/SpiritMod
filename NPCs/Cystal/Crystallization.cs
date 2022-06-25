using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Graphics.Effects;

namespace SpiritMod.NPCs.Cystal
{
	public class Crystallization : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Cystallization");
			Description.SetDefault("Rapidly losing life and reduced movement speed");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
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
			if (crystallization && player.statLife > 4) {
				if (player.lifeRegen > 0)
					player.lifeRegen = 0;
				player.lifeRegenTime = 0;
				player.lifeRegen = player.lifeRegen - 4;
			}
		}

		public override void UpdateBiomeVisuals()
		{
			if (!crystallization)
			{
				Filters.Scene.Deactivate("CystalTower", player.position);
				Filters.Scene.Deactivate("CystalBloodMoon", player.position);
			}
		}

		public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
		{
			if (drawInfo.drawPlayer.GetModPlayer<CrystalDebuffPlayer>().crystallization) {
				if (drawInfo.shadow == 0f) {
					int index = Dust.NewDust(new Vector2(player.getRect().X, player.getRect().Y), player.getRect().Width, player.getRect().Height, DustID.TeleportationPotion, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index].scale = 1.5f;
					Main.dust[index].noGravity = true;
					Main.dust[index].velocity *= 1.1f;
					Main.playerDrawDust.Add(index);
				}
				r *= 0.149f;
				g *= 0.142f;
				b *= 0.207f;
				fullBright = true;
			}
		}
	}
}
