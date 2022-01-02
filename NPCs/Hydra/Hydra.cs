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

namespace SpiritMod.NPCs.Hydra
{
	public class Hydra : ModNPC
	{
		private const int MAXHEADS = 4;

		private bool initialized = false;

		private List<NPC> heads = new List<NPC>();

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
			npc.defense = 0;
			npc.lifeMax = 1000;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath14;
			npc.value = 180f;
			npc.knockBackResist = 0;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.immortal = true;
			npc.dontTakeDamage = true;
			npc.hide = true;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
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
					SpawnHead(npc.lifeMax);
				}
				headsDue = 1;
			}
		}

		public void SpawnHead(int life)
		{
			if (heads.Count >= MAXHEADS)
				return;
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

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lernean Hydra");
		}
		public override void SetDefaults()
		{
			npc.width = 44;
			npc.height = 32;
			npc.damage = 55;
			npc.defense = 0;
			npc.lifeMax = 1000;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath14;
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

			if (!attacking)
			{
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

			rotation += rotationSpeed;
			sway += swaySpeed;

			npc.spriteDirection = npc.direction = parent.direction;
			centralRotation = 0.3f * ((float)Math.Sin(sway) + (npc.direction * 1.5f));
			posToBe = DecidePosition();

			MoveToPosition();

			if (npc.direction == 1)
			{
				npc.rotation = npc.DirectionTo(Main.player[parent.target].Center).ToRotation();
			}
			else
			{
				npc.rotation = npc.DirectionTo(Main.player[parent.target].Center).ToRotation() - 3.14f;
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
			LaunchProjectile();
			attacking = false;
		}

		private void LaunchProjectile()
		{
			Vector2 direction = npc.DirectionTo(Main.player[parent.target].Center);
			npc.velocity = -direction * Main.rand.Next(7,10);
			switch (headColor)
			{
				case HeadColor.Red:
					Projectile.NewProjectile(npc.Center, direction * 10, ModContent.ProjectileType<HydraFireGlob>(), NPCUtils.ToActualDamage(npc.damage), 3);
					break;
				case HeadColor.Green:
					if (Main.rand.NextBool())
					{
						for (float i = -0.5f; i <= 0.5f; i += 0.5f)
							Projectile.NewProjectile(npc.Center, direction.RotatedBy(i) * 8, ModContent.ProjectileType<HydraPoisonGlob>(), NPCUtils.ToActualDamage(npc.damage), 3);
					}
					else
					{
						float rotationOffset = Main.rand.NextFloat(0.15f, 0.4f);
						Projectile.NewProjectile(npc.Center, direction.RotatedBy(rotationOffset) * 8, ModContent.ProjectileType<HydraPoisonGlob>(), NPCUtils.ToActualDamage(npc.damage), 3);
						Projectile.NewProjectile(npc.Center, direction.RotatedBy(-rotationOffset) * 8, ModContent.ProjectileType<HydraPoisonGlob>(), NPCUtils.ToActualDamage(npc.damage), 3);
					}
					break;
				case HeadColor.Purple:
					Projectile.NewProjectile(npc.Center, direction * 15, ModContent.ProjectileType<HydraVenomGlob>(), NPCUtils.ToActualDamage(npc.damage), 3);
					break;
				default:
					break;
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0 && parent.modNPC is Hydra modNPC)
			{
				if (modNPC.newHeadCountdown < 0)
					modNPC.newHeadCountdown = 150;
				modNPC.headsDue++;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			string colorString = getColor();
			string texturePath = Texture + colorString;
			Texture2D headTex = ModContent.GetTexture(texturePath);
			Texture2D neckTex = ModContent.GetTexture(texturePath + "_Neck");

			Vector2 direction = npc.Center - parent.Bottom;
			Vector2 centralPos = (new Vector2(0, -1) * direction.Length());

			//Control point relative to the parent npc
			Vector2 BaseControlPoint = parent.Bottom + (centralPos.RotatedBy(-centralRotation / 2) * 0.5f);

			//Control point connecting to behind the npc, to make the neck look more like a neck
			float headControlLength = 100;
			Vector2 HeadControlPoint = npc.Center - Vector2.UnitX.RotatedBy(npc.rotation + ((npc.spriteDirection < 0) ? MathHelper.Pi : 0)) * headControlLength;

			//Control point to smooth out the bezier, taking the midway point beween the other 2 control points and moving it backwards from them perpindicularly 
			float smootheningFactor = 0.4f;
			Vector2 SmootheningControlPoint = Vector2.Lerp(BaseControlPoint, HeadControlPoint, 0.5f) + (HeadControlPoint - BaseControlPoint).RotatedBy(-npc.spriteDirection * MathHelper.PiOver2) * smootheningFactor;

			BezierCurve curve = new BezierCurve(new Vector2[] { parent.Bottom, BaseControlPoint, SmootheningControlPoint, HeadControlPoint, npc.Center });

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

			spriteBatch.Draw(headTex, npc.Center - Main.screenPosition, null, drawColor, npc.rotation, new Vector2(headTex.Width, headTex.Height) / 2, npc.scale, npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			return false;
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
	public class HydraFireGlob : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hydra Spit");
			Main.projFrames[projectile.type] = 4;
		}
		public override void SetDefaults()
		{
			projectile.penetrate = 1;
			projectile.width = 24;
			projectile.height = 24;
			projectile.aiStyle = 1;
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
		}

		public override void OnHitPlayer(Player target, int damage, bool crit) => target.AddBuff(BuffID.OnFire, 200);

		public override Color? GetAlpha(Color lightColor) => Color.White;

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			int frameHeight = tex.Height / Main.projFrames[projectile.type];
			Rectangle frame = new Rectangle(0, frameHeight * projectile.frame, tex.Width, frameHeight);
			spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - Main.screenPosition, frame, Color.White, projectile.rotation, new Vector2(tex.Width * 0.75f, frameHeight / 2), projectile.scale, SpriteEffects.None, 0f);
			return false;
		}

		public override void Kill(int timeLeft)
		{
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
		}
		public override void SetDefaults()
		{
			projectile.penetrate = 1;
			projectile.width = 24;
			projectile.height = 24;
			projectile.aiStyle = 1;
			projectile.hostile = true;
			projectile.tileCollide = true;
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;

		public override void OnHitPlayer(Player target, int damage, bool crit) => target.AddBuff(BuffID.Poisoned, 200);

	}
	public class HydraVenomGlob : ModProjectile
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
		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation();
			if (Main.rand.NextBool(3))
			{
				Vector2 dustVel = -projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.3f, 0.3f)) * Main.rand.NextFloat(0.4f);
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.VenomStaff, dustVel.X, dustVel.Y);
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
			for (int i = 0; i < 10; i++)
				Dust.NewDustPerfect(projectile.Center, DustID.VenomStaff, Main.rand.NextVector2Circular(2, 2));
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