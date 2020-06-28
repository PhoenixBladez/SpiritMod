using SpiritMod.Items.Material;
using SpiritMod.Tiles.Furniture;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Thrown
{
	public class AmbertuskSpear : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ambertusk Spear");
			Tooltip.SetDefault("Enemies hit are afflicted by a damaging debuff");
		}


		public override void SetDefaults()
		{
			item.width = item.height = 42;
			item.rare = ItemRarityID.LightPurple;
			item.maxStack = 999;
			item.crit = 10;
			item.damage = 60;
			item.value = Terraria.Item.sellPrice(0, 0, 5, 0);
			item.knockBack = 9;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = item.useAnimation = 24;
			item.melee = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.consumable = true;
			item.noUseGraphic = true;
			item.shoot = ModContent.ProjectileType<Projectiles.Thrown.AmbertuskSpear>();
			item.shootSpeed = 15;
			item.UseSound = SoundID.Item1;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DuneEssence>(), 5);
			recipe.AddTile(ModContent.TileType<EssenceDistorter>());
			recipe.SetResult(this, 100);
			recipe.AddRecipe();
		}
	}
}