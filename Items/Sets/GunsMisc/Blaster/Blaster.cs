using Microsoft.Xna.Framework;
using SpiritMod.Projectiles;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SpiritMod.Items.Sets.GunsMisc.Blaster
{
	public class Blaster : ModItem
	{
		public static string[] RandNames = { "Luminous", "Ecliptic", "Aphelaic", "Cosmic", "Perihelaic", "Ionized", "Axial" };
		protected byte nameIndex;

		public string WeaponName => RandNames[nameIndex % RandNames.Length] + " Blaster";
		public int elementSecondary;
		public int elementPrimary;

		public override bool CloneNewInstances => true;
		public int fireType = 1;
		int dustType;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Blaster");

		public override void SetDefaults()
		{
			Item.DamageType = DamageClass.Ranged;
			Item.width = 60;
			Item.height = 32;
			Item.damage = 13;
			Item.useTime = 27;
			Item.useAnimation = 27;
			Item.knockBack = 1f;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.scale = .85f;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.useTurn = false;
			Item.rare = ItemRarityID.Green;
			Item.autoReuse = true;
			Item.shoot = ProjectileID.PurificationPowder;
			Item.shootSpeed = 9f;
			Item.useAmmo = AmmoID.Bullet;

			Generate();
		}

		public override ModItem Clone(Item itemClone)
		{
			var myClone = (Blaster)base.Clone(itemClone);

			myClone.nameIndex = nameIndex;
			myClone.elementSecondary = elementSecondary;
			myClone.elementPrimary = elementPrimary;

			myClone.ApplyStats();

			return myClone;
		}

		public override bool CanRightClick() => true;
		public override bool ConsumeItem(Player player) => false;

		public override void HoldItem(Player player)
		{
			if (elementPrimary <= 1 && fireType == 1)
			{
				SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/GunsMisc/Blaster/Blaster_FireGlow");
				dustType = DustID.Fire;
			}
			if (elementPrimary >= 2 && fireType == 1)
			{
				SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/GunsMisc/Blaster/Blaster_CorrosiveGlow");
				dustType = 163;
			}
			if (elementSecondary <= 4 && fireType == 2)
			{
				SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/GunsMisc/Blaster/Blaster_ShockGlow");
				dustType = DustID.Electric;
			}
			if (elementSecondary >= 5 && fireType == 2)
			{
				SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/GunsMisc/Blaster/Blaster_FreezeGlow");
				dustType = DustID.DungeonSpirit;
			}
		}

		public override void RightClick(Player player)
		{
			if (elementPrimary <= 1)
				elementalType = "Fire";
			if (elementPrimary >= 2)
				elementalType = "Poison";
			if (elementSecondary <= 4)
				elementalType2 = "Shock";
			if (elementSecondary >= 5)
				elementalType2 = "Freeze";

			if (fireType == 1)
			{
				CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y - 10, player.width, player.height), new Color(255, 255, 255, 100), elementalType2);
				fireType++;
			}
			else if (fireType == 2)
			{
				CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y - 10, player.width, player.height), new Color(255, 255, 255, 100), elementalType);
				fireType--;
			}
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			SoundEngine.PlaySound(SoundLoader.customSoundType, player.position, Mod.GetSoundSlot(SoundType.Custom, "Sounds/MaliwanShot1"));

			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY - 1)) * 38f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
				position += muzzleOffset;

			float spread = MathHelper.ToRadians(4f);
			float baseSpeed = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
			double baseAngle = Math.Atan2(speedX, speedY);
			double randomAngle = baseAngle + (Main.rand.NextFloat() - 0.5f) * spread;
			speedX = baseSpeed * (float)Math.Sin(randomAngle);
			speedY = baseSpeed * (float)Math.Cos(randomAngle);

			if (elementPrimary <= 1 && fireType == 1)
			{
				int proj = Projectile.NewProjectile(position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
				Main.projectile[proj].GetGlobalProjectile<SpiritGlobalProjectile>().shotFromMaliwanFireCommon = true;
			}
			if (elementPrimary >= 2 && fireType == 1)
			{
				int proj = Projectile.NewProjectile(position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
				Main.projectile[proj].GetGlobalProjectile<SpiritGlobalProjectile>().shotFromMaliwanAcidCommon = true;
			}
			if (elementSecondary <= 4 && fireType == 2)
			{
				int proj = Projectile.NewProjectile(position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
				Main.projectile[proj].GetGlobalProjectile<SpiritGlobalProjectile>().shotFromMaliwanShockCommon = true;
			}
			if (elementSecondary >= 5 && fireType == 2)
			{
				int proj = Projectile.NewProjectile(position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
				Main.projectile[proj].GetGlobalProjectile<SpiritGlobalProjectile>().shotFromMaliwanFreezeCommon = true;
			}
			for (int index1 = 0; index1 < 5; ++index1)
			{
				int index2 = Dust.NewDust(new Vector2(position.X, position.Y), Item.width - 64, Item.height - 16, dustType, speedX, speedY, (int)byte.MaxValue, new Color(), (float)SpiritMod.Instance.spiritRNG.Next(10, 17) * 0.1f);
				Main.dust[index2].noLight = true;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity *= 0.5f;
				Main.dust[index2].scale *= .6f;
			}
			return false;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override void SaveData(TagCompound tag)/* tModPorter Suggestion: Edit tag parameter instead of returning new TagCompound */ => new TagCompound
		{
			{ nameof(nameIndex), nameIndex },
			{ nameof(elementPrimary), elementPrimary },
			{ nameof(elementSecondary), elementSecondary }
		};

		public override void LoadData(TagCompound tag)
		{
			if (!tag.ContainsKey(nameof(nameIndex)))
				return;

			nameIndex = tag.Get<byte>(nameof(nameIndex));
			elementPrimary = tag.Get<int>(nameof(elementPrimary));
			elementSecondary = tag.Get<int>(nameof(elementSecondary));

			ApplyStats();
		}

		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(nameIndex);
			writer.Write(elementPrimary);
			writer.Write(elementSecondary);
		}

		public override void NetReceive(BinaryReader reader)
		{
			nameIndex = reader.ReadByte();
			elementPrimary = reader.ReadInt32();
			elementSecondary = reader.ReadInt32();

			ApplyStats();
		}

		public void Generate()
		{
			nameIndex = (byte)SpiritMod.Instance.spiritRNG.Next(RandNames.Length);
			elementPrimary = SpiritMod.Instance.spiritRNG.Next(0, 3);
			elementSecondary = SpiritMod.Instance.spiritRNG.Next(3, 6);

			ApplyStats();
		}

		public void ApplyStats() => Item.SetNameOverride(WeaponName);

		public string elementalType;
		public string elementalType2;

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (elementPrimary <= 1)
				elementalType = "Fire";
			if (elementPrimary >= 2)
				elementalType = "Poison";
			if (elementSecondary <= 4)
				elementalType2 = "Shock";
			if (elementSecondary >= 5)
				elementalType2 = "Freeze";

			var line = new TooltipLine(Mod, "", "Right-click in inventory to toggle between " + elementalType + " & " + elementalType2);
			tooltips.Add(line);
		}
	}
}