using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.MeteorMagus
{
	public class MeteorMagus : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Meteor Magus");
			Main.npcFrameCount[npc.type] = 5;
		}
		//large chunk of this(attack pattern use and randomizing) directly ripped from haunted tome, TODO: reduce boilerplate a lot

		private Point frame = new Point(0, 0);

		public override void SetDefaults()
		{
			npc.Size = new Vector2(168, 118);
			npc.lifeMax = 1500;
			npc.damage = 56;
			npc.defense = 28;
			npc.noTileCollide = true;
			npc.noGravity = true;
			npc.aiStyle = -1;
			npc.value = 1100;
			npc.knockBackResist = .55f;
			npc.HitSound = new LegacySoundStyle(SoundID.NPCHit, 55).WithPitchVariance(0.2f);
			npc.DeathSound = SoundID.NPCDeath51;
			npc.visualOffset = new Vector2(84, 0);
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale) => npc.lifeMax = (int)(npc.lifeMax * 0.66f * bossLifeScale);

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => false;

		ref float AiTimer => ref npc.ai[0];

		ref float AttackType => ref npc.ai[1];

		private delegate void Attack(Player player, NPC npc);

		private enum Attacks
		{
			FallingStars = 1,
			CirclingStars = 2,
			ShootingStars = 3
		}

		private static readonly IDictionary<int, Attack> AttackDict = new Dictionary<int, Attack> {
			{ (int)Attacks.CirclingStars, delegate(Player player, NPC npc) { CirclingStars(player, npc); } },
			{ (int)Attacks.FallingStars, delegate(Player player, NPC npc) { FallingStars(player, npc); } },
		};

		private List<int> Pattern = new List<int>
		{
			(int)Attacks.FallingStars,
			(int)Attacks.CirclingStars,
		};

		public override void SendExtraAI(BinaryWriter writer)
		{
			foreach (int i in Pattern)
				writer.Write(i);
		}

		public override void ReceiveExtraAI(BinaryReader reader) => Pattern = Pattern.Select(i => reader.ReadInt32()).ToList();

		private readonly int IdleTime = 210;

		public override void AI()
		{
			Player player = Main.player[npc.target];
			npc.TargetClosest(true);
			npc.spriteDirection = npc.direction;

			if (++AiTimer < IdleTime)
			{
				Vector2 homeCenter = player.Center;

				npc.rotation = npc.velocity.X * .035f;

				switch (Pattern[(int)AttackType])
                {
					case (int)Attacks.FallingStars:
						homeCenter.Y -= 300;
						npc.velocity = new Vector2(MathHelper.Clamp(npc.velocity.X + (0.18f * npc.DirectionTo(homeCenter).X), -4, 4), MathHelper.Clamp(npc.velocity.Y + (0.2f * npc.DirectionTo(homeCenter).Y), -5, 5));
						break;
					default:
						homeCenter.Y -= 50;
						npc.velocity = new Vector2(MathHelper.Clamp(npc.velocity.X + (0.15f * npc.DirectionTo(homeCenter).X), -5, 5), MathHelper.Clamp(npc.velocity.Y + (0.1f * npc.DirectionTo(homeCenter).Y), -2, 2));
						break;
                }
			}

			if (AiTimer == IdleTime && Main.netMode != NetmodeID.Server)
				Main.PlaySound(new LegacySoundStyle(SoundID.Item, 123).WithPitchVariance(0.2f).WithVolume(1.3f), npc.Center);

			if (AiTimer > IdleTime) {
				AttackDict[Pattern[(int)AttackType]].Invoke(Main.player[npc.target], npc);

				npc.rotation = 0f;
				
				UpdateFrame(8, 0, 3);
				frame.X = 1;
			}
			else {
				UpdateFrame(10, 0, 4);
				frame.X = 0;
			}

			npc.localAI[0] = Math.Max(npc.localAI[0] - 0.05f, 0);
		}

		private void ResetPattern()
		{
			AttackType++;
			AiTimer = 0;
			if (AttackType >= Pattern.Count) {
				AttackType = 0;
				Pattern.Randomize();
			}
			npc.netUpdate = true;
		}

		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) => knockback = (AiTimer < IdleTime) ? knockback : 0;

		public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit) => knockback = (AiTimer < IdleTime) ? knockback : 0;

		private void PlayCastSound(Vector2 position)
        {
			if (Main.netMode != NetmodeID.Server)
				Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/starCast").WithPitchVariance(0.3f).WithVolume(0.7f), position);
        }

		private static void FallingStars(Player player, NPC npc)
		{
			MeteorMagus modnpc = npc.modNPC as MeteorMagus;
			npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Zero, 0.1f);

			if (modnpc.AiTimer % 15 == 0)
			{
				modnpc.PlayCastSound(npc.Center);
				if (Main.netMode != NetmodeID.MultiplayerClient)
					Projectile.NewProjectileDirect(npc.Center + new Vector2(Main.rand.Next(-100, 100), Main.rand.Next(-120, -70)),
						Vector2.Zero,
						ModContent.ProjectileType<MortarStar>(),
						npc.damage / 4,
						1,
						Main.myPlayer,
						npc.whoAmI,
						player.whoAmI);
			}

			if (modnpc.AiTimer > 330)
				modnpc.ResetPattern();
		}

		private static void CirclingStars(Player player, NPC npc)
		{
			MeteorMagus modnpc = npc.modNPC as MeteorMagus;
			npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Zero, 0.1f);

			if (modnpc.AiTimer == 220)
			{
				modnpc.PlayCastSound(player.Center);
				int numstars = 3;
				int direction = Main.rand.NextBool() ? -1 : 1;
				Vector2 offset = new Vector2(0, 250).RotatedByRandom(MathHelper.TwoPi);
				for (int i = 0; i < numstars; i++)
                {
					if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
						Projectile projectile = Projectile.NewProjectileDirect(player.Center,
							Vector2.Zero,
							ModContent.ProjectileType<CirclingStar>(),
							npc.damage / 4,
							1,
							Main.myPlayer,
							npc.whoAmI,
							player.whoAmI);

						CirclingStar proj = (projectile.modProjectile as CirclingStar);
						proj.Direction = direction;
						proj.Offset = offset.RotatedBy(MathHelper.TwoPi * i / numstars);
					}
				}
			}

			if (modnpc.AiTimer > 300)
				modnpc.ResetPattern();
		}

		private void UpdateFrame(int framespersecond, int minframe, int maxframe)
		{
			npc.frameCounter++;
			if (npc.frameCounter >= (60 / framespersecond)) {
				frame.Y++;
				npc.frameCounter = 0;
			}
			if (frame.Y > maxframe || frame.Y < minframe)
				frame.Y = minframe;
		}

        public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0) {
				for (int i = 0; i < 6; i++)
				{
					Gore.NewGore(npc.Center, npc.velocity * .5f, 99, Main.rand.NextFloat(.75f, 1f));
				}
			}
		}

        /*public override void NPCLoot()
		{
			npc.DropItem(ModContent.ItemType<Items.Weapon.Magic.ScreamingTome.ScreamingTome>());
			MyWorld.downedTome = true;
			if (Main.netMode != NetmodeID.SinglePlayer)
				NetMessage.SendData(MessageID.WorldData);

			for (int i = 0; i < 8; i++)
				Gore.NewGore(npc.Center, Main.rand.NextVector2Circular(0.5f, 0.5f), 99, Main.rand.NextFloat(0.6f, 1.2f));

			if (Main.netMode != NetmodeID.Server)
				Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/DownedMiniboss"));
		}*/

        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y = frame.Y * frameHeight;
			npc.frame.Width = Main.npcTexture[npc.type].Width / 2;
			npc.frame.X = frame.X * npc.frame.Width;
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
			if (AiTimer > IdleTime) {
				float num395 = Main.mouseTextColor / 200f - 0.35f;
				num395 *= 0.2f;
				float num366 = num395 + 1.15f;
				DrawAfterImage(Main.spriteBatch, new Vector2(0f, 0f), 0.5f, Color.White * .7f, Color.White * .1f, 0.45f, num366, .65f);
			}
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition, npc.frame, drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, SpriteEffects.None, 0);
			return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			void DrawGlowmask(Vector2 position, float opacity = 1)
            {
				spriteBatch.Draw(mod.GetTexture("NPCs/StarjinxEvent/Enemies/MeteorMagus/MeteorMagus_glow"), position - Main.screenPosition, npc.frame, Color.White * opacity, npc.rotation, npc.frame.Size() / 2, npc.scale, SpriteEffects.None, 0);
			}
			DrawGlowmask(npc.Center);

			for(int i = 0; i < 4; i++)
			{
				Vector2 drawpos = npc.Center + new Vector2(0, 4 * (((float)Math.Sin(Main.GlobalTime * 4) / 2) + 0.5f)).RotatedBy(i * MathHelper.PiOver2);
				DrawGlowmask(drawpos, 0.3f);
			}
		}
		public void DrawAfterImage(SpriteBatch spriteBatch, Vector2 offset, float trailLengthModifier, Color color, float opacity, float startScale, float endScale) => DrawAfterImage(spriteBatch, offset, trailLengthModifier, color, color, opacity, startScale, endScale);
        public void DrawAfterImage(SpriteBatch spriteBatch, Vector2 offset, float trailLengthModifier, Color startColor, Color endColor, float opacity, float startScale, float endScale)
        {
            SpriteEffects spriteEffects = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            for (int i = 1; i < 10; i++)
            {
                Color color = Color.Lerp(startColor, endColor, i / 10f) * opacity;
                spriteBatch.Draw(mod.GetTexture("NPCs/StarjinxEvent/Enemies/MeteorMagus/MeteorMagus_Afterimage"), new Vector2(npc.Center.X, npc.Center.Y) + offset - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity * (float)i * trailLengthModifier, npc.frame, color, npc.rotation, npc.frame.Size() * 0.5f, MathHelper.Lerp(startScale, endScale, i / 10f), spriteEffects, 0f);
            }
        }
    }

	internal class MortarStar : ModProjectile, ITrailProjectile, IBasicPrimDraw
    {
		public override string Texture => "Terraria/Projectile_1";

        public override void SetStaticDefaults() => DisplayName.SetDefault("Shooting Star");

        public override void SetDefaults()
		{
			projectile.Size = new Vector2(32, 32);
			projectile.scale = Main.rand.NextFloat(0.8f, 1.2f);
			projectile.hostile = true;
			projectile.tileCollide = true;
			projectile.alpha = 255;
			projectile.hide = true;
		}

		internal NPC Parent => Main.npc[(int)projectile.ai[0]]; 
		internal Player Target => Main.player[(int)projectile.ai[1]];

		internal ref float Timer => ref projectile.localAI[0];

		readonly float gravity = 0.3f;

		public override bool PreAI()
		{
			for (int i = 0; i < 2; i++)
            {
                float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
                float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;

                int num = Dust.NewDust(projectile.position, 6, 6, 223, 0f, 0f, 0, default(Color), .35f);
                Main.dust[num].position = projectile.Center - projectile.velocity / num * (float)i;

                Main.dust[num].velocity = projectile.velocity;
                Main.dust[num].scale = MathHelper.Clamp(projectile.ai[0], .015f, 1.25f);
                Main.dust[num].noGravity = true;
                Main.dust[num].fadeIn = (float)(100 + projectile.owner);

            }
			return true;
		}

		public override void AI()
		{

			projectile.alpha = Math.Max(projectile.alpha - 15, 0);
			if (projectile.localAI[1] == 0)
				projectile.localAI[1] = Main.rand.NextBool() ? -0.2f : 0.2f;

			projectile.rotation += projectile.localAI[1];

			if (!Parent.active || !Target.active || Target.dead)
				projectile.Kill();

			if(++Timer == 20)
				projectile.velocity = GetArcVel().RotatedByRandom(MathHelper.Pi / 12);

			else if (Timer > 20)
				projectile.velocity.Y += gravity;
        }

		Vector2 GetArcVel()
        {
			Vector2 DistanceToTravel = Target.Center - projectile.Center;
			float MaxHeight = MathHelper.Clamp(DistanceToTravel.Y - 100, -400, -100);
			float TravelTime = (float)Math.Sqrt(-2 * MaxHeight / gravity) + (float)Math.Sqrt(2 * Math.Max(DistanceToTravel.Y - MaxHeight, 0) / gravity);
			return new Vector2(MathHelper.Clamp(DistanceToTravel.X / TravelTime, -15, 15), - (float)Math.Sqrt(-2 * gravity * MaxHeight));
		}

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateTrail(projectile, new GradientTrail(new Color(228, 31, 156), new Color(180, 88, 237)), new RoundCap(), new ArrowGlowPosition(), 100f, 180f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_4"), 0.01f, 1f, 1f));
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, new Color(241, 153, 255) * .25f, new Color(241, 153, 255) * .125f), new RoundCap(), new ArrowGlowPosition(), 42f, 200f, new DefaultShader());
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, new Color(228, 31, 156, 150), new Color(228, 31, 156, 150) * 0.5f), new RoundCap(), new DefaultTrailPosition(), 20f, 80f, new DefaultShader());
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, new Color(228, 31, 156, 150), new Color(228, 31, 156, 150) * 0.5f), new RoundCap(), new DefaultTrailPosition(), 20f, 80f, new DefaultShader());
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, new Color(241, 153, 255) * .5f, new Color(241, 153, 255) * .25f), new RoundCap(), new ArrowGlowPosition(), 42f, 40f, new DefaultShader());
		}

		public void DrawPrimShape(BasicEffect effect) => StarDraw.DrawStarBasic(effect, projectile.Center, projectile.rotation, projectile.scale * 15, Color.White * projectile.Opacity);

        public override void Kill(int timeLeft)
        {
            DustHelper.DrawStar(projectile.Center, 223, pointAmount: 5, mainSize: .9425f, dustDensity: 2, dustSize: .5f, pointDepthMult: 0.3f, noGravity: true);
			if (Main.netMode != NetmodeID.Server)
				Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/starHit").WithVolume(0.65f).WithPitchVariance(0.3f), projectile.Center);
		}
    }

	internal class CirclingStar : MortarStar //inheriting from mortar star to cut down on boilerplate, since they share the same visuals
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Shooting Star");

		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.tileCollide = false;
		}

		private Vector2 homeCenter;

		public float Direction;

		public Vector2 Offset;

		readonly float circlingtime = 60;

		public override void AI()
		{


			projectile.alpha = Math.Max(projectile.alpha - 15, 0);
			if (!Parent.active || !Target.active || Target.dead)
            {
				projectile.Kill();
				return;
			}

            else
            {
				float speed = Math.Max((circlingtime - Timer) / (circlingtime * 0.75f), 0.25f);
				projectile.rotation += speed * Direction / 5;

				if (++Timer < 60)
					homeCenter = Target.Center;
				else
					Offset *= (float)Math.Pow((circlingtime / Timer)/10 + 0.9f, 3);

				projectile.Center = Offset + homeCenter;
				Offset = Offset.RotatedBy(speed * Direction * MathHelper.Pi / 30);

				if (projectile.Distance(homeCenter) < 10)
					projectile.Kill();
			}
		}
	}
}