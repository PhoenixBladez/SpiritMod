using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Furniture.Pylons
{
	internal abstract class SimplePylonTile<T> : ModPylon where T : ModItem
	{
		public const int CrystalHorizontalFrameCount = 2;
		public const int CrystalVerticalFrameCount = 8;
		public const int CrystalFrameHeight = 64;

		public Asset<Texture2D> crystalTexture;
		public Asset<Texture2D> mapIcon;

		internal abstract string MapKeyName { get; }

		public override void Load()
		{
			crystalTexture = ModContent.Request<Texture2D>(Texture + "_Crystal");
			mapIcon = ModContent.Request<Texture2D>(Texture + "_MapIcon");
		}

		public override void SetStaticDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.StyleHorizontal = true;

			TEModdedPylon moddedPylon = ModContent.GetInstance<SimplePylonEntity>();
			TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(moddedPylon.PlacementPreviewHook_CheckIfCanPlace, 1, 0, true);
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(moddedPylon.Hook_AfterPlacement, -1, 0, false);
			TileObjectData.addTile(Type);

			TileID.Sets.InteractibleByNPCs[Type] = true;
			TileID.Sets.PreventsSandfall[Type] = true;

			AddToArray(ref TileID.Sets.CountsAsPylon);

			ModTranslation pylonName = CreateMapEntryName();
			pylonName.SetDefault($"${MapKeyName}");
			AddMapEntry(Color.White, pylonName);
		}

		public virtual bool IsSold(int npcType, Player player, bool npcHappyEnough) => true;
		public override int? IsPylonForSale(int npcType, Player player, bool isNPCHappyEnough) => IsSold(npcType, player, isNPCHappyEnough) ? ModContent.ItemType<T>() : null;

		public override void MouseOver(int i, int j)
		{
			Main.LocalPlayer.cursorItemIconEnabled = true;
			Main.LocalPlayer.cursorItemIconID = ModContent.ItemType<T>();
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			ModContent.GetInstance<SimplePylonEntity>().Kill(i, j); //Kill pylon tile entity
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 2, 3, ModContent.ItemType<T>());
		}

		public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch) => DefaultDrawPylonCrystal(spriteBatch, i, j, crystalTexture, Color.White, CrystalFrameHeight, CrystalHorizontalFrameCount, CrystalVerticalFrameCount);

		public override void DrawMapIcon(ref MapOverlayDrawContext context, ref string mouseOverText, TeleportPylonInfo pylonInfo, bool isNearPylon, Color drawColor, float deselectedScale, float selectedScale)
		{
			// Just like in SpecialDraw, we want things to be handled the EXACT same way vanilla would handle it, which ModPylon also has built in methods for:
			bool mouseOver = DefaultDrawMapIcon(ref context, mapIcon, pylonInfo.PositionInTiles.ToVector2() + new Vector2(1.5f, 2f), drawColor, deselectedScale, selectedScale);
			DefaultMapClickHandle(mouseOver, pylonInfo, MapKeyName, ref mouseOverText);
		}
	}
}