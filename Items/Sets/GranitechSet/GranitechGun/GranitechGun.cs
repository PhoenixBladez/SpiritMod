using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Bullet;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GranitechSet.GranitechGun
{
	public class GranitechGun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Granitech Blastre");
			Tooltip.SetDefault("shoot bulet");
		}

		public override void SetDefaults()
		{
			item.damage = 37;
			item.width = 28;
			item.height = 14;
			item.useTime = item.useAnimation = 24;
			item.knockBack = 0f;
			item.shootSpeed = 24;
			item.noMelee = true;
			item.autoReuse = true;
			item.ranged = true;
			item.useTurn = false;
			item.value = Item.sellPrice(0, 1, 32, 0);
			item.rare = ItemRarityID.Orange;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ProjectileID.PurificationPowder;
			item.UseSound = SoundID.NPCHit18;
			item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-6, 0);

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * (item.width / 2f);
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) 
				position += muzzleOffset;

			VFX(position + muzzleOffset, new Vector2(speedX, speedY) * 0.2f);

			var p = Projectile.NewProjectileDirect(position, new Vector2(speedX, speedY).RotatedByRandom(0.03f), ModContent.ProjectileType<GranitechGunBullet>(), damage, knockBack, player.whoAmI);
			p.GetGlobalProjectile<GranitechGunGlobalProjectile>().spawnedByGranitechGun = true;
			return false;
		}

		private void VFX(Vector2 position, Vector2 velocity)
		{
			for (int i = 0; i < 4; ++i)
			{
				Vector2 vel = velocity.RotatedByRandom(MathHelper.ToRadians(30)) * Main.rand.NextFloat(0.5f, 1.2f);
				var d = Dust.NewDustPerfect(position, ModContent.DustType<GranitechGunDust>(), vel);
				GranitechGunDust.RandomizeFrame(d);
			}
		}
	}
}