using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Block
{
	public class MarbleOre : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSpelunker[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;  //true for block to emit light
			Main.tileLighted[Type] = true;
            drop = mod.ItemType("MarbleChunk");   //put your CustomBlock name
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Enchanted Marble Chunk");
			AddMapEntry(new Color(227, 191, 75), name);
			soundType = 21;
			minPick = 65;
			dustType = DustID.GoldCoin;

		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			{
				r = .219f;
				g = .199f;
				b = .132f;
			}
		}
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            if (!NPC.downedBoss2)
            {
                return false;
            }
            return true;
        }
    }
}