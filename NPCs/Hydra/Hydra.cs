using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using SpiritMod.Mechanics.BoonSystem;
using SpiritMod.Utilities;
using SpiritMod.Buffs;
using Terraria.Audio;
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.NPCs.Hydra
{
	public class Hydra : ModNPC
	{
		private const int MAXHEADS = 4;

		private bool initialized = false;

		private List<NPC> heads = new List<NPC>();

		public int headsSpawned = 0;

		public int newHeadCountdown = -1;
		public int headsDue = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lernean Hydra");
		}
		public override void SetDefaults()
		{
			npc.width = 36;
			npc.height = 36;
			npc.damage = 0;
			npc.defense = 10;
			npc.lifeMax = 700;
			npc.HitSound = SoundID.NPCHit7;
			npc.DeathSound = SoundID.NPCDeath5;
			npc.value = 900f;
			npc.knockBackResist = 0;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.immortal = true;
			npc.dontTakeDamage = true;
			npc.hide = true;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			foreach (NPC npc2 in Main.npc)
			{
				if (npc2.active && npc2.type == ModContent.NPCType<Hydra>())
					return 0f;
			}
			int x = spawnInfo.spawnTileX;
			int y = spawnInfo.spawnTileY;
			int tile = (int)Main.tile[x, y].type;
			return (tile == 367) && spawnInfo.spawnTileY > Main.rockLayer && Main.hardMode ? 0.5f : 0f;
		}

		public override void AI()
		{
			npc.TargetClosest(true);
			if (!initialized)
			{
				initialized = true;
				for (int i = 0; i < 2; i++)
				{
					SpawnHead(npc.lifeMax);
				}
			}

			foreach (NPC head in heads.ToArray())
			{
				if (!head.active)
					heads.Remove(head);
			}

			if (heads.Count <= 0)
			{
				npc.life = 0;
				npc.StrikeNPC(1, 0, 0);
			}

			newHeadCountdown--;
			if (newHeadCountdown == 0)
			{
				for (int i = 0; i < headsDue; i++)
				{
					SpawnHead(Math.Max(npc.lifeMax - (50 * headsSpawned), 100));
				}
				headsDue = 1;
			}
		}

		public void SpawnHead(int life)
		{
			if (heads.Count >= MAXHEADS)
				return;

			headsSpawned++;
			int npcIndex = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<HydraHead>(), 0, npc.whoAmI);
			NPC head = Main.npc[npcIndex];
			head.life = head.lifeMax = life;
			heads.Add(head);
		}
	}

	public enum HeadColor
	{
		Red = 0,
		Green = 1,
		Purple = 2
	}
	public class HydraHead : ModNPC
	{
		public const float FIREBALLGRAVITY = 0.3f;

		private bool initialized = false;

		private NPC parent => Main.npc[(int)npc.ai[0]];

		private HeadColor headColor;

		private Vector2 posToBe = Vector2.Zero;
		private float rotation;
		private float sway;

		private float centralRotation;
		private int centralDistance;
		private float rotationSpeed;
		private float swaySpeed;
		private Vector2 orbitRange;

		private int attackCounter;
		private int attackCooldown;
		private bool attacking = false;

		private float headRotationOffset;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lernean Hydra");
		}
		public override void SetDefaults()
		{
			npc.width = 44;
			npc.height = 32;
			npc.damage = 55;
			npc.defense = 12;
			npc.lifeMax = 700;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath5;
			npc.value = 0f;
			npc.knockBackResist = 0;
			npc.noGravity = true;
			npc.noTileCollide = true;
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => false;

		public override void AI()
		{
			npc.velocity *= 0.96f;
			npc.velocity.Y *= 0.96f;
			if (!initialized)
			{
				initialized = true;
				headColor = (HeadColor)Main.rand.Next(3);

				rotation = Main.rand.NextFloat(6.28f);
				sway = Main.rand.NextFloat(6.28f);

				centralDistance = Main.rand.Next(75, 125);
				rotationSpeed = Main.rand.NextFloat(0.03f, 0.05f);
				orbitRange = Main.rand.NextVector2Circular(70, 30);
				swaySpeed = Main.rand.NextFloat(0.015f, 0.035f);
				attackCooldown = Main.rand.Next(150, 200);
			}
			if (!parent.active)
			{
				npc.active = false;
				return;
			}

			RotateToPlayer();

			if (!attacking)
			{
				headRotationOffset = 0f;
				rotation += rotationSpeed;
				sway += swaySpeed;

				headRotationOffset = npc.DirectionTo(Main.player[parent.target].Center).ToRotation();

				attackCounter++;
				if (attackCounter > attackCooldown)
				{
					attacking = true;
					attackCounter = 0;
				}
			}
			else
			{
				AttackBehavior();
			}


			npc.spriteDirection = npc.direction = parent.direction;
			centralRotation = 0.3f * ((float)Math.Sin(sway) + (npc.direction * 1.5f));
			posToBe = DecidePosition();

			MoveToPosition();

			if (headColor == HeadColor.Red)
            {
				npc.buffImmune[BuffID.OnFire] = true;
				npc.buffImmune[BuffID.Confused] = true;
			}
			if (headColor == HeadColor.Green)
            {
				npc.buffImmune[BuffID.Poisoned] = true;
				npc.buffImmune[BuffID.Confused] = true;
			}
			if (headColor == HeadColor.Purple)
            {
				npc.buffImmune[BuffID.Venom] = true;
				npc.buffImmune[BuffID.Confused] = true;
			}

			if (!parent.active)
				npc.active = false;
		}

		private Vector2 DecidePosition()
		{
			Vector2 pos = new Vector2(0, -1).RotatedBy(centralRotation) * centralDistance;
			pos += orbitRange.RotatedBy(rotation);
			pos += parent.Center;
			return pos;
		}

		private void MoveToPosition()
		{
			npc.Center = Vector2.Lerp(npc.Center, posToBe, 0.05f);
		}

		private void AttackBehavior()
		{
			attackCounter++;
			if (headColor == HeadColor.Red)
				headRotationOffset = -1.57f + (npc.direction * 0.7f);
			else
				headRotationOffset = npc.DirectionTo(Main.player[parent.target].Center).ToRotation();
			if (attackCounter == 60)
			{
				LaunchProjectile();
				attacking = false;
				attackCounter = 0;
			}
		}

		private void LaunchProjectile()
		{
			Player target = Main.player[parent.target];
			int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));

			Vector2 direction = npc.DirectionTo(target.Center);
			if (headColor == HeadColor.Red)
				direction = GetArcVel();
			npc.velocity = -Vector2.Normalize(direction) * Main.rand.Next(7,10);
			switch (headColor)
			{
				case HeadColor.Red:
					Main.PlaySound(SoundID.DD2_DrakinBreathIn, npc.Center);
					Main.PlaySound(SoundID.DD2_BetsyFireballShot, npc.Center);
					for (int k = 0; k < 14; k++)
					{
						int d = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Fire, -(npc.position.X - target.position.X) / distance * 2, -(npc.position.Y - target.position.Y) / distance * 4, 0, default, Main.rand.NextFloat(.65f, .85f));
						Main.dust[d].fadeIn = 1f;
						Main.dust[d].velocity *= .95f;
						Main.dust[d].noGravity = true;
					}
					for (int j = 0; j < 12; j++)
					{
						Vector2 vector2 = Vector2.UnitX * -npc.width / 2f;
						vector2 += -Utils.RotatedBy(Vector2.UnitY, (j * 3.141591734f / 6f), default) * new Vector2(10f, 24f);
						vector2 = Utils.RotatedBy(vector2, (npc.rotation - 1.57079637f), default);
						int num8 = Dust.NewDust(npc.Center, 0, 0, DustID.Fire, 0f, 0f, 160, default, 1f);
						Main.dust[num8].scale = 1.1f;
						Main.dust[num8].noGravity = true;
						Main.dust[num8].position = npc.Center + vector2;
						Main.dust[num8].velocity = npc.velocity * 0.1f;
						Main.dust[num8].velocity = Vector2.Normalize(npc.Center - npc.velocity * 3f - Main.dust[num8].position) * 1.25f;
					}
					if (Main.netMode != NetmodeID.MultiplayerClient)
						Projectile.NewProjectile(npc.Center, direction, ModContent.ProjectileType<HydraFireGlob>(), NPCUtils.ToActualDamage(npc.damage), 3);
					break;
				case HeadColor.Green:
					for (int k = 0; k < 20; k++)
					{
						int d = Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<Dusts.AcidDust>(), -(npc.position.X - target.position.X) / distance * 2, -(npc.position.Y - target.position.Y) / distance * 4, 0, default, Main.rand.NextFloat(.65f, .85f));
						Main.dust[d].fadeIn = .6f;
						Main.dust[d].velocity *= .95f;
						Main.dust[d].noGravity = true;
					}
					Main.PlaySound(new Terraria.Audio.LegacySoundStyle(29, 7).WithPitchVariance(0.4f), npc.Center);
					Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 95).WithPitchVariance(0.4f), npc.Center);
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						if (Main.rand.NextBool())
						{
							float rotationOffset = Main.rand.NextFloat(0.25f, 0.5f);
							for (float i = -rotationOffset; i <= rotationOffset; i += rotationOffset)
								Projectile.NewProjectile(npc.Center, direction.RotatedBy(i) * 8, ModContent.ProjectileType<HydraPoisonGlob>(), NPCUtils.ToActualDamage(npc.damage), 3);
						}
						else
						{
							float rotationOffset = Main.rand.NextFloat(0.15f, 0.4f);
							Projectile.NewProjectile(npc.Center, direction.RotatedBy(rotationOffset) * 8, ModContent.ProjectileType<HydraPoisonGlob>(), NPCUtils.ToActualDamage(npc.damage), 3);
							Projectile.NewProjectile(npc.Center, direction.RotatedBy(-rotationOffset) * 8, ModContent.ProjectileType<HydraPoisonGlob>(), NPCUtils.ToActualDamage(npc.damage), 3);
						}
					}
					break;
				case HeadColor.Purple:
					for (int k = 0; k < 14; k++)
					{
						int d = Dust.NewDust(npc.position, npc.width, npc.height, DustID.VenomStaff, -(npc.position.X - target.position.X) / distance * 2, -(npc.position.Y - target.position.Y) / distance * 4, 0, default, Main.rand.NextFloat(.65f, .85f));
						Main.dust[d].fadeIn = 1f;
						Main.dust[d].velocity *= .95f;
						Main.dust[d].noGravity = true;
					}
					Main.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 23).WithPitchVariance(0.2f), npc.Center);
					Main.PlaySound(SoundID.DD2_LightningBugZap, npc.Center);
					if (Main.netMode != NetmodeID.MultiplayerClient)
						Projectile.NewProjectile(npc.Center, direction * 15, ModContent.ProjectileType<HydraVenomGlob>(), NPCUtils.ToActualDamage(npc.damage), 3);
					break;
				default:
					break;
			}
		}

		private Vector2 GetArcVel()
		{

			Vector2 DistanceToTravel = Main.player[parent.target].Center - npc.Center;
			float MaxHeight = MathHelper.Clamp(DistanceToTravel.Y - 35, -100, -10);
			float TravelTime = (float)Math.Sqrt(-2 * MaxHeight / FIREBALLGRAVITY) + (float)Math.Sqrt(2 * Math.Max(DistanceToTravel.Y - MaxHeight, 0) / FIREBALLGRAVITY);
			return new Vector2(MathHelper.Clamp(DistanceToTravel.X / TravelTime, -10, 10), -(float)Math.Sqrt(-2 * FIREBALLGRAVITY * MaxHeight));
		}

		private void RotateToPlayer()
		{
			float rotGoal = headRotationOffset;
			float currentRot = npc.rotation;

			float rotDifference = ((((rotGoal - currentRot) % 6.28f) + 9.42f) % 6.28f) - 3.14f;
			npc.rotation = MathHelper.Lerp(currentRot, currentRot + rotDifference, 0.05f);

		}
		public override void HitEffect(int hitDirection, double damage)
		{
			Main.PlaySound(SoundID.NPCHit1, npc.Center);
			if (npc.life <= 0 && parent.modNPC is Hydra modNPC)
			{
				if (modNPC.newHeadCountdown < 0)
					modNPC.newHeadCountdown = 240 + (30 * modNPC.headsSpawned);
				modNPC.headsDue++;

				SpawnGores();
			}
		}

		private void SpawnGores()
		{
			string headGore = getColor() + "HydraHead";
			string neckGore = getColor() + "HydraNeck";

			Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Hydra/" + headGore), 1f);

			float goreRotation = npc.rotation - (npc.direction == -1 ? 3.14f : 0);

			BezierCurve curve = GetCurve(goreRotation);
			int numPoints = 20; 
			Vector2[] chainPositions = curve.GetPoints(numPoints).ToArray();
			for (int i = 1; i < numPoints; i++)
			{
				Vector2 position = chainPositions[i];
				Gore.NewGore(position, Vector2.Zero, mod.GetGoreSlot("Gores/Hydra/" + neckGore), 1f);
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			float drawRotation = npc.rotation - (npc.direction == -1 ? 3.14f : 0);
			string colorString = getColor();
			string texturePath = Texture + colorString;
			Texture2D headTex = ModContent.GetTexture(texturePath);
			Texture2D neckTex = ModContent.GetTexture(texturePath + "_Neck");

			BezierCurve curve = GetCurve(drawRotation);

			int numPoints = 20; //Should make dynamic based on curve length, but I'm not sure how to smoothly do that while using a bezier curve
			Vector2[] chainPositions = curve.GetPoints(numPoints).ToArray();

			//Draw each chain segment, skipping the very first one, as it draws partially behind the player
			for (int i = 1; i < numPoints; i++)
			{
				Vector2 position = chainPositions[i];

				float rotation = (chainPositions[i] - chainPositions[i - 1]).ToRotation() - MathHelper.PiOver2; //Calculate rotation based on direction from last point
				float yScale = Vector2.Distance(chainPositions[i], chainPositions[i - 1]) / neckTex.Height; //Calculate how much to squash/stretch for smooth chain based on distance between points

				Vector2 scale = new Vector2(1, yScale); // Stretch/Squash chain segment
				Color chainLightColor = Lighting.GetColor((int)position.X / 16, (int)position.Y / 16); //Lighting of the position of the chain segment
				Vector2 origin = new Vector2(neckTex.Width / 2, neckTex.Height); //Draw from center bottom of texture
				spriteBatch.Draw(neckTex, position - Main.screenPosition, null, chainLightColor, rotation, origin, scale, npc.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			}

			spriteBatch.Draw(headTex, npc.Center - Main.screenPosition, null, drawColor, drawRotation, new Vector2(headTex.Width, headTex.Height) / 2, npc.scale, npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			return false;
		}

		private BezierCurve GetCurve(float headRotation)
		{
			Vector2 direction = npc.Center - parent.Bottom;
			Vector2 centralPos = (new Vector2(0, -1) * direction.Length());

			//Control point relative to the parent npc
			Vector2 BaseControlPoint = parent.Bottom + (centralPos.RotatedBy(-centralRotation / 2) * 0.5f);

			//Control point connecting to behind the npc, to make the neck look more like a neck
			float headControlLength = 100;
			Vector2 HeadControlPoint = npc.Center - Vector2.UnitX.RotatedBy(headRotation + ((npc.spriteDirection < 0) ? MathHelper.Pi : 0)) * headControlLength;

			//Control point to smooth out the bezier, taking the midway point beween the other 2 control points and moving it backwards from them perpindicularly 
			float smootheningFactor = 0.4f;
			Vector2 SmootheningControlPoint = Vector2.Lerp(BaseControlPoint, HeadControlPoint, 0.5f) + (HeadControlPoint - BaseControlPoint).RotatedBy(-npc.spriteDirection * MathHelper.PiOver2) * smootheningFactor;

			return new BezierCurve(new Vector2[] { parent.Bottom, BaseControlPoint, SmootheningControlPoint, HeadControlPoint, npc.Center });
		}

		private string getColor()
		{
			switch ((int)headColor)
			{
				case 0:
					return "_Red";
				case 1:
					return "_Green";
				case 2:
					return "_Purple";
				default:
					return "_Red";
			}
		}
	}
	public class HydraFireGlob : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hydra Spit");
			Main.projFrames[projectile.type] = 4;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 12;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			projectile.penetrate = 1;
			projectile.width = 24;
			projectile.height = 24;
			projectile.aiStyle = -1;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.tileCollide = true;
			projectile.damage = 60;
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation();
			if (Main.rand.NextBool(4))
			{
				Vector2 dustVel = -projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.3f, 0.3f)) * Main.rand.NextFloat(0.4f);
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Fire, dustVel.X, dustVel.Y);
			}
			projectile.frameCounter++;
			if (projectile.frameCounter % 4 == 0)
			{
				projectile.frame++;
				projectile.frame %= Main.projFrames[projectile.type];
			}
			projectile.localAI[0] += 1f;
			if (projectile.localAI[0] == 12f)
			{
				projectile.localAI[0] = 0f;
				for (int j = 0; j < 12; j++)
				{
					Vector2 vector2 = Vector2.UnitX * -projectile.width / 2f;
					vector2 += -Utils.RotatedBy(Vector2.UnitY, (j * 3.141591734f / 6f), default) * new Vector2(8f, 24f);
					vector2 = Utils.RotatedBy(vector2, (projectile.rotation), default);
					int num8 = Dust.NewDust(projectile.Center, 0, 0, DustID.Fire, 0f, 0f, 160, default, 1f);
					Main.dust[num8].scale = 1.1f;
					Main.dust[num8].noGravity = true;
					Main.dust[num8].position = projectile.Center + vector2;
					Main.dust[num8].velocity = projectile.velocity * 0.1f;
					Main.dust[num8].velocity = Vector2.Normalize(projectile.Center - projectile.velocity * 3f - Main.dust[num8].position) * 1.25f;
				}
			}
			projectile.velocity.Y += HydraHead.FIREBALLGRAVITY;
		}
		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			float scale = projectile.scale;
			Texture2D tex = ModContent.GetTexture("SpiritMod/NPCs/Hydra/HydraFireGlob_Glow");
			Color color = Color.White * 0.65f;
			int frameHeight = tex.Height / Main.projFrames[projectile.type];
			Rectangle frameRect = new Rectangle(0, projectile.frame * frameHeight, tex.Width, frameHeight);
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);

			spriteBatch.Draw(tex, projectile.position - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY), frameRect, color, projectile.rotation, drawOrigin, scale * 1.23f, default, default);
			spriteBatch.Draw(tex, projectile.position - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY), frameRect, color, projectile.rotation, drawOrigin, scale * 1.43f, default, default);

		}

		public override void OnHitPlayer(Player target, int damage, bool crit) => target.AddBuff(BuffID.OnFire, 200);

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			int frameHeight = texture.Height / Main.projFrames[projectile.type];
			Rectangle frameRect = new Rectangle(0, projectile.frame * frameHeight, texture.Width, frameHeight);
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = Color.White * .5f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, frameRect, color, projectile.rotation, drawOrigin, projectile.scale * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length), SpriteEffects.None, 0f);
			}
			spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.position - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY), frameRect, Color.White, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			return false;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 45).WithPitchVariance(0.2f), projectile.Center);
			Main.PlaySound(SoundID.DD2_BetsyFireballImpact, projectile.Center);

			for (int i = 0; i < 20; i++)
				Dust.NewDustPerfect(projectile.Center, DustID.Fire, Main.rand.NextVector2Circular(4, 4));
			Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<HydraExplosion>(), NPCUtils.ToActualDamage(projectile.damage), 3, projectile.owner);
		}
	}

	public class HydraExplosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hydra Spit");
			Main.projFrames[projectile.type] = 6;
		}
		public override void SetDefaults()
		{
			projectile.width = 85;
			projectile.height = 85;
			projectile.hostile = true;
			projectile.tileCollide = false;
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;

		public override void AI()
		{
			Lighting.AddLight(projectile.Center, Color.Orange.ToVector3());
			projectile.frameCounter++;
			projectile.hostile = projectile.frame > 1;
			if (projectile.frameCounter % 3 == 0)
			{
				projectile.frame++;
				if (projectile.frame >= Main.projFrames[projectile.type])
					projectile.active = false;
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit) => target.AddBuff(BuffID.OnFire, 200);

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			int frameHeight = tex.Height / Main.projFrames[projectile.type];
			Rectangle frame = new Rectangle(0, frameHeight * projectile.frame, tex.Width, frameHeight);
			spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - Main.screenPosition, frame, Color.White, projectile.rotation, new Vector2(tex.Width / 2, frameHeight / 2), projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}
	public class HydraPoisonGlob : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hydra Spit");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			projectile.penetrate = 1;
			projectile.width = 24;
			projectile.height = 24;
			projectile.hostile = true;
			projectile.tileCollide = true;
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;
		public override void AI() 
		{
			float num395 = Main.mouseTextColor / 200f - 0.35f;
			num395 *= 0.3f;
			projectile.scale = num395 + 0.85f;

			projectile.velocity.Y += .061f;
			projectile.rotation += .3f;
			Lighting.AddLight(projectile.Center, 0.113f, 0.227f, 0.05f);
			if (Main.rand.NextBool(7))
			{
				Vector2 dustVel = -projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.3f, 0.3f)) * Main.rand.NextFloat(0.4f);
				int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<Dusts.AcidDust>(), dustVel.X, dustVel.Y);
				Main.dust[d].scale = Main.rand.NextFloat(.6f, .8f);
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			int frameHeight = texture.Height / Main.projFrames[projectile.type];
			Rectangle frameRect = new Rectangle(0, projectile.frame * frameHeight, texture.Width, frameHeight);
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = Color.White * .5f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, frameRect, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.position - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY), frameRect, Color.White, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void OnHitPlayer(Player target, int damage, bool crit) => target.AddBuff(BuffID.Poisoned, 200);

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 54).WithPitchVariance(0.2f), projectile.Center);
			Main.PlaySound(new Terraria.Audio.LegacySoundStyle(4, 3).WithPitchVariance(0.2f), projectile.Center);
			Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 112).WithPitchVariance(0.2f).WithVolume(.6f), projectile.Center);

			for (int i = 0; i < 18; i++)
				Dust.NewDustPerfect(projectile.Center, ModContent.DustType<Dusts.AcidDust>(), Main.rand.NextVector2Circular(3, 3));
		}
	}
	public class HydraVenomGlob : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hydra Spit");
			Main.projFrames[projectile.type] = 6;
		}
		public override void SetDefaults()
		{
			projectile.penetrate = 1;
			projectile.width = 24;
			projectile.height = 24;
			projectile.aiStyle = -1;
			projectile.hostile = true;
			projectile.tileCollide = true;
		}
		public void DoTrailCreation(TrailManager tM) => tM.CreateTrail(projectile, new GradientTrail(Color.Orchid, Color.MediumPurple * .75f), new RoundCap(), new DefaultTrailPosition(), 8f, 95f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_2"), 0.01f, 1f, 1f));

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation();
			Lighting.AddLight(projectile.Center, 0.45f, 0.15f, 0.45f);

			if (Main.rand.NextBool(2))
			{
				float x1 = projectile.Center.X - projectile.velocity.X / 10f;
				float y1 = projectile.Center.Y - projectile.velocity.Y / 10f;
				int num1 = Dust.NewDust(new Vector2(x1, y1), 2, 2, DustID.VenomStaff);
				Main.dust[num1].alpha = projectile.alpha;
				Main.dust[num1].velocity = projectile.velocity;
				Main.dust[num1].fadeIn += 0.6684f;
				Main.dust[num1].noGravity = true;
				Main.dust[num1].scale = 1.1235f;
			}

			projectile.frameCounter++;
			if (projectile.frameCounter % 2 == 0)
			{
				projectile.frame++;
				projectile.frame %= Main.projFrames[projectile.type];
			}
		}

		public override void Kill(int timeLeft)
        {
			Main.PlaySound(new Terraria.Audio.LegacySoundStyle(4, 3).WithPitchVariance(0.2f), projectile.Center);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			int frameHeight = tex.Height / Main.projFrames[projectile.type];
			Rectangle frame = new Rectangle(0, frameHeight * projectile.frame, tex.Width, frameHeight);
			spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - Main.screenPosition, frame, Color.White, projectile.rotation, new Vector2(tex.Width * 0.75f, frameHeight / 2), projectile.scale, SpriteEffects.None, 0f);
			return false;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit) => target.AddBuff(BuffID.Venom, 200);
		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}