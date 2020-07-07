
using Microsoft.Xna.Framework;
using SpiritMod.NPCs.Boss;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient
{
	public class AvianEgg : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;

			Main.tileCut[Type] = false;

			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 8;
			TileObjectData.newTile.Width = 6;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
			16,
			16,
			16,
            16,
            16,
            16,
			16,
            16
			};
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Avian Egg");
			AddMapEntry(new Color(227, 195, 124), name);
		}
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
		{
			offsetY = 2;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			{
				Main.NewText("The Ancient Avian has awoken!", 175, 75, 255, true);
				NPC.NewNPC((int)i * 16 + Main.rand.Next(-60, 60), (int)j * 16 - 120, ModContent.NPCType<AncientFlyer>(), 0, 2, 1, 0, 0, Main.myPlayer);


                for (int num625 = 0; num625 < 2; num625++)
                {
                    float scaleFactor10 = 0.2f;
                    if (num625 == 1)
                    {
                        scaleFactor10 = 0.5f;
                    }
                    if (num625 == 2)
                    {
                        scaleFactor10 = 1f;
                    }
                    int num626 = Gore.NewGore(new Vector2((int)i * 16 + Main.rand.Next(-60, 60), (int)j * 16 - 120), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[num626].velocity *= scaleFactor10;
                    Gore expr_13AB6_cp_0 = Main.gore[num626];
                    expr_13AB6_cp_0.velocity.X = expr_13AB6_cp_0.velocity.X + 1f;
                    Gore expr_13AD6_cp_0 = Main.gore[num626];
                    expr_13AD6_cp_0.velocity.Y = expr_13AD6_cp_0.velocity.Y + 1f;
                    num626 = Gore.NewGore(new Vector2((int)i * 16 + Main.rand.Next(-60, 60), (int)j * 16 - 120), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[num626].velocity *= scaleFactor10;
                    Gore expr_13B79_cp_0 = Main.gore[num626];
                    expr_13B79_cp_0.velocity.X = expr_13B79_cp_0.velocity.X - 1f;
                    Gore expr_13B99_cp_0 = Main.gore[num626];
                    expr_13B99_cp_0.velocity.Y = expr_13B99_cp_0.velocity.Y + 1f;
                }
                Main.PlaySound(SoundID.Roar, new Vector2((int)i * 16, (int)j * 16), 0);
                Main.PlaySound(new Terraria.Audio.LegacySoundStyle(4, 1));
				for(int k = 0; k < 20; k++) {
					int d = Dust.NewDust(new Vector2(i * 16, j * 16 - 10), 0, 16, 0, 0.0f, -1, 0, new Color(), 0.5f);//Leave this line how it is, it uses int division
					Gore.NewGore(new Vector2((int)i * 16 + Main.rand.Next(-10, 10), (int)j * 16 + Main.rand.Next(-10, 10)), new Vector2(-1, 1), mod.GetGoreSlot("Gores/Apostle2"), Main.rand.NextFloat(.7f, 1.8f));
				}
			}
		}
	}
}