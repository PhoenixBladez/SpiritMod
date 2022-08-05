using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Sets.BowsMisc.GemBows.Diamond_Bow
{
	public class Diamond_Bow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Diamond Bow");
			Tooltip.SetDefault("Turns wooden arrows into diamond arrows\nDiamond arrows pierce an additional enemy");
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 25;
			Item.useTime = 25;
			Item.width = 12;
			Item.height = 28;
			Item.shoot = ProjectileID.WoodenArrowFriendly;
			Item.useAmmo = AmmoID.Arrow;
			Item.UseSound = SoundID.Item5;
			Item.damage = 16;
			Item.shootSpeed = 9f;
			Item.knockBack = 0.5f;
			Item.rare = ItemRarityID.Green;
			Item.noMelee = true;
            Item.value = Item.sellPrice(0, 1, 80, 0);
            Item.DamageType = DamageClass.Ranged;
			Item.autoReuse = true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 40f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
				position += muzzleOffset;
			if (type == ProjectileID.WoodenArrowFriendly)
				type = ModContent.ProjectileType<Diamond_Arrow>();
			Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(8));
			Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
			return false;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-1, 0);

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.GoldBow, 1);
			recipe.AddIngredient(ItemID.Diamond, 8);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe1 = CreateRecipe();
			recipe1.AddIngredient(ItemID.PlatinumBow, 1);
			recipe1.AddIngredient(ItemID.Diamond, 8);
			recipe1.AddTile(TileID.Anvils);
			recipe1.Register();
		}
	}
}
