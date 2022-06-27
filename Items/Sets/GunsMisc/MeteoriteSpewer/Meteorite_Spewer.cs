using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GunsMisc.MeteoriteSpewer
{
	public class Meteorite_Spewer : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magmaspewer");
			Tooltip.SetDefault("Spews meteorites that linger for a short time\n50% chance to not consume ammo");
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 66;
			Item.useTime = 22;
			Item.width = 44;
			Item.reuseDelay = 15;
			Item.height = 14;
			Item.shoot = ProjectileID.PurificationPowder;
			Item.useAmmo = AmmoID.Bullet;
			Item.damage = 13;
			Item.shootSpeed = 6f;
			Item.noMelee = true;
			Item.value = Item.sellPrice(silver: 50);
			Item.knockBack = 3.2f;
			Item.rare = ItemRarityID.Blue;
			Item.DamageType = DamageClass.Ranged;
		}

		public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.NextFloat() >= .50f;
		public override Vector2? HoldoutOffset() => new Vector2(-11, -2);

		
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 30f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
				position += muzzleOffset;

			Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(5));
			Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<Meteorite_Spew>(), damage, knockback, player.whoAmI);
			int num2 = Main.rand.Next(10, 30);
            for (int index1 = 0; index1 < num2; ++index1)
            {
                int index2 = Dust.NewDust(position - muzzleOffset, 0, 0, DustID.Torch, 0.0f, 0.0f, 100, new Color(), 1.6f);
                Main.dust[index2].velocity *= 1.2f;
                --Main.dust[index2].velocity.Y;
                Main.dust[index2].velocity += velocity;
                Main.dust[index2].noGravity = true;
            }
			return false;
		}
	}
}
