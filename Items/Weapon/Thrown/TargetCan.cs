using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class TargetCan : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Target Can");
			Tooltip.SetDefault("Hit it with a bullet in the air to do extremely high damage \n'Let's see what kind of a shot ya are, pilgrim!'");
		}
		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.width = 9;
			item.height = 15;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.ranged = true;
			item.noMelee = true;
			item.consumable = true;
			item.maxStack = 999;
			item.shoot = ModContent.ProjectileType<Projectiles.Thrown.TargetCan>();
			item.useAnimation = 25;
			item.useTime = 25;
			item.shootSpeed = 8.5f;
			item.damage = 0;
			item.knockBack = 1.5f;
			item.value = Terraria.Item.sellPrice(0, 0, 0, 20);
			item.crit = 8;
			item.rare = 1;
			item.autoReuse = true;
			item.maxStack = 999;
			item.consumable = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(2339, 1);
			recipe.AddTile(16);
			recipe.SetResult(this, 5);
			recipe.AddRecipe();
		}
	}
}
