using Microsoft.Xna.Framework;
using SpiritMod.Items.Placeable.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Tiles.Block
{
	public class AcidBrickTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileMergeDirt[Type] = true;
			AddMapEntry(new Color(163, 191, 164));
			Main.tileBlockLight[Type] = true;
			drop = ModContent.ItemType<AcidBrick>();
			soundType = SoundID.Tink;
		}
	}
}