using Microsoft.Xna.Framework;
using SpiritMod.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient
{
	public class ExplosiveBarrelTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoFail[Type] = true;
            Main.tileLighted[Type] = true;
            animationFrameHeight = 54;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
			16,
			16,
			16
			};
			TileObjectData.addTile(Type);
            dustType = -1;
            soundType = -1;

			AddMapEntry(new Color(219, 31, 31));
		}
		public override void AnimateTile(ref int frame, ref int frameCounter) {
            frameCounter++;
            if (frameCounter >= 12)
            {
                frameCounter = 0;
                frame++;
                frame %= 3;
            }
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = .242f * .5f;
            g = .132f * .5f;
            b = .068f * .5f;
        }
        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
		{
			offsetY = 2;
		}
		public int cloudtimer;
		public override void NearbyEffects(int i, int j, bool closer)
        {
            Player player = Main.LocalPlayer;
            if (closer)
            {
                int distance1 = (int)Vector2.Distance(new Vector2(i * 16, j * 16), player.Center);
                if (distance1 < 500)
                {
                    int n = NPC.NewNPC((int)i * 16 + 8, (int)j * 16 + 46, ModContent.NPCType<ExplosiveBarrel>(), 0, 2, 1, 0, 0, Main.myPlayer);
                    Main.npc[n].netUpdate = true;
                    WorldGen.KillTile(i, j);
                }

            }
        }
	}
}