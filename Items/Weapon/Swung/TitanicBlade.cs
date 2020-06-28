using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Sword;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
	public class TitanicBlade : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Titanic Blade");
			Tooltip.SetDefault("Shoots out a mass of slowing water");
		}


		public override void SetDefaults()
		{
			item.width = 54;
			item.height = 50;
			item.rare = ItemRarityID.LightPurple;

			item.crit += 4;
			item.damage = 57;
			item.knockBack = 6;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = item.useAnimation = 19;

			item.melee = true;
			item.autoReuse = true;

			item.shoot = ModContent.ProjectileType<WaterMass>();
			item.shootSpeed = 12;
			item.UseSound = SoundID.Item1;
			item.value = Terraria.Item.sellPrice(0, 6, 0, 0);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<TidalEssence>(), 16);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.EssenceDistorter>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}