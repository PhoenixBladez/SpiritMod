using Microsoft.Xna.Framework;
using SpiritMod.Dusts;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class SoulSiphon : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Soul Siphon");
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			int distToNPC = 1100;
			for(int i = 0; i < Main.maxPlayers; ++i) {
				if(Main.player[i].active && !Main.player[i].dead && (npc.Center - Main.player[i].position).Length() < distToNPC && Main.player[i].inventory[Main.player[i].selectedItem].type == ModContent.ItemType<Items.Weapon.Magic.SoulSiphon>() && Main.player[i].itemAnimation > 0) {
					if(i == Main.myPlayer) {
						++Main.player[i].GetSpiritPlayer().soulSiphon;
					}

					if(Main.rand.NextBool(2)) {
						Vector2 center = npc.Center;
						center.X += Main.rand.Next(-100, 100) * 0.05F;
						center.Y += Main.rand.Next(-100, 100) * 0.05F;
						center += npc.velocity;

						int dust = Dust.NewDust(center, 1, 1, ModContent.DustType<BloodSiphonDust>());
						Main.dust[dust].velocity *= 0.0f;
						Main.dust[dust].scale = Main.rand.Next(70, 85) * 0.01f;
						Main.dust[dust].fadeIn = i + 1;
					}
				}
			}
		}
	}
}
