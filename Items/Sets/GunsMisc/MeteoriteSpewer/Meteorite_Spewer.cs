using Microsoft.Xna.Framework;
using Terraria;
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
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 66;
			item.useTime = 22;
			item.width = 44;
			item.reuseDelay = 15;
			item.height = 14;
			item.shoot = ProjectileID.PurificationPowder;
			item.useAmmo = AmmoID.Bullet;
			item.damage = 13;
			item.shootSpeed = 6f;
			item.noMelee = true;
			item.value = Item.sellPrice(silver: 50);
			item.knockBack = 3.2f;
			item.rare = ItemRarityID.Blue;
			item.ranged = true;
		}
		public override bool ConsumeAmmo(Player player) => Main.rand.NextFloat() >= .50f;
		public override Vector2? HoldoutOffset() => new Vector2(-11, -2);
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 30f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5));
			Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("Meteorite_Spew"), damage, knockBack, player.whoAmI);
			int num2 = Main.rand.Next(10, 30);
            for (int index1 = 0; index1 < num2; ++index1)
            {
                int index2 = Dust.NewDust(position - muzzleOffset, 0, 0, DustID.Fire, 0.0f, 0.0f, 100, new Color(), 1.6f);
                Main.dust[index2].velocity *= 1.2f;
                --Main.dust[index2].velocity.Y;
                Main.dust[index2].velocity += new Vector2(speedX, speedY);
                Main.dust[index2].noGravity = true;
            }
			return false;
		}
	}
}
