using SpiritMod.Items.Accessory;
using SpiritMod.Projectiles.Clubs;
using SpiritMod.Utilities;
using SpiritMod.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SpiritMod.Items.Accessory.MeleeCharmTree;
using SpiritMod.Items.Sets.DuskingDrops;
using SpiritMod.Projectiles.Summon.CimmerianStaff;
using SpiritMod.Items.Accessory.MageTree;
using SpiritMod.Items.Sets.CascadeSet.Armor;
using System.Collections.Generic;
using SpiritMod.Items;
using System;
using System.Linq;

namespace SpiritMod.GlobalClasses.Players
{
	public class MiscAccessoryPlayer : ModPlayer
	{
		public readonly Dictionary<string, bool> accessory = new Dictionary<string, bool>();
		public readonly Dictionary<string, int> timers = new Dictionary<string, int>();

		public override void Initialize()
		{
			accessory.Clear();
			timers.Clear();

			var types = typeof(SpiritMod).Assembly.GetTypes(); //Add every accessory & timered item to this dict
			foreach (var type in types)
			{
				if (type.IsSubclassOf(typeof(AccessoryItem)) && !type.IsAbstract)
				{
					var item = Activator.CreateInstance(type) as AccessoryItem;
					accessory.Add(item.AccName, false);
				}

				if (typeof(ITimerItem).IsAssignableFrom(type) && !type.IsAbstract)
				{
					var item = Activator.CreateInstance(type) as ITimerItem;

					if (item.TimerCount() == 1)
						timers.Add(type.Name, 0);
					else
						for (int i = 0; i < item.TimerCount(); ++i)
							timers.Add(type.Name + i, 0);
				}
			}
		}

		public override void ResetEffects()
		{
			var accColl = accessory.Keys.ToList(); //Reset every acc
			foreach (string item in accColl)
				accessory[item] = false;

			var timerColl = timers.Keys.ToList(); //Decrement every timer
			foreach (string item in timerColl)
				timers[item]--;
		}

		public override void UpdateDead() => ResetEffects();

		/// <summary>Allows you to modify the knockback given ANY damage source. NOTE: This is an IL hook, which is why it needs a Player instance and is static.</summary>
		/// <param name="player">The specific player to change.</param>
		/// <param name="horizontal">Whether this is a horizontal (velocity.X) change or a vertical (velocity.Y) change.</param>
		public static float KnockbackMultiplier(Player player, bool horizontal)
		{
			float totalKb = 1f;

			// Frost Giant Belt
			if (player.HasAccessory<FrostGiantBelt>() && player.channel)
				if (HeldItemIsClub(player))
					totalKb *= 0.5f;

			// Cascade Chestplate
			if (player.ChestplateEquipped<CascadeChestplate>() && horizontal)
				totalKb *= 0.25f;

			if (totalKb < 0.001f) //Throws NullReferenceException if it's 0 for some reason
				totalKb = 0.001f;
			return totalKb;
		}

		public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
		{
			// Frost Giant Belt
			if (Player.HasAccessory<FrostGiantBelt>() && HeldItemIsClub(Player) && item.type == Player.HeldItem.type)
				mult += 1 + (item.knockBack / 30f);
		}

		public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
		{
			// Twilight Talisman & Shadow Gauntlet
			if ((Player.HasAccessory<Twilight1>() && Main.rand.NextBool(13)) || (Player.HasAccessory<ShadowGauntlet>() && Main.rand.NextBool(2)))
				target.AddBuff(BuffID.ShadowFlame, 180);

			// Ace of Spades
			if (Player.GetModPlayer<MyPlayer>().AceOfSpades && crit)
			{
				damage = (int)(damage * 1.1f + 0.5f);
				for (int i = 0; i < 3; i++)
					Dust.NewDust(target.position, target.width, target.height, ModContent.DustType<SpadeDust>(), 0, -0.8f);
			}
		}

