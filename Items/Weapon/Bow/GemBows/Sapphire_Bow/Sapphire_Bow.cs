using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace SpiritMod.Items.Weapon.Bow.GemBows.Sapphire_Bow
{
	public class Sapphire_Bow : ModItem
	{
		public override void SetDefaults()
		{
			item.useStyle = 5;
			item.useAnimation = 33-5;
			item.useTime = 33-5;
			item.width = 12;
			item.height = 28;
			item.shoot = 1;
			item.useAmmo = AmmoID.Arrow;
			item.UseSound = SoundID.Item5;
			item.damage = 12;
			item.shootSpeed = 9.75f;
			item.knockBack = 4.25f;
			item.rare = 1;
			item.noMelee = true;
			item.value = Item.sellPrice(silver: 20);
			item.ranged = true;
		}
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sapphire Bow");
			Tooltip.SetDefault("Turns wooden arrows into sapphire arrows\nSapphire arrows slightly home toward the cursor");
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 40f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) {
				position += muzzleOffset;
			}
			float angle = Main.rand.NextFloat(MathHelper.PiOver4, -MathHelper.Pi - MathHelper.PiOver4);
			Vector2 spawnPlace = Vector2.Normalize(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle))) * 20f;
			if (Collision.CanHit(position, 0, 0, position + spawnPlace, 0, 0)) {
			position += spawnPlace;
			}			
			if (type == ProjectileID.WoodenArrowFriendly)
			{
				type = mod.ProjectileType("Sapphire_Arrow");
			}
			Vector2 velocity = Vector2.Normalize(Main.MouseWorld - position) * item.shootSpeed;
			int p = Projectile.NewProjectile(position.X, position.Y, velocity.X, velocity.Y, type, damage, knockBack, 0, 0.0f, 0.0f);
				
			return false;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-1, 0);
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IronBow, 1);
			recipe.AddIngredient(ItemID.Sapphire, 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();

			ModRecipe recipe1 = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LeadBow, 1);
			recipe.AddIngredient(ItemID.Sapphire, 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
