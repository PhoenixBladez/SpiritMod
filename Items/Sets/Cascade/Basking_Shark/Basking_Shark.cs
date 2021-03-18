using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.Cascade.Basking_Shark
{
	public class Basking_Shark : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Basking Shark");
			Tooltip.SetDefault("");
		}

		public override void SetDefaults()
		{
			item.useStyle = 5;
			item.autoReuse = true;
			item.useAnimation = 30;
			item.useTime = 10;
			item.width = 38;
			item.height = 6;
			item.shoot = 10;
			item.damage = 13;
			item.shootSpeed = 9f;
			item.noMelee = true;
			item.reuseDelay = 45;
			item.value = Item.sellPrice(silver: 70);
			item.knockBack = 2f;
			item.useAmmo = AmmoID.Bullet;
			item.ranged = true;
			item.rare = 2;
		}
		
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-4, 2);
		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 40f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			if (type == ProjectileID.Bullet)
			{
				type = mod.ProjectileType("Basking_Shark_Projectile");
			}
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(8));
			speedX = perturbedSpeed.X;
			speedY = perturbedSpeed.Y;
			return true;
		}
	}
}
