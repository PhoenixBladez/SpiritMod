using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Thrown
{
	public class AmberSlasher : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Amber Slasher");
			Tooltip.SetDefault("Causes another Amber Slasher to strike foes when hitting enemies");
		}


		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 20;
			item.value = Item.sellPrice(0, 0, 3, 0);
			item.rare = ItemRarityID.LightPurple;
			item.maxStack = 999;
			item.crit = 4;
			item.damage = 57;
			item.knockBack = 3;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = item.useAnimation = 16;
			item.ranged = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.consumable = true;
			item.noUseGraphic = true;
			item.shoot = ModContent.ProjectileType<Projectiles.Thrown.AmberSlasher>();
			item.shootSpeed = 11;
			item.UseSound = SoundID.Item1;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DuneEssence>(), 4);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.EssenceDistorter>());
			recipe.SetResult(this, 100);
			recipe.AddRecipe();
		}
	}
}