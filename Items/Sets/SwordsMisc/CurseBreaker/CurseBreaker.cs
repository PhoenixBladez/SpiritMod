using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.Utilities;
using Terraria;
using Terraria.Enums;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Particles;
using SpiritMod.Prim;
using System.Collections.Generic;
using SpiritMod.Items.Sets.BloodcourtSet;
using Terraria.DataStructures;
using ReLogic.Content;

namespace SpiritMod.Items.Sets.SwordsMisc.CurseBreaker
{
	public class CurseBreaker : ModItem
	{
		public bool ChargeReady => charge % 3 == 2;

		private int charge;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursebreaker");
			Tooltip.SetDefault("Every third swing curses nearby enemies\nStrike again to break the curse, dealing extra damage");
		}

		public override void SetDefaults()
		{
			Item.damage = 100;
			Item.DamageType = DamageClass.Melee;
			Item.width = 36;
			Item.height = 44;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.reuseDelay = 20;
			Item.channel = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 10f;
			Item.value = Item.sellPrice(0, 2, 70, 0);
			Item.crit = 4;
			Item.rare = ItemRarityID.Pink;
			Item.shootSpeed = 1f;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<CurseBreakerProj>();
			Item.noUseGraphic = true;
			Item.noMelee = true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Vector2 direction = velocity;
			SoundEngine.PlaySound(SoundID.Item1 with { PitchVariance = 0.5f }, player.Center);
			Projectile proj = Projectile.NewProjectileDirect(position + (direction * 20) + (direction.RotatedBy(-1.57f * player.direction) * 20), Vector2.Zero, type, damage, knockback, player.whoAmI);
			var mp = proj.ModProjectile as CurseBreakerProj;
			mp.Phase = charge % 3;

			if (charge % 3 != 0)
				SoundEngine.PlaySound(SpiritMod.Instance.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/SwordSlash1").WithPitchVariance(0.6f).WithVolume(0.8f), player.Center);

			charge++;

			if (charge % 3 != 0)
				SoundEngine.PlaySound(SpiritMod.Instance.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/SwordSlash1").WithPitchVariance(0.6f).WithVolume(0.8f), player.Center);
			else
				SoundEngine.PlaySound(SpiritMod.Instance.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/PowerSlash1").WithPitchVariance(0.2f).WithVolume(0.6f), player.Center);
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.SoulofNight, 12);
			recipe.AddIngredient(ModContent.ItemType<DreamstrideEssence>(), 8);
			recipe.AddRecipeGroup("SpiritMod:Tier3HMBar", 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}

	internal class CurseBreakerProj : ModProjectile, IDrawAdditive
	{
		public float SwingRadians //Total radians of the sword's arc
		{
			get
			{
				if (Empowered)
					return MathHelper.Pi * 1.75f; 
				else
					return MathHelper.Pi * 1.35f; 
			}
		}

		private int SwingTime
		{
			get
			{
				if (!Empowered)
					return 40;
				else
					return 54;
			}
		}

		public int Phase;

		public Player Player => Main.player[Projectile.owner];

		public bool Empowered => Phase == 2;

		private bool initialized = false;

		Vector2 direction = Vector2.Zero;

		private bool flip = false;

		public int Timer;

		private float rotation;

		private bool cursed = false;

		private int cursedTimer = -1;

		public int SwingDirection
		{
			get
			{
				switch (Phase)
				{
					case 0:
						return -1 * Math.Sign(direction.X);
					case 1:
						return 1 * Math.Sign(direction.X);
					default:
						return -1 * Math.Sign(direction.X);

				}
			}
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursebreaker");
		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
			Projectile.Size = new Vector2(100, 40);
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 16;
			Projectile.ownerHitCheck = true;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Vector2 lineDirection = rotation.ToRotationVector2();
			float collisionPoint = 0;
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Player.Center, Player.Center + (lineDirection * Projectile.width), Projectile.height, ref collisionPoint))
				return true;
			return false;
		}

		public override bool? CanCutTiles() => true;

		// Plot a line from the start of the Solar Eruption to the end of it, to change the tile-cutting collision logic. (Don't change this.)
		public override void CutTiles()
		{
			Vector2 lineDirection = rotation.ToRotationVector2();
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Utils.PlotTileLine(Player.Center, Player.Center + (lineDirection * Projectile.width), Projectile.height, DelegateMethods.CutTiles);
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (GetProgress() > 0.2f && GetProgress() < 0.8f)
				return base.CanHitNPC(target);
			return false;
		}

		public override void AI()
		{
			Projectile.velocity = Vector2.Zero;
			Player.itemTime = Player.itemAnimation = 5;
			Player.heldProj = Projectile.whoAmI;

			if (Projectile.owner != Main.myPlayer)
				return;

			if (!initialized)
			{
				initialized = true;
				direction = Player.DirectionTo(Main.MouseWorld);
				direction.Normalize();
				Projectile.rotation = direction.ToRotation();

				if (Phase == 1)
					flip = !flip;

				if (direction.X < 0)
					flip = !flip;

				if (Empowered)
					Projectile.Size = new Vector2(130, 40);
			}

			Projectile.Center = Player.Center + (direction.RotatedBy(-1.57f) * 20);

			cursedTimer--;
			if (cursedTimer == 0  && !Empowered)
			{
				Player.GetModPlayer<MyPlayer>().Shake = 8;

				foreach (Projectile proj in Main.projectile)
				{
					if (proj.active && proj.type == ModContent.ProjectileType<CurseBreakerCurse>() && proj.owner == Player.whoAmI)
					{
						NPC npc = Main.npc[(int)proj.ai[0]];
						int buffIndex = npc.FindBuffIndex(ModContent.BuffType<CurseBreakerMark>());
						if (buffIndex != -1)
							npc.DelBuff(buffIndex);
						proj.Kill();
					}
				}

			}
			if (cursedTimer <= 0)
				Timer++;

			if (Timer > SwingTime - 7)
				Projectile.Kill();
			float progress = GetProgress();


			Projectile.scale = 1.5f - (Math.Abs(0.5f - progress));

			if (Empowered)
				Projectile.scale = (((Projectile.scale - 1) * 1.66f) + 1);

			rotation = Projectile.rotation + MathHelper.Lerp(SwingRadians / 2 * SwingDirection, -SwingRadians / 2 * SwingDirection, progress);

			Player.direction = Math.Sign(direction.X);

			Player.itemRotation = rotation;
			if (Player.direction != 1)
				Player.itemRotation -= 3.14f;
			Player.itemRotation = MathHelper.WrapAngle(Player.itemRotation);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

			float progress = GetProgress();

			List<PrimitiveSlashArc> slashArcs = new List<PrimitiveSlashArc>();
			Effect effect = Mod.GetEffect("Effects/NemesisBoonShader");
			effect.Parameters["white"].SetValue(Color.OrangeRed.ToVector4());
			effect.Parameters["opacity"].SetValue((float)Math.Sqrt(1 - progress));
			PrimitiveSlashArc slash = new PrimitiveSlashArc
			{
				BasePosition = Player.Center - Main.screenPosition,
				StartDistance = (Projectile.width * 0.3f) * Projectile.scale,
				EndDistance = (Projectile.width * 0.85f) * (((Projectile.scale - 1) * 0.5f) + 1),
				AngleRange = new Vector2(SwingRadians / 2 * SwingDirection, -SwingRadians / 2 * SwingDirection),
				DirectionUnit = direction,
				Color = Color.Red,
				SlashProgress = progress
			};
			slashArcs.Add(slash);
			PrimitiveRenderer.DrawPrimitiveShapeBatched(slashArcs.ToArray(), effect);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

			Texture2D tex2 = TextureAssets.Projectile[Projectile.type].Value;
			Texture2D tex3 = ModContent.Request<Texture2D>(Texture + "_Glow", AssetRequestMode.ImmediateLoad).Value;
			if (flip)
			{
				if (Empowered)
					Main.spriteBatch.Draw(tex3, Player.Center - Main.screenPosition, null, Color.Red * (float)Math.Sqrt(1 - progress) * 0.3f, rotation + 2.355f, new Vector2(tex3.Width, tex3.Height), Projectile.scale, SpriteEffects.FlipHorizontally, 0f);
				Main.spriteBatch.Draw(tex2, Player.Center - Main.screenPosition, null, lightColor * .5f, rotation + 2.355f, new Vector2(tex2.Width, tex2.Height), Projectile.scale, SpriteEffects.FlipHorizontally, 0f);
			}
			else
			{
				if (Empowered)
					Main.spriteBatch.Draw(tex3, Player.Center - Main.screenPosition, null, Color.Red * (float)Math.Sqrt(1 - progress) * 0.3f, rotation + 0.785f, new Vector2(0, tex3.Height), Projectile.scale, SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(tex2, Player.Center - Main.screenPosition, null, lightColor, rotation + 0.785f, new Vector2(0, tex2.Height), Projectile.scale, SpriteEffects.None, 0f);
			}

			return false;
		}
		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			float progress = GetProgress();
			if (Empowered)
			{
				Texture2D tex3 = ModContent.Request<Texture2D>(Texture + "_Flare", AssetRequestMode.ImmediateLoad);
				for (float i = 0; i < 6.28f; i += 1.57f)
					spriteBatch.Draw(tex3, Player.Center - Main.screenPosition + (rotation.ToRotationVector2() * 75 * Projectile.scale), null, Color.White, i + (Main.GlobalTimeWrappedHourly * 1.5f), new Vector2(tex3.Width, 0) / 2, 0.5f * (float)Math.Pow(1 - progress, 2), SpriteEffects.None, 0f);
			}
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			hitDirection = Math.Sign(direction.X);

			if (Empowered)
			{
				foreach (NPC npc in Main.npc)
				{
					if (npc.active && npc.CanBeChasedBy(this) && npc.Distance(target.Center) < 150 && !npc.HasBuff(ModContent.BuffType<CurseBreakerMark>()))
					{
						npc.AddBuff(ModContent.BuffType<CurseBreakerMark>(), 180);
						Projectile.NewProjectile(Projectile.GetSource_OnHit(npc), npc.Center, Vector2.Zero, ModContent.ProjectileType<CurseBreakerCurse>(), Projectile.damage, 0, Player.whoAmI, npc.whoAmI);
					}
				}
			}
			else if (target.HasBuff(ModContent.BuffType<CurseBreakerMark>()))
			{
				if (!cursed)
				{
					ParticleHandler.SpawnParticle(new PulseCircle(Player.Center, new Color(242, 41, 58) * 0.124f, (.9f) * 100, 20, PulseCircle.MovementType.OutwardsSquareRooted)
					{
						Angle = 0f,
						ZRotation = 0,
						RingColor = new Color(242, 41, 58),
						Velocity = Vector2.Zero
					});
					cursed = true;
					cursedTimer = 8;
				}
			}
		}

		private float GetProgress()
		{
			float progress = Timer / (float)SwingTime;
			progress = EaseFunction.EaseCircularInOut.Ease(progress);
			progress = EaseFunction.EaseQuadOut.Ease(progress);

			if (Empowered)
				progress = EaseFunction.EaseQuadInOut.Ease(progress);
			return progress;
		}
	}
	public class CurseBreakerCurse : ModProjectile
	{
		private NPC target => Main.npc[(int)Projectile.ai[0]];

		private float counter;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Curse");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.width = 32;
			Projectile.height = 28;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.penetrate = 1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 12;
			Projectile.ignoreWater = true;
		}
		private readonly Color Red = new Color(242, 41, 58);
		private readonly Color Black = new Color(38, 10, 12);

		public override Color? GetAlpha(Color lightColor) => Color.White * .6f;
		public override void AI()
		{
			if (Main.rand.NextBool(10))
			{
				Vector2 velocity = -Vector2.UnitY.RotatedByRandom(MathHelper.Pi / 4) * Main.rand.NextFloat(2f, 3f);
				ParticleHandler.SpawnParticle(new CursebreakerRunes(Projectile.Center, velocity, Red, Black, Main.rand.NextFloat(0.75f, 1.25f), 30, delegate (Particle p)
				{
					p.Velocity *= 0.95f;
					p.Scale *= 0.95f;
				}));

			}
			Lighting.AddLight(Projectile.Center, Color.Red.ToVector3() * 0.3f);
			counter += 0.025f;
			if (target.active)
			{
				if (target.HasBuff(ModContent.BuffType<CurseBreakerMark>()))
				{
					Projectile.scale = MathHelper.Clamp(counter * 3, 0, 1);
					Projectile.timeLeft = 12;
				}
				else
					Projectile.scale -= 0.083f;
				Projectile.Center = target.Center;
			}
			else
				Projectile.active = false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			float progress = counter % 1;
			float transparency = (float)Math.Pow(1 - progress, 2);
			float scale = 1 + progress;

			spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White * transparency, Projectile.rotation, tex.Size() / 2, scale * Projectile.scale, SpriteEffects.None, 0f);
			return true;
		}
		public override void PostDraw(Color lightColor)
		{
			DrawBloom(spriteBatch, new Color(242, 41, 58) * 0.33f, 0.48f);
		}
		protected void DrawBloom(SpriteBatch spriteBatch, Color color, float scale)
		{
			Texture2D glow = SpiritMod.Instance.GetTexture("Effects/Masks/Extra_49");
			color.A = 0;

			float glowScale = 1 + ((float)Math.Sin(counter) / 4);

			spriteBatch.Draw(glow, Projectile.Center - Main.screenPosition, null,
				color * glowScale, 0, glow.Size() / 2, Projectile.scale * scale, SpriteEffects.None, 0f);
		}
		public override void Kill(int timeLeft)
		{
			if (timeLeft > 4)
			{
				target.StrikeNPC(Projectile.damage, 0, 0);

				Projectile.NewProjectile(target.Center, Vector2.Zero, ModContent.ProjectileType<CurseBreak>(), 0, 0, Projectile.owner, target.whoAmI);
				SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact, Projectile.Center);
			}
		}
	}
	internal class CurseBreak : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Curse Break");
			Main.projFrames[Projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			Projectile.friendly = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.tileCollide = false;
			Projectile.Size = new Vector2(120, 56);
			Projectile.penetrate = -1;
			Projectile.hide = true;
		}
		public override void AI()
		{
			if (Projectile.frameCounter == 0)
			{
				Projectile.rotation = Main.rand.NextFloat(6.28f);
				for (int i = 0; i < 8; i++)
				{
					Vector2 vel = (Projectile.rotation + 3.14f).ToRotationVector2() * 5;
					vel.Normalize();
					vel = vel.RotatedBy(Main.rand.NextFloat(-0.5f, 0.5f));
					vel *= Main.rand.NextFloat(2, 5);
					ImpactLine line = new ImpactLine(Main.npc[(int)Projectile.ai[0]].Center - (vel * 5), vel, Color.Red, new Vector2(0.25f, Main.rand.NextFloat(0.75f, 1.75f)), 70);
					line.TimeActive = 30;
					ParticleHandler.SpawnParticle(line);

				}
			}
			Projectile.Center = Main.npc[(int)Projectile.ai[0]].Center;
			Projectile.velocity = Vector2.Zero;
			Projectile.frameCounter++;
			if (Projectile.frameCounter % 5 == 0)
				Projectile.frame++;
			if (Projectile.frame >= Main.projFrames[Projectile.type])
				Projectile.active = false;

		}

		public void AdditiveCall(SpriteBatch sB)
		{
			//Adjust framing due to secondary column
			Rectangle frame = Projectile.DrawFrame();

			void Draw(Vector2 offset, float opacity)
			{
				sB.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center + offset - Main.screenPosition, frame,
					Color.White * opacity, Projectile.rotation, new Vector2(frame.Width * 0.8f, frame.Height * 0.5f), Projectile.scale, SpriteEffects.None, 0);
			}


			PulseDraw.DrawPulseEffect((float)Math.Asin(-0.6), 8, 12, delegate (Vector2 posOffset, float opacityMod)
			{
				Draw(posOffset, opacityMod * 0.33f);
			});
			Draw(Vector2.Zero, 1);
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;
	}

	public class CurseBreakerMark : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Curse Mark");
			Main.buffNoTimeDisplay[Type] = false;
		}
	}
}