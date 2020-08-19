using Microsoft.Xna.Framework;
using SpiritMod.Items.Placeable.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Block
{
	public class SpiritIce : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			AddMapEntry(new Color(70, 130, 180));
			drop = ModContent.ItemType<SpiritIceItem>();
            soundType = -1;
            dustType = 51;
		}
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            Player player = Main.LocalPlayer;
            int distance = (int)Vector2.Distance(new Vector2(i * 16, j * 16), player.Center);
            if (distance < 500)
            {
                Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 50));
            }
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if (!Framing.GetTileSafely(i, j - 1).active()) {
				r = 0.08f;
				g = 0.12f;
				b = 0.28f;
			}
		}

		public override bool CanExplode(int i, int j)
		{
			return true;
		}
	}
}

