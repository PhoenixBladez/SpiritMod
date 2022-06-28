using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SpiritMod.Particles;

namespace SpiritMod.Items.Accessory.ElectricGuitar
{
	public class ElectricGuitar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Electric Guitar");
			Tooltip.SetDefault("Nearby enemies and enemies hit by attacks may be hit by chain lightning");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 6));
		}

		public override void SetDefaults()
		{
			Item.width = 70;
			Item.height = 48;
			Item.value = Item.buyPrice(0, 3, 0, 0);
			Item.rare = ItemRarityID.Pink;
			Item.accessory = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Ukelele.Ukelele>());
			recipe.AddIngredient(ModContent.ItemType<UnstableTeslaCoil.Unstable_Tesla_Coil>());
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();
		}

		public override void Update(ref float gravity, ref float maxFallSpeed) => Lighting.AddLight(Item.Center, 0.075f, 0.231f, 0.255f);
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
				SoundEngine.PlaySound(SoundID.Item47 with { PitchVariance = .8f, Volume = 0.7f }, Player.Center);
				SoundEngine.PlaySound(SoundID.Item12 with { Volume = 0.6f }, Player.Center);
				SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap, target.position);
				DoLightningChain(target, damage);
			}
		}

		public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
		{
			if (active && Main.rand.Next(4) == 0 && overcharge < 30)
			{
				SoundEngine.PlaySound(SoundID.Item47 with { PitchVariance = .8f, Volume = 0.7f }, Player.Center);
				SoundEngine.PlaySound(SoundID.Item12 with { Volume = 0.6f }, Player.Center);
				SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap, target.position);

				DoLightningChain(target, damage);
			}
		}
		int attackTimer;

		public override void PreUpdate()
		{
			if (active)
			{
				attackTimer++;

				int npcsHit = 0;
				if (attackTimer >= 90)
				{
					for (int i = 0; i < Main.npc.Length; i++)
					{
						NPC npc = Main.npc[i];
						if (npc.CanBeChasedBy() && Vector2.Distance(Player.Center, npc.Center) <= 500f)
						{
							int proj = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<ElectricGuitarProjPlayer>(), 35, 2, Player.whoAmI);
							Main.projectile[proj].ai[0] = npc.position.X;
							Main.projectile[proj].ai[1] = npc.position.Y;
							ParticleHandler.SpawnParticle(new PulseCircle(Player.Center, new Color(255, 36, 50) * 0.124f, (.45f) * 100, 20, PulseCircle.MovementType.Outwards)
							{
								Angle = 0f,
								ZRotation = 0,
								RingColor = new Color(255, 36, 50) * 0.84f,
								Velocity = Vector2.Zero
							});
							Main.projectile[proj].netUpdate = true;

							SoundEngine.PlaySound(SoundID.Item47 with { PitchVariance = .8f, Volume = 0.7f }, Player.Center);
							SoundEngine.PlaySound(SoundID.Item12 with { Volume = 0.6f }, Player.Center);
							SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap, Player.position);

							if (npcsHit++ > 3)
								break;
						}
					}
					attackTimer = 0;
				}
			}
		}
		private void DoLightningChain(NPC target, int damage)
		{
			overcharge += 15;

			ParticleHandler.SpawnParticle(new PulseCircle(Vector2.Zero, new Color(255, 36, 50) * 0.4f, (.7f) * 100, 20, PulseCircle.MovementType.OutwardsQuadratic)
			{
				RingColor = new Color(255, 36, 50) * 0.4f,
				Velocity = Vector2.Zero
			});

			int proj = Projectile.NewProjectile(Player.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<ElectricGuitarProj>(), damage / 2, 2, Player.whoAmI);
			if (Main.projectile[proj].ModProjectile is ElectricGuitarProj lightning)
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

		public override void SetStaticDefaults() => DisplayName.SetDefault("Electric Guitar");

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
								DustHelper.DrawElectricity(currentEnemy.Center, target.Center, DustID.FireworkFountain_Red, 0.5f);

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
			SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap, target.position);
			ParticleHandler.SpawnParticle(new PulseCircle(target.Center, new Color(255, 36, 50) * 0.124f, (.75f) * 100, 20, PulseCircle.MovementType.Outwards)
			{
				Angle = 0f,
				ZRotation = 0,
				RingColor = new Color(255, 36, 50) * 0.84f,
				Velocity = Vector2.Zero
			});
			Projectile.netUpdate = true;
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
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.hide = true;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.MaxUpdates = 15;
			Projectile.timeLeft = 66;
			Projectile.tileCollide = false;
		}
		public override void AI()
		{
			Projectile.localAI[0] += 1f;

			if (Projectile.localAI[0] > -1f)
				x = Projectile.Center.Y + 50;

			if (Projectile.localAI[0] > -1f)
			{
				for (int i = 0; i < 10; i++)
				{
					float PosX = Projectile.Center.X - Projectile.velocity.X / 10f * i;
					float PosY = Projectile.Center.Y - Projectile.velocity.Y / 10f * i;

					int dustIndex = Dust.NewDust(new Vector2(PosX, PosY), 0, 0, DustID.FireworkFountain_Red, 0f, 0f, 180, default, 0.5f);

					Main.dust[dustIndex].position.X = PosX;
					Main.dust[dustIndex].position.Y = PosY;

					Main.dust[dustIndex].velocity *= 0f;
					Main.dust[dustIndex].noGravity = true;
				}
			}

			Vector2 destination = new Vector2(Projectile.ai[0], Projectile.ai[1]);
			Vector2 dif = destination - Projectile.Center;
			float speed = 16f / dif.Length();

			Vector2 randomSpeed = new Vector2(dif.X, dif.Y).RotatedByRandom(MathHelper.ToRadians(90));

			if (Projectile.localAI[0] > 1f)
				Projectile.velocity = new Vector2(randomSpeed.X * speed, randomSpeed.Y * speed);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.NewProjectile(target.GetSource_OnHit(target), target.Center, Vector2.Zero, ModContent.ProjectileType<ElectricGuitarProj>(), Projectile.damage / 2, 0, Projectile.owner);
			SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap, target.position);
			ParticleHandler.SpawnParticle(new PulseCircle(target.Center, new Color(255, 36, 50) * 0.124f, (.45f) * 100, 20, PulseCircle.MovementType.Outwards)
			{
				Angle = 0f,
				ZRotation = 0,
				RingColor = new Color(255, 36, 50) * 0.84f,
				Velocity = Vector2.Zero
			});
			Projectile.Kill();
		}
	}
}