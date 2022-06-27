using Terraria;
using System;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;

namespace SpiritMod.Items.Accessory.Ukelele
{
	public class Ukelele : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ukulele");
			Tooltip.SetDefault("Hitting enemies has a chance to create a chain of lightning\n'...and his music was electric.'");
		}

		public override void SetDefaults()
		{
			Item.width = 50;
			Item.height = 48;
			Item.value = Item.buyPrice(0, 3, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
		}

		public override void Update(ref float gravity, ref float maxFallSpeed) => Lighting.AddLight(Item.Center, 0.075f, 0.231f, 0.255f);
		public override Color? GetAlpha(Color lightColor) => Color.White;
		public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<UkelelePlayer>().active = true;
	}

	public class UkelelePlayer : ModPlayer
	{
		public bool active = false;
		int overcharge = 0;

		public override void ResetEffects()
		{
			active = false;
			if (overcharge > 0)
				overcharge--;
		}

		public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
		{
			if (active && proj.type != ModContent.ProjectileType<UkeleleProj>() && Main.rand.Next(4) == 0 && overcharge < 30)
			{
				SoundEngine.PlaySound(SpiritMod.Instance.GetLegacySoundSlot(SoundType.Custom, "Sounds/Ukulele").WithPitchVariance(0.8f).WithVolume(0.7f), Player.Center);
				SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 12).WithVolume(0.6f), target.Center);
				SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap, target.position);
				DoLightningChain(target, damage);
			}
		}

		public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
		{
			if (active && Main.rand.Next(4) == 0 && overcharge < 30)
			{
				SoundEngine.PlaySound(SpiritMod.Instance.GetLegacySoundSlot(SoundType.Custom, "Sounds/Ukulele").WithPitchVariance(0.8f).WithVolume(0.7f), Player.Center);
				SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 12).WithVolume(0.6f), target.Center);
				SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap, target.position);
	
				DoLightningChain(target, damage);
			}
		}

		private void DoLightningChain(NPC target, int damage)
		{
			overcharge += 15;

			ParticleHandler.SpawnParticle(new PulseCircle(Player.Center, Color.Cyan * 0.124f, (.75f) * 100, 20, PulseCircle.MovementType.Outwards)
			{
				Angle = 0f,
				ZRotation = 0,
				Velocity = Vector2.Zero
			});

			Projectile.NewProjectile(target.Center + new Vector2(0, 16), Vector2.Zero, ModContent.ProjectileType<UkeleleProjTwo>(), 0, 0, Player.whoAmI);
			int proj = Projectile.NewProjectile(target.Center, Vector2.Zero, ModContent.ProjectileType<UkeleleProj>(), damage / 2, 0, Player.whoAmI);
			if (Main.projectile[proj].ModProjectile is UkeleleProj lightning)
			{
				lightning.currentEnemy = target;
				lightning.hit[5] = target;
			}
		}
	}

	public class UkeleleProj : ModProjectile
	{
		public NPC[] hit = new NPC[8];
		public NPC currentEnemy;
		int animCounter = 5;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ukulele");
			Main.projFrames[Projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 5;
			Projectile.timeLeft = 300;
			Projectile.aiStyle = -1;
			Projectile.height = 10;
			Projectile.width = 10;
			Projectile.alpha = 255;
		}

		private int Mode
		{
			get => (int)Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		private NPC Target
		{
			get => Main.npc[(int)Projectile.ai[1]];
			set => Projectile.ai[1] = value.whoAmI;
		}

		private void SetOrigin(Vector2 value)
		{
			Projectile.localAI[0] = value.X;
			Projectile.localAI[1] = value.Y;
		}

		public override void AI()
		{
			if (Mode == 0)
			{
				SetOrigin(Projectile.position);
				Mode = 1;
			}
			else
			{
				if (Mode == 2)
				{
					Projectile.extraUpdates = 0;
					Projectile.numUpdates = 0;
				}
				if (Projectile.timeLeft < 300)
				{
					animCounter--;
					if (animCounter > 0)
						Projectile.Center = currentEnemy.Center;
					if (animCounter == 1)
					{
						NPC target = TargetNext(currentEnemy);
						if (target != null && !target.friendly && !target.townNPC)
						{
							Projectile.Center = target.Center;
							for (int k = 0; k < 3; k++)
							{
								DustHelper.DrawElectricity(currentEnemy.Center, target.Center, 226, 0.5f);
							}
							Target = target;
						}
						else
							Projectile.Kill();
					}
				}
				SetOrigin(Projectile.position);
			}
		}

	

		private bool CanTarget(NPC target)
		{
			foreach (var npc in hit)
				if (target == npc)
					return false;
			return true;
		}

		public NPC TargetNext(NPC current)
		{
			float range = 50 * 14;
			range *= range;
			NPC target = null;
			var center = Projectile.Center;
			for (int i = 0; i < 200; ++i)
			{
				NPC npc = Main.npc[i];
				//if npc is a valid target (active, not friendly, and not a critter)
				if (npc != current && npc.CanBeChasedBy() && CanTarget(npc))
				{
					float dist = Vector2.DistanceSquared(center, npc.Center);
					if (dist < range)
					{
						range = dist;
						target = npc;
					}
				}
			}
			return target;
		}

		public override bool? CanHitNPC(NPC target) => CanTarget(target) && target == Target;

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.velocity = Vector2.Zero;
			hit[Projectile.penetrate - 1] = target;
			currentEnemy = target;
			animCounter = 5;
			ParticleHandler.SpawnParticle(new PulseCircle(target.Center, Color.Cyan * 0.4f, (.35f) * 100, 20, PulseCircle.MovementType.Outwards)
			{
				Angle = 0f,
				ZRotation = 0,
				RingColor = Color.Cyan,
				Velocity = Vector2.Zero
			});
			Projectile.NewProjectile(target.Center + new Vector2(0, 16), Vector2.Zero, ModContent.ProjectileType<UkeleleProjTwo>(), 0, 0, Projectile.owner);
			SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap, target.position);

			Projectile.netUpdate = true;
		}
	}

	public class UkeleleProjTwo : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ukelele");
			Main.projFrames[Projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 16;
			Projectile.aiStyle = -1;
			Projectile.height = 68;
			Projectile.width = 74;
		}

		public override void AI()
		{
			Projectile.frame = ((16 - Projectile.timeLeft) / 4) - 1;
			Color color = Color.Cyan;
			Lighting.AddLight((int)((Projectile.position.X + (Projectile.width / 2)) / 16f), (int)((Projectile.position.Y + (Projectile.height / 2)) / 16f), color.R / 450f, color.G / 450f, color.B / 450f);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			int frameHeight = tex.Height / 4;
			sb.Draw(tex, Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Rectangle(0, Projectile.frame * frameHeight, tex.Width, frameHeight), Color.White, Projectile.rotation, new Vector2(tex.Width - (Projectile.width / 2), frameHeight), Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}