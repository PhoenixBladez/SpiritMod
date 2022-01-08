using Terraria;
using System;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;

namespace SpiritMod.Items.Accessory.ElectricGuitar
{
	public class ElectricGuitar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Electric Guitar");
			Tooltip.SetDefault("Nearby enemies and enemies hit by attacks may be hit by chain lightning");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 6));
		}

		public override void SetDefaults()
		{
			item.width = 70;
			item.height = 48;
			item.value = Item.buyPrice(0, 3, 0, 0);
			item.rare = ItemRarityID.Pink;
			item.accessory = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Items.Accessory.Ukelele.Ukelele>());
			recipe.AddIngredient(ModContent.ItemType<Items.Accessory.UnstableTeslaCoil.Unstable_Tesla_Coil>());
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();

		}

		public override void Update(ref float gravity, ref float maxFallSpeed) => Lighting.AddLight(item.Center, 0.075f, 0.231f, 0.255f);
		public override Color? GetAlpha(Color lightColor) => Color.White;
		public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<ElectricGuitarPlayer>().active = true;
	}

	public class ElectricGuitarPlayer : ModPlayer
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
			if (active && proj.type != ModContent.ProjectileType<ElectricGuitarProj>() && proj.type != ModContent.ProjectileType<ElectricGuitarProjPlayer>() && Main.rand.Next(4) == 0 && overcharge < 30)
			{
				Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 47).WithPitchVariance(0.8f).WithVolume(0.7f), player.Center);
				Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 12).WithVolume(0.6f), target.Center);
				Main.PlaySound(SoundID.DD2_LightningAuraZap, target.position);
				DoLightningChain(target, damage);
			}
		}

		public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
		{
			if (active && Main.rand.Next(4) == 0 && overcharge < 30)
			{
				Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 47).WithPitchVariance(0.8f).WithVolume(0.7f), player.Center);
				Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 12).WithVolume(0.6f), target.Center);
				Main.PlaySound(SoundID.DD2_LightningAuraZap, target.position);

				DoLightningChain(target, damage);
			}
		}
		int attackTimer;

		public override void PreUpdate()
		{
			if (active)
			{
				attackTimer++;
				if (attackTimer >= 90)
				{
					for (int i = 0; i < Main.npc.Length; i++)
					{
						NPC npc = Main.npc[i];
						if (Vector2.Distance(player.Center, npc.Center) <= 500f && !npc.friendly && npc.damage > 0 && npc.life > 0 && npc.life < npc.lifeMax && npc.active)
						{
							int proj = Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<ElectricGuitarProjPlayer>(), 35, 2, player.whoAmI);
							Main.projectile[proj].ai[0] = npc.position.X;
							Main.projectile[proj].ai[1] = npc.position.Y;
							ParticleHandler.SpawnParticle(new PulseCircle(player.Center, new Color(255, 36, 50) * 0.124f, (.45f) * 100, 20, PulseCircle.MovementType.Outwards)
							{
								Angle = 0f,
								ZRotation = 0,
								RingColor = new Color(255, 36, 50) * 0.84f,
								Velocity = Vector2.Zero
							});
							Main.projectile[proj].netUpdate = true;

							Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 47).WithPitchVariance(0.8f).WithVolume(0.7f), player.Center);
							Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 12).WithVolume(0.6f), player.Center);
							Main.PlaySound(SoundID.DD2_LightningAuraZap, player.position);
						}
					}
					attackTimer = 0;
				}
			}
		}
		private void DoLightningChain(NPC target, int damage)
		{
			overcharge += 15;

			Vector2 dirToMouse = (player.Center - target.Center);

			ParticleHandler.SpawnParticle(new PulseCircle(Vector2.Zero, new Color(255, 36, 50) * 0.4f, (.7f) * 100, 20, PulseCircle.MovementType.OutwardsQuadratic)
			{
				RingColor = new Color(255, 36, 50) * 0.4f,
				Velocity = Vector2.Zero
			});

			int proj = Projectile.NewProjectile(target.Center, Vector2.Zero, ModContent.ProjectileType<ElectricGuitarProj>(), damage / 2, 2, player.whoAmI);
			if (Main.projectile[proj].modProjectile is ElectricGuitarProj lightning)
			{
				lightning.currentEnemy = target;
				lightning.hit[5] = target;
			}
		}
	}

	public class ElectricGuitarProj : ModProjectile
	{
		public NPC[] hit = new NPC[8];
		public NPC currentEnemy;
		int animCounter = 5;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Electric Guitar");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.penetrate = 5;
			projectile.timeLeft = 300;
			projectile.aiStyle = -1;
			projectile.height = 10;
			projectile.width = 10;
			projectile.alpha = 255;
		}

		private int Mode
		{
			get => (int)projectile.ai[0];
			set => projectile.ai[0] = value;
		}

		private NPC Target
		{
			get => Main.npc[(int)projectile.ai[1]];
			set => projectile.ai[1] = value.whoAmI;
		}

		private void SetOrigin(Vector2 value)
		{
			projectile.localAI[0] = value.X;
			projectile.localAI[1] = value.Y;
		}

		public override void AI()
		{
			if (Mode == 0)
			{
				SetOrigin(projectile.position);
				Mode = 1;
			}
			else
			{
				if (Mode == 2)
				{
					projectile.extraUpdates = 0;
					projectile.numUpdates = 0;
				}
				if (projectile.timeLeft < 300)
				{
					animCounter--;
					if (animCounter > 0)
						projectile.Center = currentEnemy.Center;
					if (animCounter == 1)
					{
						NPC target = TargetNext(currentEnemy);
						if (target != null && !target.friendly && !target.townNPC)
						{
							Vector2 dirToMouse = (currentEnemy.Center - target.Center);

							projectile.Center = target.Center;
							for (int k = 0; k < 3; k++)
							{
								DustHelper.DrawElectricity(currentEnemy.Center, target.Center, DustID.FireworkFountain_Red, 0.5f);
							}

							Target = target;
						}
						else
							projectile.Kill();
					}
				}
				SetOrigin(projectile.position);
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
			var center = projectile.Center;
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
			projectile.velocity = Vector2.Zero;
			hit[projectile.penetrate - 1] = target;
			currentEnemy = target;
			animCounter = 5;
			Main.PlaySound(SoundID.DD2_LightningAuraZap, target.position);
			ParticleHandler.SpawnParticle(new PulseCircle(target.Center, new Color(255, 36, 50) * 0.124f, (.75f) * 100, 20, PulseCircle.MovementType.Outwards)
			{
				Angle = 0f,
				ZRotation = 0,
				RingColor = new Color(255, 36, 50) * 0.84f,
				Velocity = Vector2.Zero
			});
			projectile.netUpdate = true;
		}
	}
	public class ElectricGuitarProjPlayer : ModProjectile
	{
		public float x = 0f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lightning Zap");
		}
		public override void SetDefaults()
		{
			projectile.width = 4;
			projectile.height = 4;
			projectile.hide = true;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.MaxUpdates = 15;
			projectile.timeLeft = 66;
			projectile.tileCollide = false;
		}
		public override void AI()
		{
			projectile.localAI[0] += 1f;

			if (projectile.localAI[0] > -1f)
			{
				x = projectile.Center.Y + 50;
			}
			if (projectile.localAI[0] > -1f)
			{
				for (int i = 0; i < 10; i++)
				{
					float PosX = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
					float PosY = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;

					int dustIndex = Dust.NewDust(new Vector2(PosX, PosY), 0, 0, DustID.FireworkFountain_Red, 0f, 0f, 180, default, 0.5f);

					Main.dust[dustIndex].position.X = PosX;
					Main.dust[dustIndex].position.Y = PosY;

					Main.dust[dustIndex].velocity *= 0f;
					Main.dust[dustIndex].noGravity = true;
				}
			}

			Vector2 vector2_1 = new Vector2(projectile.ai[0], projectile.ai[1]);
			float speed = 16f;
			float dX = vector2_1.X - projectile.Center.X;
			float dY = vector2_1.Y - projectile.Center.Y;

			float dist = (float)Math.Sqrt((double)(dX * dX + dY * dY));

			speed /= dist;

			Vector2 randomSpeed = new Vector2(dX, dY).RotatedByRandom(MathHelper.ToRadians(90));

			if (projectile.localAI[0] > 1f)
			{
				projectile.velocity = new Vector2(randomSpeed.X * speed, randomSpeed.Y * speed);
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.NewProjectile(target.Center, Vector2.Zero, ModContent.ProjectileType<ElectricGuitarProj>(), projectile.damage / 2, 0, projectile.owner);
			Main.PlaySound(SoundID.DD2_LightningAuraZap, target.position);
			ParticleHandler.SpawnParticle(new PulseCircle(target.Center, new Color(255, 36, 50) * 0.124f, (.45f) * 100, 20, PulseCircle.MovementType.Outwards)
			{
				Angle = 0f,
				ZRotation = 0,
				RingColor = new Color(255, 36, 50) * 0.84f,
				Velocity = Vector2.Zero
			});
			projectile.Kill();
		}
	}
}