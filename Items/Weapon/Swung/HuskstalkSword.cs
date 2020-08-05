using SpiritMod.Buffs;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
	public class HuskstalkSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Huskstalk Sword");
		}


		public override void SetDefaults()
		{
			item.damage = 10;
			item.melee = true;
			item.width = 32;
			item.height = 32;
			item.useTime = 25;
			item.useAnimation = 25;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 5;
			item.rare = 0;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.value = Item.sellPrice(0, 0, 0, 20);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<AncientBark>(), 7);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}