		public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit) => OnHitNPCWithAnything(proj, target, damage, knockback, crit);
		public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit) => OnHitNPCWithAnything(item, target, damage, knockback, crit);

		public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			// Twilight Talisman & Shadow Gauntlet
			bool shadowFlameCondition = (Player.HasAccessory<Twilight1>() && Main.rand.NextBool(15)) || (Player.HasAccessory<ShadowGauntlet>() && proj.melee && Main.rand.NextBool(2));
			AddBuffWithCondition(shadowFlameCondition, target, BuffID.ShadowFlame, 180);
		}

		public void OnHitNPCWithAnything(Entity weapon, NPC target, int damage, float knockback, bool crit)
		{
			// Hell Charm
			if (Player.HasAccessory<HCharm>() && Main.rand.Next(11) == 0)
			{
				if ((weapon is Item i && i.melee) || weapon is Projectile)
					target.AddBuff(BuffID.OnFire, 120);
			}

			// Amazon Charm
			if (Player.HasAccessory<YoyoCharm2>() && Main.rand.Next(11) == 0)
			{
				if ((weapon is Item i && i.melee) || weapon is Projectile)
					target.AddBuff(BuffID.Poisoned, 120);
			}
		}

		private void AddBuffWithCondition(bool condition, NPC p, int id, int ticks) { if (condition) p.AddBuff(id, ticks); }

		public override void PostUpdateEquips()
		{
			// Shadow Gauntlet
			if (Player.HasAccessory<ShadowGauntlet>())
			{
				Player.kbGlove = true;
				Player.GetDamage(DamageClass.Melee) += 0.07F;
				Player.meleeSpeed += 0.07F;
			}

			// Cimmerian Scepter
			if (!Player.dead && Player.HasAccessory<CimmerianScepter>() && Player.ownedProjectileCounts[ModContent.ProjectileType<CimmerianScepterProjectile>()] < 1)
				Projectile.NewProjectile(Player.Center, Vector2.Zero, ModContent.ProjectileType<CimmerianScepterProjectile>(), (int)(22 * Player.GetDamage(DamageClass.Summon)), 1.5f, Player.whoAmI);
		}

		public override void MeleeEffects(Item item, Rectangle hitbox)
		{
			// Shadow Gauntlet
			if (Player.HasAccessory<ShadowGauntlet>() && item.IsMelee())
				Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Shadowflame);
		}

		public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
		{
			// Spectre Ring
			if (Player.HasAccessory<SpectreRing>())
			{
				for (int h = 0; h < 3; h++)
				{
					Vector2 vel = new Vector2(0, -1);
					float rand = Main.rand.NextFloat() * MathHelper.TwoPi;
					vel = vel.RotatedBy(rand);
					vel *= 2f;
					Projectile.NewProjectile(Player.GetSource_OnHurt(null), Player.Center, vel, ProjectileID.LostSoulFriendly, 45, 0, Main.myPlayer);
				}
			}

			// Mana Shield & Seraphim Bulwark
			if (Player.HasAccessory<ManaShield>() || Player.HasAccessory<SeraphimBulwark>())
			{
				damage -= (int)damage / 10;
				if (Player.statMana > (int)damage / 10 * 4)
				{
					if ((Player.statMana - (int)damage / 10 * 4) > 0)
						Player.statMana -= (int)damage / 10 * 4;
					else
						Player.statMana = 0;
				}
			}
		}

		public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
		{
			// Spectre Ring
			if (Player.HasAccessory<SpectreRing>())
			{
				int newProj = Projectile.NewProjectile(Player.GetSource_OnHurt(null), Player.Center, new Vector2(6, 6), ProjectileID.SpectreWrath, 40, 0f, Main.myPlayer);

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
			if (heldItem.shoot > ProjectileID.None && heldItem.ModItem != null && heldItem.ModItem.Mod == ModContent.GetInstance<SpiritMod>())
			{
				var p = new Projectile();
				p.SetDefaults(player.HeldItem.shoot);

				if (p.ModProjectile != null && p.ModProjectile is ClubProj)
					return true;
			}
			return false;
		}
	}
}
