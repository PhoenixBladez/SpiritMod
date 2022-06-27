using SpiritMod.Buffs;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Potion
{
	public class SpiritPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Potion");
			Tooltip.SetDefault("Increases damage and critical strike chance by 5% \nGetting hurt occasionally spawns a damaging bolt to chase enemies");
		}


		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 30;
			Item.rare = ItemRarityID.Pink;
			Item.maxStack = 30;

			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useTime = Item.useAnimation = 20;

			Item.consumable = true;
			Item.autoReuse = false;

			Item.buffType = ModContent.BuffType<SpiritBuff>();
			Item.buffTime = 7200;

			Item.UseSound = SoundID.Item3;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<SpiritKoi>(), 1);
			recipe.AddIngredient(ItemID.SoulofLight, 1);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddTile(TileID.Bottles);
			recipe.Register();
		}
	}
}
