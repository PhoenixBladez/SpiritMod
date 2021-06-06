using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using SpiritMod.Buffs;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.BossBags;
using SpiritMod.NPCs.Boss.MoonWizardTwo.Projectiles;
using SpiritMod.Prim;

namespace SpiritMod.NPCs.Boss.MoonWizardTwo
{
	[AutoloadBossHead]
	public class MoonWizardTwo : ModNPC
	{
		public int cooldownCounter = 50;

		private bool phaseTwo => npc.life < 3000;
		private bool attacking => cooldownCounter <= 0;

		private Vector2 projectileStart => npc.Center - new Vector2(0, 60);

		private float trueFrame = 0;
		private int attackCounter = 0;
		private int preAttackCounter = 0;

		const int NUMBEROFATTACKS = 2;
		private enum CurrentAttack
		{
			InwardPull = 0,
			SineBalls = 1,
			SkyStrikes = 2,
		}
		private CurrentAttack currentAttack
		{
			get
			{
				return (CurrentAttack)(int)npc.ai[1];
			}
			set
			{
				npc.ai[1] = (int)value;
			}
		}


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mystic Moon Jelly Wizard");
			Main.npcFrameCount[npc.type] = 21;
			NPCID.Sets.TrailCacheLength[npc.type] = 10;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.friendly = false;
			npc.lifeMax = 12000;
			npc.defense = 20;
			npc.value = 40000;
			npc.aiStyle = -1;
            bossBag = ModContent.ItemType<MJWBag>();
            npc.knockBackResist = 0f;
			npc.width = 17;
			npc.height = 35;
			npc.damage = 0;
            npc.scale = 2f;
			npc.lavaImmune = true;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.buffImmune[ModContent.BuffType<FesteringWounds>()] = true;
            npc.buffImmune[BuffID.Venom] = true;
            npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit13;
			npc.DeathSound = SoundID.NPCDeath2;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/MoonJelly");
            npc.boss = true;
		}

