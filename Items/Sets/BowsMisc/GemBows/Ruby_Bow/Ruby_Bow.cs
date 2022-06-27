using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Sets.BowsMisc.GemBows.Ruby_Bow
{
	public class Ruby_Bow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ruby Bow");
			Tooltip.SetDefault("Turns wooden arrows into ruby arrows\nRuby arrows bounce once");
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 26;
			Item.useTime = 26;
			Item.width = 12;
			Item.height = 28;
			Item.shoot = ProjectileID.WoodenArrowFriendly;
			Item.useAmmo = AmmoID.Arrow;
			Item.UseSound = SoundID.Item5;
			Item.damage = 14;
			Item.shootSpeed = 8f;
			Item.knockBack = 0.5f;
			Item.rare = ItemRarityID.Blue;
			Item.noMelee = true;
            Item.value = Item.sellPrice(0, 1, 35, 0);
            Item.DamageType = DamageClass.Ranged;
			Item.autoReuse = true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 40f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
				position += muzzleOffset;
			if (type == ProjectileID.WoodenArrowFriendly)
				type = ModContent.ProjectileType<Ruby_Arrow>();
			Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(8));
			Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
			return false;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-1, 0);

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SilverBow, 1);
			recipe.AddIngredient(ItemID.Ruby, 8);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe1 = CreateRecipe();
			recipe1.AddIngredient(ItemID.TungstenBow, 1);
			recipe1.AddIngredient(ItemID.Ruby, 8);
			recipe1.AddTile(TileID.Anvils);
			recipe1.Register();
		}
	}
}
