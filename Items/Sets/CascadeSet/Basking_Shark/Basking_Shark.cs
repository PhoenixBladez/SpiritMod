using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Sets.FloatingItems;

namespace SpiritMod.Items.Sets.CascadeSet.Basking_Shark
{
	public class Basking_Shark : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Basking Shark");
			Tooltip.SetDefault("Converts bullets into chum chunks");
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
			Item.useAnimation = 39;
			Item.useTime = 13;
			Item.width = 38;
			Item.height = 6;
			Item.shoot = ProjectileID.PurificationPowder;
			Item.damage = 8;
			Item.shootSpeed = 8f;
			Item.noMelee = true;
			Item.reuseDelay = 45;
			Item.value = Item.sellPrice(silver: 30);
			Item.knockBack = .25f;
			Item.useAmmo = AmmoID.Bullet;
			Item.DamageType = DamageClass.Ranged;
			Item.rare = ItemRarityID.Blue;
		}

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<DeepCascadeShard>(), 12);
            recipe.AddIngredient(ModContent.ItemType<Kelp>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

		public override Vector2? HoldoutOffset() => new Vector2(-4, 2);

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 40f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
				position += muzzleOffset;

			if (type == ProjectileID.Bullet)
				type = ModContent.ProjectileType<Basking_Shark_Projectile>();

			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(8));
		}
	}
}
