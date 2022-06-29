using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BowsMisc.GemBows.Emerald_Bow
{
	public class Emerald_Bow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Emerald Bow");
			Tooltip.SetDefault("Turns wooden arrows into emerald arrows\nEmerald arrows occasionally explode upon hitting enemies");
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
			Item.damage = 13;
			Item.shootSpeed = 8f;
			Item.knockBack = 0.5f;
			Item.rare = ItemRarityID.Blue;
			Item.noMelee = true;
			Item.value = Item.sellPrice(silver: 90);
			Item.DamageType = DamageClass.Ranged;
			Item.autoReuse = true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 40f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
				position += muzzleOffset;
			if (type == ProjectileID.WoodenArrowFriendly)
				type = ModContent.ProjectileType<Emerald_Arrow>();
			Vector2 perturbedSpeed = new Vector2(velocity).RotatedByRandom(MathHelper.ToRadians(8));
			Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
			return false;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-1, 0);

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SilverBow, 1);
			recipe.AddIngredient(ItemID.Emerald, 8);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe1 = CreateRecipe();
			recipe1.AddIngredient(ItemID.TungstenBow, 1);
			recipe1.AddIngredient(ItemID.Emerald, 8);
			recipe1.AddTile(TileID.Anvils);
			recipe1.Register();
		}
	}
}
