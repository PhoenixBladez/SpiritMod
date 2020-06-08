using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient.ReachMicros
{
    public class Vine1 : ModTile
    {
        public override void SetDefaults() {
            Main.tileBlockLight[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileLighted[Type] = false;
            soundType = SoundID.Grass;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
            16,
            16,
            };
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.AnchorBottom = default(AnchorData);
            TileObjectData.addTile(Type);
            dustType = 2;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Vine");
            AddMapEntry(new Color(104, 156, 70), name);
        }
        public override void RandomUpdate(int i, int j) {
            if(WorldGen.genRand.Next(40) == 0) {
                bool isPlayerNear = WorldGen.PlayerLOS(i, j);
                if(isPlayerNear) {
                    Framing.GetTileSafely(i, j).ClearTile();
                    WorldGen.PlaceTile(i, j, mod.TileType("Vine2"));
                }
            }
        }
    }
}