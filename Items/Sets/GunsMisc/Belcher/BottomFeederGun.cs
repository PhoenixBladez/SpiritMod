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
			item.damage = 7;
			item.ranged = true;
			item.width = 28;
			item.height = 14;
			item.useTime = 12;
			item.useAnimation = 12;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 0f;
			item.useTurn = false;
			item.value = Item.sellPrice(0, 1, 32, 0);
			item.rare = ItemRarityID.Orange;
			item.UseSound = SoundID.NPCHit18;
			item.autoReuse = true;
			item.shoot = ProjectileID.PurificationPowder;
			item.shootSpeed = 7.5f;
			item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-6, 0);

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
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
			speedX = item.shootSpeed * (float)Math.Sin(randomAngle);
			speedY = item.shootSpeed * (float)Math.Cos(randomAngle);
			return true;
		}
	}
}