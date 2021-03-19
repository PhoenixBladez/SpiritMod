using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
	public class BismiteBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bismite Bow");
			Tooltip.SetDefault("Occasionally causes foes to receive 'Festering Wounds,' which deal more damage to enemies under half health");
		}



		public override void SetDefaults()
		{
			item.damage = 10;
			item.noMelee = true;
			item.ranged = true;
			item.width = 20;
			item.height = 46;
			item.useTime = 21;
			item.useAnimation = 21;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ProjectileID.Shuriken;
			item.useAmmo = AmmoID.Arrow;
			item.knockBack = 1;
			item.useTurn = false;
			item.value = Terraria.Item.sellPrice(0, 0, 20, 0);
			item.rare = 1;
			item.UseSound = SoundID.Item5;
			item.autoReuse = false;
			item.shootSpeed = 6.5f;
			item.crit = 8;
			item.reuseDelay = 20;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<BismiteCrystal>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
			Main.projectile[proj].GetGlobalProjectile<SpiritGlobalProjectile>().shotFromBismiteBow = true;
			Projectile projectile = Main.projectile[proj];
			for (int k = 0; k < 25; k++) {
				Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
				Vector2 offset = mouse - player.position;
				offset.Normalize();
				offset *= 15f;
				int dust = Dust.NewDust(projectile.Center + offset, projectile.width / 2, projectile.height / 2, 167);

				Main.dust[dust].velocity *= -1f;
				Main.dust[dust].noGravity = true;
				//        Main.dust[dust].scale *= 2f;
				Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
				vector2_1.Normalize();
				Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.02f);
				Main.dust[dust].velocity = vector2_2;
				vector2_2.Normalize();
				Vector2 vector2_3 = vector2_2 * 10f;
				Main.dust[dust].position = (projectile.Center + offset) + vector2_3;
			}
			return false;
		}
	}
}