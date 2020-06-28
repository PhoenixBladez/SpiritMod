using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
	public class Quasar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quasar");
			Tooltip.SetDefault("Arrows turn into powerful, homing souls");
		}



		public override void SetDefaults()
		{
			item.damage = 74;
			item.noMelee = true;
			item.ranged = true;
			item.width = 24;
			item.height = 46;
			item.useTime = 14;
			item.useAnimation = 14;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = 3;
			item.useAmmo = AmmoID.Arrow;
			item.knockBack = 3;
			item.value = Terraria.Item.sellPrice(0, 10, 0, 0);
			item.rare = 8;
			item.UseSound = SoundID.Item5;
			item.autoReuse = true;
			item.shootSpeed = 8.8f;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int p = Terraria.Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ProjectileID.LostSoulFriendly, damage, knockBack, player.whoAmI, 0f, 0f);
			Main.projectile[p].magic = false;
			Main.projectile[p].ranged = true;
			return false;

		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<NightSky>(), 1);
			recipe.AddIngredient(ItemID.Ectoplasm, 16);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
