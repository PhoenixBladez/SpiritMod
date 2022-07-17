using SpiritMod.Buffs.Potion;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Potion
{
	public class SoulPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soulguard Potion");
			Tooltip.SetDefault("Getting hurt may cause all nearby enemies to suffer 'Soul Burn' for a short while\nIncreases melee damage by 5%");
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
			Item.buffType = ModContent.BuffType<SoulPotionBuff>();
			Item.buffTime = 10800;
			Item.UseSound = SoundID.Item3;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<SoulBloom>(), 1);
			recipe.AddIngredient(ItemID.IronOre, 1);
			recipe.AddIngredient(ModContent.ItemType<Items.Sets.SpiritSet.SpiritOre>(), 3);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddTile(TileID.Bottles);
			recipe.Register();
		}
	}
}
