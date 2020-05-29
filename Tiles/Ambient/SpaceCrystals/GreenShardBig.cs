using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient.SpaceCrystals
{
    public class GreenShardBig : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Width = 2;
            Main.tileLighted[Type] = true;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
            16,
            16
            };
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Giant Crystal");
            dustType = 110;
            AddMapEntry(new Color(200, 200, 200), name);
        }
        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
        {
            offsetY = 2;
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.5f/4;
            g = 0.8f/4;
            b = 0.4f/4;
        }
       public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Terraria.Item.NewItem(i * 16, j * 16, 64, 32, mod.ItemType("RockCandy"));
			if (Main.rand.Next(4) == 1)
			{
				Terraria.Item.NewItem(i * 16, j * 16, 64, 32, mod.ItemType("Geode"), 3);
			}
			Player player = Main.LocalPlayer;
            {
                Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 27));
            }
		}
    }
}
