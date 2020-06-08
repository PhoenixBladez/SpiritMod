using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;
using SpiritMod.Items.Placeable;
using Terraria.DataStructures;

namespace SpiritMod.Tiles
{
	public class JumpPadTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.Origin = new Point16(1, 1);
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
             TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight; //allows me to place example chairs facing the same way as the player
            TileObjectData.addAlternate(1); //facing right will use the second texture style
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Jump Pad");
            dustType = 67;
			AddMapEntry(new Color(200, 200, 200), name);
		}

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			{
				//Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 27));
				Item.NewItem(i * 16, j * 16, 48, 48, ModContent.ItemType<JumpPadItem>(), Main.rand.Next(6, 13));
			}
		}
        public override void NearbyEffects(int i, int j, bool closer)
        {
            Tile tile = Main.tile[i, j];
            Player player = Main.LocalPlayer;
            if (closer)
            {
                int distance1 = (int)Vector2.Distance(new Vector2(i * 16, j * 16), player.Center);
               
            }
        }
    }
}