using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.DonatorItems.FrostTroll
{
	public class BlizzardEdge : ModItem
	{
		private readonly Color Blue = new Color(0, 114, 201);
		private readonly Color White = new Color(255, 255, 255);
		int counter = 5;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blizzard's Edge");
			Tooltip.SetDefault("Right-click after five swings to summon a blizzard");
			SpiritGlowmask.AddGlowMask(item.type, Texture + "_Glow");

		}

		public override void SetDefaults()
		{
			item.damage = 52;
			item.useTime = 24;
			item.useAnimation = 24;
			item.melee = true;
			item.width = 40;
			item.height = 50;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 5;
			item.value = 96700;
			item.rare = ItemRarityID.LightPurple;
			item.shootSpeed = 0f;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useTurn = true;
			item.crit = 6;
			item.shoot = ModContent.ProjectileType<BlizzardProjectile>();
		}
		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			if (Main.rand.Next(4) == 0)
				target.AddBuff(BuffID.Frostburn, 400, true);
		}
		public override bool AltFunctionUse(Player player) => true;
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => GlowmaskUtils.DrawItemGlowMaskWorld(spriteBatch, item, ModContent.GetTexture(Texture + "_Glow"), rotation, scale);

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			counter--;
			if (player.altFunctionUse == 2)
			{
				DrawDust(player);
				if (counter <= 0)
				{
					player.GetModPlayer<MyPlayer>().Shake += 8;
					Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 45));

					if (Main.netMode != NetmodeID.Server)
						Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/BlizzardLoop").WithVolume(0.65f).WithPitchVariance(0.54f), player.Center);

					int p = Projectile.NewProjectile(position.X + (110 * player.direction), position.Y - 8, 0, 0, ModContent.ProjectileType<BlizzardProjectile>(), damage / 3, knockBack / 4, player.whoAmI);
					Main.projectile[p].direction = player.direction;
					counter = 5;
				}
			}
			if (counter == 0)
			{
				if (Main.netMode != NetmodeID.Server)
				{
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/MagicCast1").WithVolume(0.5f).WithPitchVariance(0.54f), player.Center);
					Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 46));
				}

			}
			return false;
		}
		public void DrawDust(Player player)
		{
			float cosRot = (float)Math.Cos(player.itemRotation - 0.78f * player.direction * player.gravDir);
			float sinRot = (float)Math.Sin(player.itemRotation - 0.78f * player.direction * player.gravDir);
			if (!Main.dedServ)
			{
				for (int i = 0; i < 13; i++)
				{
					float length = (item.width * 1.2f - i * item.width / 9) * item.scale + 32;
					ParticleHandler.SpawnParticle(new FireParticle(new Vector2((float)(player.itemLocation.X + length * cosRot * player.direction), (float)(player.itemLocation.Y + length * sinRot * player.direction)), new Vector2(0, Main.rand.NextFloat(-.15f, -.1f)), Blue, White, Main.rand.NextFloat(0.15f, 0.45f), 30));
				}
			}
		}
		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				if (counter > 0)
					return false;

				item.useStyle = ItemUseStyleID.HoldingUp;

			}
			else
			{
				item.useStyle = ItemUseStyleID.SwingThrow;
			}
			return true;
		}
	}

	class BlizzardProjectile : ModProjectile
	{
		private readonly Color Blue = new Color(0, 114, 201);
		private readonly Color White = new Color(255, 255, 255);
		public override void SetStaticDefaults() => DisplayName.SetDefault("Blizzard");

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 25;
			projectile.timeLeft = 120;
			projectile.height = 60;
			projectile.tileCollide = false;
			projectile.width = 350;
			projectile.alpha = 255;
		}

		public override void AI()
		{
			projectile.velocity.X = .5f * projectile.direction;

			Dust dust = Main.dust[Dust.NewDust(new Vector2(projectile.Center.X - (120f * projectile.direction), projectile.Center.Y - 30 + Main.rand.Next(-25, 25)), 0, 0, ModContent.DustType<Dusts.BlizzardDust>(), 0f, 0f, 100, new Color(), Main.rand.NextFloat(1.125f, 1.775f))];
			dust.velocity.Y = 0f;
			dust.velocity.X = Main.rand.NextFloat(13f, 16f) * projectile.direction;

			if (Main.rand.NextBool(3))
			{
				ParticleHandler.SpawnParticle(new SmokeParticle(new Vector2(projectile.Center.X - (120f * projectile.direction), projectile.Center.Y + Main.rand.Next(-25, 25)), new Vector2(Main.rand.NextFloat(-1.5f, 1.5f)), Color.Lerp(Color.LightBlue * .8f, White * .8f, projectile.timeLeft / 120f), Main.rand.NextFloat(0.55f, 0.75f), 30, delegate (Particle p)
			   {
				   p.Velocity *= 0.93f;
				   p.Velocity.Y = 0f;
				   p.Velocity.X = Main.rand.NextFloat(11f, 14f) * projectile.direction;
			   }));
			}
			if (Main.rand.NextBool(5))
			{
				ParticleHandler.SpawnParticle(new SnowflakeParticle(new Vector2(projectile.Center.X - (120f * projectile.direction), projectile.Center.Y + Main.rand.Next(-25, 25)), new Vector2(Main.rand.NextFloat(-1.5f, 1.5f), Main.rand.NextFloat(-4.5f, -2.5f)), Blue, White, Main.rand.NextFloat(.45f, .95f), 45, .5f, Main.rand.Next(3), delegate (Particle p)
				{
					p.Velocity *= 0.93f;
					p.Velocity.Y += 0.15f;
					p.Velocity.X = Main.rand.NextFloat(12f, 15f) * projectile.direction;
				}));
			}
			ParticleHandler.SpawnParticle(new FireParticle(new Vector2(projectile.Center.X - (120f * projectile.direction), projectile.Center.Y + Main.rand.Next(-25, 25)), new Vector2(Main.rand.NextFloat(-1.5f, 1.5f), Main.rand.NextFloat(-4.5f, -2.5f)), Blue, White, Main.rand.NextFloat(0.15f, 0.45f), 30, delegate (Particle p)
			{
				p.Velocity *= 0.93f;
				p.Velocity.Y = 0f;
				p.Velocity.X = Main.rand.NextFloat(13f, 16f) * projectile.direction;
			}));
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool(3))
				target.AddBuff(BuffID.Frostburn, 240);

			for (int i = 0; i < 2; i++)
			{
				ImpactLine line = new ImpactLine(target.Center, new Vector2(projectile.velocity.X * .65f, Main.rand.NextFloat(-.5f, .5f)), Color.Lerp(White, Blue, Main.rand.NextFloat()), new Vector2(0.25f, Main.rand.NextFloat(0.4f, .55f) * 1.5f), 70);
				line.TimeActive = 30;
				ParticleHandler.SpawnParticle(line);

			}

		}
	}
}