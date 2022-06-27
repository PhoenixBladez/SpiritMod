using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.DataStructures;

namespace SpiritMod.Items.Sets.BismiteSet
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
			Item.damage = 10;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 20;
			Item.height = 46;
			Item.useTime = 21;
			Item.useAnimation = 21;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ProjectileID.Shuriken;
			Item.useAmmo = AmmoID.Arrow;
			Item.knockBack = 1;
			Item.useTurn = false;
			Item.value = Terraria.Item.sellPrice(0, 0, 20, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = false;
			Item.shootSpeed = 6.5f;
			Item.crit = 8;
			Item.reuseDelay = 20;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<BismiteCrystal>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			int proj = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
			Main.projectile[proj].GetGlobalProjectile<SpiritGlobalProjectile>().shotFromBismiteBow = true;
			Projectile projectile = Main.projectile[proj];
			for (int k = 0; k < 25; k++) {
				Vector2 mouse = Main.MouseWorld;
				Vector2 offset = mouse - player.position;
				offset.Normalize();
				offset *= 15f;
				int dust = Dust.NewDust(projectile.Center + offset, projectile.width / 2, projectile.height / 2, DustID.Plantera_Green);

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