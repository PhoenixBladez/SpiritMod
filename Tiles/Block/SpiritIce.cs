using Microsoft.Xna.Framework;
using SpiritMod.Items.Placeable.Tiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Block
{
	public class SpiritIce : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			TileID.Sets.Conversion.Ice[Type] = true;
			AddMapEntry(new Color(70, 130, 180));
			ItemDrop = ModContent.ItemType<SpiritIceItem>();
            HitSound = null;
            DustType = DustID.SnowBlock;
		}

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            Player player = Main.LocalPlayer;
            int distance = (int)Vector2.Distance(new Vector2(i * 16, j * 16), player.Center);
            if (distance < 500)
                SoundEngine.PlaySound(SoundID.Item50);
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if (!Framing.GetTileSafely(i, j - 1).HasTile) {
				r = 0.08f;
				g = 0.12f;
				b = 0.28f;
			}
		}
	}
}

