﻿using SpiritMod.Items.Accessory;
using SpiritMod.Projectiles.Clubs;
using SpiritMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SpiritMod.Items.Accessory.MeleeCharmTree;
using SpiritMod.Items.Sets.DuskingDrops;
using System;
using SpiritMod.Projectiles;
using SpiritMod.Projectiles.Summon.CimmerianStaff;

namespace SpiritMod.Players
{
	public class MiscAccessoryPlayer : ModPlayer
	{
		/// <summary>Allows you to modify the knockback given ANY damage source. NOTE: This is an IL hook, which is why it needs a Player instance and is static.</summary>
		/// <param name="player">The specific player to change.</param>
		/// <param name="horizontal">Whether this is a horizontal (velocity.X) change or a vertical (velocity.Y) change.</param>
		public static float KnockbackMultiplier(Player player, bool horizontal)
		{
			float totalKb = 1f;

			// Frost Giant Belt
			if (player.AccessoryEquipped<FrostGiantBelt>() && player.channel)
				if (HeldItemIsClub(player))
					totalKb *= 0.5f;

			if (totalKb < 0.001f) //Throws NullReferenceException if it's 0 for some reason
				totalKb = 0.001f;
			return totalKb;
		}

		public override void ModifyWeaponDamage(Item item, ref float add, ref float mult, ref float flat)
		{
			// Frost Giant Belt
			if (HeldItemIsClub(player))
				mult += item.knockBack / 5f;
		}

		public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
		{
			// Twilight Talisman & Shadow Gauntlet
			if ((player.AccessoryEquipped<Twilight1>() && Main.rand.NextBool(13)) || (player.AccessoryEquipped<ShadowGauntlet>() && Main.rand.NextBool(2)))
				target.AddBuff(BuffID.ShadowFlame, 180);
		}

		public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
		{
			OnHitNPCWithAnything(item, target, damage, knockback, crit);
		}

		public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			// Twilight Talisman & Shadow Gauntlet
			bool shadowFlameCondition = (player.AccessoryEquipped<Twilight1>() && Main.rand.NextBool(15)) || (player.AccessoryEquipped<ShadowGauntlet>() && proj.melee && Main.rand.NextBool(2));
			AddBuffWithCondition(shadowFlameCondition, target, BuffID.ShadowFlame, 180);
		}

		public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
		{
			OnHitNPCWithAnything(proj, target, damage, knockback, crit);
		}

		public void OnHitNPCWithAnything(Entity weapon, NPC target, int damage, float knockback, bool crit)
		{
			// Hell Charm
			if (player.AccessoryEquipped<HCharm>() && Main.rand.Next(11) == 0)
			{
				if ((weapon is Item i && i.melee) || weapon is Projectile)
					target.AddBuff(BuffID.OnFire, 120);
			}

			// Amazon Charm
			if (player.AccessoryEquipped<YoyoCharm2>() && Main.rand.Next(11) == 0)
			{
				if ((weapon is Item i && i.melee) || weapon is Projectile)
					target.AddBuff(BuffID.Poisoned, 120);
			}
		}

		public override void OnHitAnything(float x, float y, Entity victim)
		{
		}

		private void AddBuffWithCondition(bool condition, NPC p, int id, int ticks) { if (condition) p.AddBuff(id, ticks); }

		public override void PostUpdateEquips()
		{
			// Shadow Gauntlet
			if (player.AccessoryEquipped<ShadowGauntlet>())
			{
				player.kbGlove = true;
				player.meleeDamage += 0.07F;
				player.meleeSpeed += 0.07F;
			}

			// Cimmerian Scepter
			if (!player.dead && player.AccessoryEquipped<CimmerianScepter>() && player.ownedProjectileCounts[ModContent.ProjectileType<CimmerianScepterProjectile>()] < 1)
				Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<CimmerianScepterProjectile>(), (int)(22 * player.minionDamage), 1.5f, player.whoAmI);
		}

		public override void MeleeEffects(Item item, Rectangle hitbox)
		{
			// Shadow Gauntlet
			if (player.AccessoryEquipped<ShadowGauntlet>() && item.melee)
				Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Shadowflame);
		}

		public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
		{
			// Spectre Ring
			if (player.AccessoryEquipped<SpectreRing>())
			{
				for (int h = 0; h < 3; h++)
				{
					Vector2 vel = new Vector2(0, -1);
					float rand = Main.rand.NextFloat() * MathHelper.TwoPi;
					vel = vel.RotatedBy(rand);
					vel *= 2f;
					Projectile.NewProjectile(Main.player[Main.myPlayer].Center, vel, ProjectileID.LostSoulFriendly, 45, 0, Main.myPlayer);
				}
			}
		}

		public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
		{
			// Spectre Ring
			if (player.AccessoryEquipped<SpectreRing>())
			{
				int newProj = Projectile.NewProjectile(player.Center, new Vector2(6, 6), ProjectileID.SpectreWrath, 40, 0f, Main.myPlayer);

				int dist = 800;
				int target = -1;
				for (int i = 0; i < 200; ++i)
				{
					if (Main.npc[i].active && Main.npc[i].CanBeChasedBy(Main.projectile[newProj], false))
					{
						if ((Main.npc[i].Center - Main.projectile[newProj].Center).LengthSquared() < dist * dist)
						{
							target = i;
							break;
						}
					}
				}

				Main.projectile[newProj].ai[0] = target;
			}
		}

		/// <summary>A bit of a wacky way of checking if a held item is a club, but it works.</summary>
		/// <param name="player">Player to check held item of.</param>
		public static bool HeldItemIsClub(Player player)
		{
			Item heldItem = player.HeldItem;
			if (heldItem.shoot > ProjectileID.None && heldItem.modItem != null && heldItem.modItem.mod == ModContent.GetInstance<SpiritMod>())
			{
				var p = new Projectile();
				p.SetDefaults(player.HeldItem.shoot);

				if (p.modProjectile != null && p.modProjectile is ClubProj)
					return true;
			}
			return false;
		}
	}
}