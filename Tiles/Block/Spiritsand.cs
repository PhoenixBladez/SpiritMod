using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.World.Generation;

namespace SpiritMod.Tiles.Block
{
	public class Spiritsand : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			SetModCactus(new SpiritCactus());
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			AddMapEntry(new Color(135, 206, 235));
			drop = mod.ItemType("SpiritSandItem");
			dustType = 103;
		}

		public override bool CanExplode(int i, int j)
		{
			return true;
		}
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            if (j < Main.maxTilesY && !Main.tile[i, j + 1].active())
            {
                Main.tile[i, j].active(false);
                Projectile.NewProjectile(new Vector2(i * 16f + 8f, j * 16f + 8f), Vector2.Zero * 15f, mod.ProjectileType("SpiritSand"), 15, 0f);
                WorldGen.SquareTileFrame(i, j);
                return false;
            }
            return true;
        }
	}
}

