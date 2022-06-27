using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Furniture
{
	public class PottedSakura : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sakura Bonsai");
		}


		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 28;
			Item.value = Terraria.Item.buyPrice(0, 0, 80, 0);

			Item.maxStack = 99;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = Mod.Find<ModTile>("PottedSakuraTile").Type;
		}
	}
}