using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Bullet;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Sets.GunsMisc.Belcher
{
	public class BottomFeederGun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Belcher");
			Tooltip.SetDefault("Converts regular bullets into clumps of rotting flesh");
		}

		public override void SetDefaults()
		{
			Item.damage = 7;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 28;
			Item.height = 14;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 0f;
			Item.useTurn = false;
			Item.value = Item.sellPrice(0, 1, 32, 0);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.NPCHit18;
			Item.autoReuse = true;
			Item.shoot = ProjectileID.PurificationPowder;
			Item.shootSpeed = 7.5f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-6, 0);

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 37f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) 
				position += muzzleOffset;

			int bloodproj = Main.rand.Next(new int[] {
				ModContent.ProjectileType<FriendlyFeeder>(),
				ModContent.ProjectileType<FriendlyFeeder.FriendlyFeeder2>(),
				ModContent.ProjectileType<FriendlyFeeder.FriendlyFeeder3>()
			});
			if (type == ProjectileID.Bullet)
				type = bloodproj;

			double randomAngle = Math.Atan2(speedX, speedY) + (Main.rand.NextFloat() - 0.5f) * MathHelper.ToRadians(30);
			speedX = Item.shootSpeed * (float)Math.Sin(randomAngle);
			speedY = Item.shootSpeed * (float)Math.Cos(randomAngle);
			return true;
		}
	}
}