using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using SpiritMod.Items.Material;
using SpiritMod.Items.Sets.SpiritSet;
using SpiritMod.Items.Sets.HuskstalkSet;
using SpiritMod.Projectiles;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using SpiritMod.Items;

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
			Item.damage = 65;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 22;
			Item.height = 46;
			Item.useTime = 19;
			Item.useAnimation = 19;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ProjectileID.WoodenArrowFriendly;
			Item.useAmmo = AmmoID.Arrow;
			Item.knockBack = 7;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.useTurn = false;
			Item.shootSpeed = 14.2f;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
				position += muzzleOffset;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (type == ProjectileID.WoodenArrowFriendly)
			{
				var p = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
				if (Main.rand.Next(3) == 1)
					p.GetGlobalProjectile<SpiritGlobalProjectile>().effects.Add(new HeroBowFireEffect());
				else if (Main.rand.Next(2) == 1)
					p.GetGlobalProjectile<SpiritGlobalProjectile>().effects.Add(new HeroBowIceEffect());
				else
					p.GetGlobalProjectile<SpiritGlobalProjectile>().effects.Add(new HeroBowLightEffect());
				return false;
			}
			else
			{
				return true;
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Ectoplasm, 8);
			recipe.AddIngredient(ModContent.ItemType<AncientBark>(), 10);
			recipe.AddIngredient(ModContent.ItemType<OldLeather>(), 10);
			recipe.AddIngredient(ModContent.ItemType<SpiritBar>(), 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}

	public class HeroBowFireEffect : SpiritProjectileEffect
	{
		public override bool ProjectilePreAI(Projectile projectile)
		{
			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Torch);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity *= 0f;
			Main.dust[dust].scale = 1.5f;
			return true;
		}

		public override void ProjectileOnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.OnFire, 240, true);

			if (Main.rand.NextBool(4)) {
				target.AddBuff(BuffID.CursedInferno, 180, true);
			}
			if (Main.rand.NextBool(8)) {
				target.AddBuff(BuffID.ShadowFlame, 180, true);
			}
		}
	}

	public class HeroBowIceEffect : SpiritProjectileEffect
	{
		public override bool ProjectilePreAI(Projectile projectile)
		{
			if (Main.rand.NextBool()) {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.IceTorch);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].scale = 1.5f;
			}
			return true;
		}

		public override void ProjectileOnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Frostburn, 120, true);

			if (Main.rand.NextBool(15)) {
				target.AddBuff(ModContent.BuffType<MageFreeze>(), 180, true);
			}
		}
	}

	public class HeroBowLightEffect : SpiritProjectileEffect
	{
		public override bool ProjectilePreAI(Projectile projectile)
		{
			if (Main.rand.NextBool()) {
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.GoldCoin);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].scale = 1.8f;
			}
			return true;
		}

		public override void ProjectileOnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
		{
			if (!target.boss && Main.rand.NextBool(50)) {
				target.AddBuff(ModContent.BuffType<Death>(), 240, true);
			}
		}
	}
}