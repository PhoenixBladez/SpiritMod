using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Systems;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient
{
	public class StargrassFlowers : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileCut[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileMergeDirt[Type] = true;
			Main.tileLighted[Type] = false;

			DustType = DustID.Grass;
			HitSound = SoundID.Grass;

			//Terraria.GameContent.Metadata.TileMaterials.SetForTileId(Type, Terraria.GameContent.Metadata.TileMaterials._materialsByName["Plant"]);
			//TileID.Sets.SwaysInWindBasic[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.WaterDeath = false;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinateHeights = new int[] { 20 };
			TileObjectData.newTile.DrawYOffset = -2;
			TileObjectData.newTile.Style = 0;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.UsesCustomCanPlace = true;

			for (int i = 0; i < 11; i++) {
				TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
				TileObjectData.addSubTile(TileObjectData.newSubTile.Style);
			}

			TileObjectData.addTile(Type);
		}

		public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData) => base.DrawEffects(i, j, spriteBatch, ref drawData);
		public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch) => base.SpecialDraw(i, j, spriteBatch);
		public override void NumDust(int i, int j, bool fail, ref int num) => num = 2;

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			int frame = Main.tile[i, j].TileFrameX / 18;
			if (frame >= 6)
				(r, g, b) = (0.025f, 0.1f, 0.25f);
		}

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			TileSwaySystem.DrawGrassSway(spriteBatch, TextureAssets.Tile[Type].Value, i, j, Lighting.GetColor(i, j));
			return false;
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Color colour = Color.White * MathHelper.Lerp(0.2f, 1f, (float)((System.Math.Sin(SpiritMod.GlobalNoise.Noise(i * 0.2f, j * 0.2f) * 3f + Main.GlobalTimeWrappedHourly * 1.3f) + 1f) * 0.5f));
			TileSwaySystem.DrawGrassSway(spriteBatch, Texture + "_Glow", i, j, colour);
		}
	}
}