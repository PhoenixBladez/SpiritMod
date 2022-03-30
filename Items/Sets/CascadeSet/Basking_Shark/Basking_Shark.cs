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
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.autoReuse = true;
			item.useAnimation = 39;
			item.useTime = 13;
			item.width = 38;
			item.height = 6;
			item.shoot = ProjectileID.PurificationPowder;
			item.damage = 8;
			item.shootSpeed = 8f;
			item.noMelee = true;
			item.reuseDelay = 45;
			item.value = Item.sellPrice(silver: 30);
			item.knockBack = .25f;
			item.useAmmo = AmmoID.Bullet;
			item.ranged = true;
			item.rare = ItemRarityID.Blue;
		}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<DeepCascadeShard>(), 12);
            recipe.AddIngredient(ModContent.ItemType<Kelp>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

		public override Vector2? HoldoutOffset() => new Vector2(-4, 2);

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 40f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
				position += muzzleOffset;
			if (type == ProjectileID.Bullet)
				type = ModContent.ProjectileType<Basking_Shark_Projectile>();
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(8));
			speedX = perturbedSpeed.X;
			speedY = perturbedSpeed.Y;
			return true;
		}
	}
}
