using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using SpiritMod.Items.Placeable.Tiles;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Block
{
	public class CreepingIceTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
			AddMapEntry(new Color(163, 224, 240));
			ItemDrop = ModContent.ItemType<CreepingIce>();
			DustType = DustID.SnowBlock;
		}
		public override bool HasWalkDust()
		{
			return true;
		}
		public override void WalkDust(ref int dustType, ref bool makeDust, ref Color color)
		{
			dustType = DustID.SnowBlock;
			makeDust = true;
		}
		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			Player player = Main.LocalPlayer;
			int distance = (int)Vector2.Distance(new Vector2(i * 16, j * 16), player.Center);
			if (distance < 54) {
				SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 27));

			}
		}
		public override bool IsTileDangerous(int i, int j, Player player)
		{
			return true;
		}
		public override void FloorVisuals(Player player)
		{
			player.AddBuff(BuffID.Chilled, 1200);
		}
		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (closer) {
				float distance = 15 * 16;
				List<NPC> foundNPCs = Main.npc.Where(n => n.active && n.DistanceSQ(new Vector2(i * 16, j * 16)) < distance * distance).OrderBy(n => n.DistanceSQ(new Vector2(i * 16, j * 16)) < distance * distance).ToList();
				foreach (var foundNPC in foundNPCs) {
					if (foundNPC != null) {
						int distance1 = (int)Vector2.Distance(new Vector2(i * 16, j * 16), foundNPC.Center);
						if (distance1 < 22) {
							foundNPC.AddBuff(ModContent.BuffType<MageFreeze>(), 20);
						}
					}
				}
				if (Main.rand.Next(20) == 0) {
					int d = Dust.NewDust(new Vector2(i * 16, j * 16 - 10), Main.rand.Next(-2, 2), Main.rand.Next(-2, 2), DustID.SnowBlock, 0.0f, -1, 0, new Color(), 0.95f);//Leave this line how it is, it uses int division

					Main.dust[d].velocity *= .8f;
					Main.dust[d].noGravity = true;
				}
			}
		}
	}
}

