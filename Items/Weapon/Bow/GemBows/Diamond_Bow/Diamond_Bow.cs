using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow.GemBows.Diamond_Bow
{
	public class Diamond_Bow : ModItem
	{
		public override void SetDefaults()
		{
			item.useStyle = 5;
			item.useAnimation = 25;
			item.useTime = 25;
			item.width = 12;
			item.height = 28;
			item.shoot = 1;
			item.useAmmo = AmmoID.Arrow;
			item.UseSound = SoundID.Item5;
			item.damage = 16;
			item.shootSpeed = 9f;
			item.knockBack = 0.5f;
			item.rare = 2;
			item.noMelee = true;
            item.value = Terraria.Item.sellPrice(0, 1, 80, 0);
            item.ranged = true;
			item.autoReuse = true;
		}
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Diamond Bow");
			Tooltip.SetDefault("Turns wooden arrows into diamond arrows\nDiamond arrows pierce an additional enemy");
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 40f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			if (type == ProjectileID.WoodenArrowFriendly)
			{
				type = mod.ProjectileType("Diamond_Arrow");
			}
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(8));
			Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			return false;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-1, 0);
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.GoldBow, 1);
			recipe.AddIngredient(ItemID.Diamond, 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();

			ModRecipe recipe1 = new ModRecipe(mod);
			recipe1.AddIngredient(ItemID.PlatinumBow, 1);
			recipe1.AddIngredient(ItemID.Diamond, 8);
			recipe1.AddTile(TileID.Anvils);
			recipe1.SetResult(this);
			recipe1.AddRecipe();
		}
	}
}
