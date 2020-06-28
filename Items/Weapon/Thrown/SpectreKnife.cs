using SpiritMod.Projectiles.Thrown;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class SpectreKnife : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spectre Knife");
			Tooltip.SetDefault("Upon hitting enemies or tiles, Spectre bolts are releaaed");
		}


		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 24;
			item.value = Terraria.Item.buyPrice(0, 30, 0, 0);
			item.rare = 8;
			item.maxStack = 999;
			item.damage = 65;
			item.knockBack = 3.5f;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 15;
			item.useAnimation = 15;
			item.ranged = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.consumable = true;
			item.noUseGraphic = true;
			item.shoot = ModContent.ProjectileType<SpectreKnifeProj>();
			item.shootSpeed = 11f;
			item.UseSound = SoundID.Item1;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SpectreBar, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 33);
			recipe.AddRecipe();
		}
	}
}