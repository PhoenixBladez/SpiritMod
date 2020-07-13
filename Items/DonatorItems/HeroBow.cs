using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
	public class HeroBow : SpiritItem
	{
		public override string SetDisplayName => "Hero's Bow";
		public override string SetTooltip => "Wooden arrows are infused with the elements\n"
				+ "- Fire arrows can inflict multiple different burns\n"
				+ "- Ice arrows can freeze and frostburn\n"
				+ "- Light arrows have a 2% chance to instantly kill\n";

		public override void SetDefaults()
		{
			item.damage = 65;
			item.noMelee = true;
			item.ranged = true;
			item.width = 22;
			item.height = 46;
			item.useTime = 19;
			item.useAnimation = 19;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ProjectileID.WoodenArrowFriendly;
			item.useAmmo = AmmoID.Arrow;
			item.knockBack = 7;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.UseSound = SoundID.Item5;
			item.autoReuse = true;
			item.useTurn = false;
			item.shootSpeed = 14.2f;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 25f;
			if(Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) {
				position += muzzleOffset;
			}

			if(type == ProjectileID.WoodenArrowFriendly) {
				var p = Projectile.NewProjectileDirect(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
				if(Main.rand.Next(3) == 1) {
					p.GetGlobalProjectile<SpiritGlobalProjectile>().effects.Add(new HeroBowFireEffect());
				} else if(Main.rand.Next(2) == 1) {
					p.GetGlobalProjectile<SpiritGlobalProjectile>().effects.Add(new HeroBowIceEffect());
				} else {
					p.GetGlobalProjectile<SpiritGlobalProjectile>().effects.Add(new HeroBowLightEffect());
				}
				return false;
			} else {
				return true;
			}
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Ectoplasm, 8);
			recipe.AddIngredient(ModContent.ItemType<AncientBark>(), 10);
			recipe.AddIngredient(ModContent.ItemType<OldLeather>(), 10);
			recipe.AddIngredient(ModContent.ItemType<SpiritBar>(), 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class HeroBowFireEffect : SpiritProjectileEffect
	{
		public override bool ProjectilePreAI(Projectile projectile)
		{
			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity *= 0f;
			Main.dust[dust].scale = 1.5f;
			return true;
		}

		public override void ProjectileOnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.OnFire, 240, true);

			if(Main.rand.NextBool(4)) {
				target.AddBuff(BuffID.CursedInferno, 180, true);
			}
			if(Main.rand.NextBool(8)) {
				target.AddBuff(BuffID.ShadowFlame, 180, true);
			}
		}
	}

	public class HeroBowIceEffect : SpiritProjectileEffect
	{
		public override bool ProjectilePreAI(Projectile projectile)
		{
			if(Main.rand.NextBool()) {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 135);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].scale = 1.5f;
			}
			return true;
		}

		public override void ProjectileOnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Frostburn, 120, true);

			if(Main.rand.NextBool(15)) {
				target.AddBuff(ModContent.BuffType<MageFreeze>(), 180, true);
			}
		}
	}

	public class HeroBowLightEffect : SpiritProjectileEffect
	{
		public override bool ProjectilePreAI(Projectile projectile)
		{
			if(Main.rand.NextBool()) {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.GoldCoin);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].scale = 1.8f;
			}
			return true;
		}

		public override void ProjectileOnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
		{
			if(!target.boss && Main.rand.NextBool(50)) {
				target.AddBuff(ModContent.BuffType<Death>(), 240, true);
			}
		}
	}
}