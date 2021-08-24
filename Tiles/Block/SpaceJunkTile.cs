using Microsoft.Xna.Framework;
using SpiritMod.Items.Placeable.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Block
{
	public class SpaceJunkTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
			AddMapEntry(new Color(87, 85, 81));
			drop = ModContent.ItemType<SpaceJunkItem>();
			dustType = DustID.Wraith;
		}

		public override void WalkDust(ref int dustType, ref bool makeDust, ref Color color)
		{
			dustType = DustID.Wraith;
			makeDust = true;
		}

		public override void FloorVisuals(Player player) => player.AddBuff(BuffID.Bleeding, 300);
		public override bool HasWalkDust() => true;

		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (closer && Main.rand.Next(20) == 0)
			{
				int d = Dust.NewDust(new Vector2(i * 16, j * 16 - 10), Main.rand.Next(-2, 2), Main.rand.Next(-2, 2), DustID.Wraith, 0.0f, -1, 0, new Color(), 0.5f);//Leave this line how it is, it uses int division
				Main.dust[d].velocity *= .8f;
				Main.dust[d].noGravity = true;
			}
		}
	}
}

