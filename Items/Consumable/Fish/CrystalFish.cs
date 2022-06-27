
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Fish
{
	public class CrystalFish : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystallized Salmon");
			Tooltip.SetDefault("Minor improvements to all stats\nBoosts magic power");
		}


		public override void SetDefaults()
		{
			Item.width = Item.height = 22;
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 99;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useTime = Item.useAnimation = 30;

			Item.buffType = BuffID.WellFed;
			Item.buffTime = 72000;
			Item.noMelee = true;
			Item.consumable = true;
			Item.UseSound = SoundID.Item2;
			Item.autoReuse = false;

		}
		public override bool CanUseItem(Player player)
		{
			player.AddBuff(BuffID.MagicPower, 9860);
			return true;
		}
		public override void AddRecipes()
		{
			Recipe recipe1 = CreateRecipe(1);
			recipe1.AddIngredient(ModContent.ItemType<RawFish>(), 1);
			recipe1.AddIngredient(ItemID.CrystalShard, 1);
			recipe1.AddTile(TileID.CookingPots);
			recipe1.Register();
		}
	}
}
