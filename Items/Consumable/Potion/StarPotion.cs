using SpiritMod.Buffs.Potion;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Potion
{
	public class StarPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starburn Potion");
			Tooltip.SetDefault("Critical hits may cause stars to fall from the sky\nIncreases ranged damage and critical strike chance by 4% when moving");
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
			Item.buffType = ModContent.BuffType<StarPotionBuff>();
			Item.buffTime = 10800;
			Item.UseSound = SoundID.Item3;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<SoulBloom>(), 1);
			recipe.AddIngredient(ItemID.Feather, 2);
			recipe.AddIngredient(ModContent.ItemType<StarPiece>(), 1);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddTile(TileID.Bottles);
			recipe.Register();
		}
	}
}
