using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;

namespace SpiritMod.Tiles.Ambient.IceSculpture.Hostile
{
	public class IceFlinxHostile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
			16,
			16
			};
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Frozen Snow Flinx");
            dustType = 51;
			AddMapEntry(new Color(200, 200, 200), name);
		}
		public override void SetDrawPositions (int i, int j, ref int width, ref int offsetY, ref int height)
		{
			offsetY = 2;
		}
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            if (!NPC.downedBoss3)
            {
                return false;
            }
            return true;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			{
				Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 27));
				Item.NewItem(i * 16, j * 16, 48, 48, mod.ItemType("CreepingIce"), Main.rand.Next(6, 13));
			}
        }
        public override void NearbyEffects(int i, int j, bool closer)
        {

            Player player = Main.LocalPlayer;
            if (closer && NPC.downedBoss3)
            {
                int distance1 = (int)Vector2.Distance(new Vector2(i * 16, j * 16), player.Center);
                if (distance1 < 56)
                {
                    Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 27));
                    int n = NPC.NewNPC((int)i * 16, (int)j * 16, NPCID.SnowFlinx, 0, 2, 1, 0, 0, Main.myPlayer);
                    Main.npc[n].GivenName = "Icy Snow Flinx";
                    Main.npc[n].lifeMax = Main.npc[n].lifeMax * 2;
                    Main.npc[n].life = Main.npc[n].lifeMax;
                    Main.npc[n].damage = (int)(Main.npc[n].damage * 1.5f);
                    Main.npc[n].knockBackResist = 0.25f;
                    WorldGen.KillTile(i, j);
                }
            }
        }
    }
}