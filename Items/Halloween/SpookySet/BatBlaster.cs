using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Bullet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Halloween.SpookySet
{
	public class BatBlaster : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bat Blaster");
			Tooltip.SetDefault("Turns bullets into bats");
		}


		public override void SetDefaults()
		{
			Item.damage = 55;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 65;
			Item.height = 21;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 6;
			Item.useTurn = false;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item36;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<BatBullet>();
			Item.shootSpeed = 6.8f;
			Item.useAmmo = AmmoID.Bullet;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Vector2 origVect = velocity;
			Vector2 newVect;

			if (Main.rand.Next(2) == 1) {
				newVect = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(150, 1800) / 10));
			}
			else {
				newVect = origVect.RotatedBy(-System.Math.PI / (Main.rand.Next(150, 1800) / 10));
			}
			
			Projectile.NewProjectile(source, position.X, position.Y, newVect.X, newVect.Y, ModContent.ProjectileType<BatBullet>(), damage, knockback, player.whoAmI);
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.SpookyWood, 14);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}
	}
}
