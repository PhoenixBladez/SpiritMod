using Microsoft.Xna.Framework;
using SpiritMod.Items.Placeable.Tiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Tiles.Block
{
	public class ScrapTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileMergeDirt[Type] = true;
			AddMapEntry(new Color(150, 150, 150));
			Main.tileBlockLight[Type] = true;
			ItemDrop = ModContent.ItemType<ScrapItem>();
			HitSound = SoundID.Tink;
		}
		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			Player player = Main.LocalPlayer;
			SoundEngine.PlaySound(SoundID.NPCHit4);
		}
	}
}