
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
			item.width = item.height = 22;
			item.rare = 1;
			item.maxStack = 99;
			item.noUseGraphic = true;
			item.useStyle = 2;
			item.useTime = item.useAnimation = 30;

			item.buffType = BuffID.WellFed;
			item.buffTime = 72000;
			item.noMelee = true;
			item.consumable = true;
			item.UseSound = SoundID.Item2;
			item.autoReuse = false;

		}
		public override bool CanUseItem(Player player)
		{
			player.AddBuff(BuffID.MagicPower, 9860);
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe1 = new ModRecipe(mod);
			recipe1.AddIngredient(ModContent.ItemType<RawFish>(), 1);
			recipe1.AddIngredient(ItemID.CrystalShard, 1);
			recipe1.AddTile(TileID.CookingPots);
			recipe1.SetResult(this, 1);
			recipe1.AddRecipe();
		}
	}
}
