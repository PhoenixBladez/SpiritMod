using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Returning
{
	public class Florang : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floran Cutter");
			Tooltip.SetDefault("Rolls along the ground, cutting up enemies \nVines occasionally ensnare the foes, reducing their movement speed \n'Sharp as a razorleaf'");
		}


		public override void SetDefaults()
		{
			item.damage = 21;
			item.melee = true;
			item.width = 44;
			item.height = 40;
			item.useTime = 5;
			item.useAnimation = 50;
			item.noUseGraphic = true;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 0;
			item.value = Terraria.Item.sellPrice(0, 0, 15, 0);
			item.rare = 1;
			item.shootSpeed = 5f;
			item.shoot = mod.ProjectileType("FloraP");
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (speedX > 0) {
				speedX = 2;
			}
			else {
				speedX = -2;
			}
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<FloranBar>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}