		public void DrawAfterImage(SpriteBatch spriteBatch, Vector2 offset, float trailLengthModifier, Color color, float opacity, float startScale, float endScale) => DrawAfterImage(spriteBatch, offset, trailLengthModifier, color, color, opacity, startScale, endScale);
		public void DrawAfterImage(SpriteBatch spriteBatch, Vector2 offset, float trailLengthModifier, Color startColor, Color endColor, float opacity, float startScale, float endScale)
		{
			SpriteEffects spriteEffects = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			for (int i = 1; i < 10; i++)
			{
				Color color = Color.Lerp(startColor, endColor, i / 10f) * opacity;
				spriteBatch.Draw(mod.GetTexture("NPCs/Boss/MoonWizardTwo/MoonWizardTwo_Afterimage"), new Vector2(npc.Center.X, npc.Center.Y) + offset - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity * (float)i * trailLengthModifier, npc.frame, color, npc.rotation, npc.frame.Size() * 0.5f, MathHelper.Lerp(startScale, endScale, i / 10f), spriteEffects, 0f);
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			float num395 = Main.mouseTextColor / 200f - 0.35f;
			num395 *= 0.2f;
			float num366 = num395 + 2.45f;
			if (npc.velocity != Vector2.Zero || phaseTwo)
			{
				DrawAfterImage(Main.spriteBatch, new Vector2(0f, -18f), 0.5f, Color.White * .7f, Color.White * .1f, 0.75f, num366, 1.65f);
			}
			return false;
		}


		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			var effects = npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], new Vector2(npc.Center.X, npc.Center.Y - 18) - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame, lightColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			DrawSpecialGlow(spriteBatch, lightColor);
		}
		public void DrawSpecialGlow(SpriteBatch spriteBatch, Color drawColor)
		{
			float num108 = 4;
			float num107 = (float)Math.Cos((double)(Main.GlobalTime % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;
			float num106 = 0f;

			SpriteEffects spriteEffects3 = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Vector2 vector33 = new Vector2(npc.Center.X, npc.Center.Y - 18) - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity;
			Color color29 = new Color(127 - npc.alpha, 127 - npc.alpha, 127 - npc.alpha, 0).MultiplyRGBA(Color.LightBlue);
			for (int num103 = 0; num103 < 4; num103++)
			{
				Color color28 = color29;
				color28 = npc.GetAlpha(color28);
				color28 *= 1f - num107;
				Vector2 vector29 = new Vector2(npc.Center.X, npc.Center.Y - 18) + ((float)num103 / (float)num108 * 6.28318548f + npc.rotation + num106).ToRotationVector2() * (4f * num107 + 2f) - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity * (float)num103;
				Main.spriteBatch.Draw(mod.GetTexture("NPCs/Boss/MoonWizardTwo/MoonWizardTwo_Glow"), vector29, npc.frame, color28, npc.rotation, npc.frame.Size() / 2f, npc.scale, spriteEffects3, 0f);
			}
			Main.spriteBatch.Draw(mod.GetTexture("NPCs/Boss/MoonWizardTwo/MoonWizardTwo_Glow"), vector33, npc.frame, color29, npc.rotation, npc.frame.Size() / 2f, npc.scale, spriteEffects3, 0f);

		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale) => npc.lifeMax = (int)(npc.lifeMax * 0.8f * bossLifeScale);
		//0-3: idle
		//4-9 propelling
		//10-13 skirt up
		//14-21: turning
		//22-28: kick
		//29-38: Teleport
		//39-51:Teleport part 2
		//54-61: Weird float

		public override void AI()
		{
			Lighting.AddLight(new Vector2(npc.Center.X, npc.Center.Y), 0.075f * 2, 0.231f * 2, 0.255f * 2);
			npc.TargetClosest();
			if (!attacking)
			{
				npc.ai[1] %= NUMBEROFATTACKS; //make sure the current attack is within the index
				attackCounter = 0;
				preAttackCounter = 0;
				cooldownCounter--;
				npc.velocity = Vector2.Zero;
				npc.rotation = 0;
				if (phaseTwo)
					UpdateFrame(0.15f, 54, 61);
				else
					UpdateFrame(0.15f, 0, 3);
			}
			else
			{
				bool attackStart = false;
				switch(currentAttack)
				{
					case CurrentAttack.InwardPull:
						attackStart = DoInwardPull();
						break;
					case CurrentAttack.SineBalls:
						attackStart = DoSineBalls();
						break;
					case CurrentAttack.SkyStrikes:
						break;
				}
				if (attackStart)
					attackCounter++;
				else
					preAttackCounter++;
			}
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frame.Width = 60;
			npc.frame.X = ((int)trueFrame % 3) * npc.frame.Width;
			npc.frame.Y = (((int)trueFrame - ((int)trueFrame % 3)) / 3) * npc.frame.Height;
		}

		public void UpdateFrame(float speed, int minFrame, int maxFrame)
		{
			trueFrame += speed;
			if (trueFrame < minFrame) 
			{
				trueFrame = minFrame;
			}
			if (trueFrame > maxFrame) 
			{
				trueFrame = minFrame;
			}
		}

		#region attacks

		private bool DoInwardPull()
		{
			Player player = Main.player[npc.target];
			UpdateFrame(0.15f, 10, 13);
			if (attackCounter == 70)
			{
				Vector2 direction = player.Center - projectileStart;
				int proj = Projectile.NewProjectile(projectileStart, Vector2.Zero, ModContent.ProjectileType<MysticWall>(), npc.damage / 2, 3, npc.target, 1);
				Projectile projectileOne = Projectile.NewProjectileDirect(projectileStart, Vector2.Zero, ModContent.ProjectileType<MysticWall>(), npc.damage / 2, 3, npc.target, -1, proj + 1);
				Projectile projectileTwo = Main.projectile[proj];
				projectileOne.hostile = true;
				projectileOne.friendly = false;
				projectileTwo.hostile = true;
				projectileTwo.friendly = false;

				if (projectileOne.modProjectile is MysticWall modproj)
				{
					modproj.Parent = npc;
					modproj.InitialDistance = (int)direction.Length() + 500;
				}
				if (projectileTwo.modProjectile is MysticWall modproj2)
				{
					modproj2.Parent = npc;
					modproj2.InitialDistance = (int)direction.Length() + 500;
				}
				SpiritMod.primitives.CreateTrail(new MSineOrbPrimTrail(projectileOne, projectileTwo));
			}
			return true;
		}
		private bool DoSineBalls()
		{
			Player player = Main.player[npc.target];
			UpdateFrame(0.15f, 10, 13);
			if (attackCounter % 60 == 30)
			{
				Vector2 direction = player.Center - projectileStart;
				direction.Normalize();
				int proj = Projectile.NewProjectile(projectileStart, direction, ModContent.ProjectileType<MysticSineBall>(), npc.damage / 2, 3, npc.target, 180);
				Projectile projectileOne = Projectile.NewProjectileDirect(projectileStart, direction, ModContent.ProjectileType<MysticSineBall>(), npc.damage / 2, 3, npc.target, 0, proj + 1);
				Projectile projectileTwo = Main.projectile[proj];

				projectileOne.hostile = true;
				projectileOne.friendly = false;
				projectileTwo.hostile = true;
				projectileTwo.friendly = false;
				SpiritMod.primitives.CreateTrail(new MSineOrbPrimTrail(projectileOne, projectileTwo));
			}
			if (attackCounter > 220)
			{
				npc.ai[1]++;
				cooldownCounter = 30;
			}
			if (preAttackCounter > 30)
				return true;
			return false;
		}
		#endregion
	}
}
