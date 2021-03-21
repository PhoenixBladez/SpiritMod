using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

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
			if (crystallization && player.statLife > 10) {
				if (player.lifeRegen > 0) {
					player.lifeRegen = 0;
				}
				player.lifeRegenTime = 0;
				player.lifeRegen = player.lifeRegen - 10;
				player.moveSpeed = player.moveSpeed * 0.25f;
			}
		}

		public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
		{
			if (crystallization) {
				if (drawInfo.shadow == 0f) {
					int index = Dust.NewDust(new Vector2((float)player.getRect().X, (float)player.getRect().Y), player.getRect().Width, player.getRect().Height, 164, 0.0f, 0.0f, 0, new Color(), 1f);
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
