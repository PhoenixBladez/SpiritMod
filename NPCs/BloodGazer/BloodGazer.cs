using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Prim;
using SpiritMod.VerletChains;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.BloodGazer
{
	internal enum BloodGazerAiStates
	{
		Passive = 0,
		EyeSpawn = 1,
		Hostile = 2,
		EyeSwing = 3,
		EyeSpin = 4,
		DetatchingEyes = 5,
		Despawn = 6
	}

	public class BloodGazer : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Gazer");
			NPCID.Sets.TrailCacheLength[npc.type] = 5;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.Size = new Vector2(50, 68);
			npc.damage = 45;
			npc.defense = 8;
			npc.lifeMax = 7000;
			npc.knockBackResist = 0.1f;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.npcSlots = 3;
			npc.aiStyle = -1;
			Main.npcFrameCount[npc.type] = 4;
			npc.HitSound = SoundID.NPCHit19;
			npc.DeathSound = SoundID.NPCDeath22;
            banner = npc.type;
            bannerItem = ModContent.ItemType<Items.Banners.BloodGazerBanner>();
        }

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * bossLifeScale * 0.66f);
			npc.damage = (int)(npc.damage * bossLifeScale * 0.66f);
		}

		private ref float AiState => ref npc.ai[0];
		private ref float AiTimer => ref npc.ai[1];

		public bool trailing = false;

		private void UpdateAiState(BloodGazerAiStates aistate)
		{
			AiState = (float)aistate;
			AiTimer = 0;
			npc.netUpdate = true;
		}

		public override void AI()
		{
			npc.spriteDirection = npc.direction;
			npc.TargetClosest();
			Player player = Main.player[npc.target];
			if (player.active && !player.dead && Collision.CanHit(npc.Center, 0, 0, player.Center, 0, 0) && npc.Distance(player.Center) < 500 && AiState == (float)BloodGazerAiStates.Passive)   //aggro on player if able to reach them, and in its passive state
				UpdateAiState(BloodGazerAiStates.EyeSpawn);

			AiTimer++;
			npc.rotation = npc.velocity.X * 0.05f;
			if (Main.netMode != NetmodeID.Server && Main.rand.Next(60) == 0)
				Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/HeartbeatFx"));

			switch ((BloodGazerAiStates)AiState) {
				case BloodGazerAiStates.EyeSpawn:
					npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Zero, 0.1f);

					if (AiTimer % 60 == 0) {
						if (Main.netMode != NetmodeID.Server)
							Main.PlaySound(new LegacySoundStyle(SoundID.NPCKilled, 22).WithPitchVariance(0.2f).WithVolume(0.8f), npc.Center);

						if (Main.netMode != NetmodeID.MultiplayerClient) {
							NPC eye = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<BloodGazerEye>(), 0, npc.whoAmI, (int)(AiTimer / 60))];
							Vector2 velocity = Main.rand.NextVector2CircularEdge(1, 1) * Main.rand.NextFloat(2, 4);
							eye.velocity = velocity;
							(eye.modNPC as BloodGazerEye).InitializeChain(npc.Center);
							for (int i = 0; i < 25; i++) {
								Dust dust = Dust.NewDustDirect(npc.Center, 10, 10, ModContent.DustType<Dusts.Blood>(), 0f, -2f, 0, default, Main.rand.NextFloat(0.9f, 1.5f));
								dust.velocity = velocity.RotatedByRandom(MathHelper.Pi / 14) * Main.rand.NextFloat(0.5f, 1f);
							}
							npc.velocity = -velocity / 2;
							eye.netUpdate = true;
						}
						npc.netUpdate = true;
					}

					if (AiTimer > 180)
						UpdateAiState(BloodGazerAiStates.Hostile);

					break;

				case BloodGazerAiStates.Hostile:
					npc.velocity = Vector2.Lerp(npc.velocity, npc.DirectionTo(player.Center), 0.03f);

					if (AiTimer >= 200)
						UpdateAiState(Main.rand.NextBool() ? BloodGazerAiStates.EyeSwing : BloodGazerAiStates.EyeSwing);

					break;

				case BloodGazerAiStates.EyeSwing:
					npc.velocity = Vector2.Lerp(npc.velocity, npc.DirectionTo(player.Center), 0.03f);

					if (AiTimer >= 250)
						UpdateAiState(BloodGazerAiStates.DetatchingEyes);

					break;

				case BloodGazerAiStates.EyeSpin:
					npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Zero, 0.03f);

					if (AiTimer >= 180)
						UpdateAiState(BloodGazerAiStates.DetatchingEyes);

					break;

				case BloodGazerAiStates.DetatchingEyes:
					npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Zero, 0.03f);

					if (AiTimer >= 180)
						UpdateAiState(BloodGazerAiStates.EyeSpawn);
					break;

				case BloodGazerAiStates.Passive:
					npc.spriteDirection = Math.Sign(npc.velocity.X) < 0 ? -1 : 1;
					AiTimer++;
					npc.velocity.X = (float)Math.Sin(AiTimer / 720) * 2;
					npc.velocity.Y = (float)Math.Cos(AiTimer / 180);
					break;

				case BloodGazerAiStates.Despawn:
					npc.knockBackResist = 0f;
					npc.spriteDirection = Math.Sign(npc.velocity.X) < 0 ? -1 : 1;
					npc.timeLeft = Math.Min(npc.timeLeft - 1, 60);
					AiTimer++;
					npc.velocity.X = MathHelper.Lerp(npc.velocity.X, npc.spriteDirection * 14, 0.0015f);
					npc.velocity.Y = (float)Math.Cos(AiTimer / 180);
					break;
			}

			if((!player.active || player.dead || npc.Distance(player.Center) > 1200) && AiState != (float)BloodGazerAiStates.Passive && AiState != (float)BloodGazerAiStates.Despawn)  //deaggro if player is dead or too far away
				UpdateAiState(BloodGazerAiStates.Passive);

			if (((npc.Distance(player.Center) > 2000) || Main.dayTime) && AiState != (float)BloodGazerAiStates.Despawn)  //despawn if day
				UpdateAiState(BloodGazerAiStates.Despawn);
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => trailing;

		public override bool? CanHitNPC(NPC target) => trailing;

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0 || npc.life >= 0) {
				int d = 5;
				for (int k = 0; k < 25; k++) {
					Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.47f);
					Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, .97f);
				}
			}

			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Gazer/Gazer1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Gazer/Gazer2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Gazer/Gazer3"), 1f);
				int d = 5;
				for (int k = 0; k < 25; k++) {
					Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.97f);
					Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 1.27f);
				}
			}
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D tex = Main.npcTexture[npc.type];
			if (trailing) {
				for (int i = 0; i < NPCID.Sets.TrailCacheLength[npc.type]; i++) {
					float opacity = 0.25f * (float)(NPCID.Sets.TrailCacheLength[npc.type] - i) / NPCID.Sets.TrailCacheLength[npc.type];
					spriteBatch.Draw(tex, npc.oldPos[i] + npc.Size/2 - Main.screenPosition, npc.frame, drawColor * opacity, npc.rotation, npc.frame.Size() / 2, npc.scale, SpriteEffects.None, 0);
				}
			}

			return true;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.spawnTileY < Main.rockLayer && (Main.bloodMoon) && Main.hardMode && !NPC.AnyNPCs(ModContent.NPCType<BloodGazer>()) ? 0.024f : 0f;
	}

	internal class BloodGazerEye : ModNPC
	{
		private Chain chain;

		private NPC Parent => Main.npc[(int)npc.ai[0]];

		private Player Target => Main.player[Parent.target];

		private bool Active => Parent.active && Parent.type == ModContent.NPCType<BloodGazer>() && ((Parent.ai[0] != (float)BloodGazerAiStates.Passive && Parent.ai[0] != (float)BloodGazerAiStates.Despawn) || detatched);
		public override void SetStaticDefaults() => DisplayName.SetDefault("Blood Gazer");

		public override void SetDefaults()
		{
			npc.Size = new Vector2(26, 24);
			npc.damage = 45;
			npc.defense = 13;
			npc.lifeMax = 800;
			npc.knockBackResist = 0.5f;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.dontCountMe = true;
			npc.aiStyle = -1;
			npc.HitSound = SoundID.NPCHit19;
			npc.DeathSound = SoundID.NPCDeath22;
            for (int i = 0; i < BuffLoader.BuffCount; i++)
            {
                npc.buffImmune[i] = true;
            }
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale) => npc.damage = (int)(npc.damage * bossLifeScale * 0.66f);

		public void InitializeChain(Vector2 position) => chain = new Chain(ModContent.GetTexture(Texture + "_chain"), 8, 16, position, new ChainPhysics(0.9f, 0.5f, 0f));

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;

		public override bool CheckActive() => !Active;

		public override bool CheckDead() => !Active;

		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) => damage = 0;

		public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit) => damage = 0;

		public override bool PreNPCLoot() => false;

		ref float AiTimer => ref Parent.ai[1];

		ref float EyeNumber => ref npc.ai[1];

		ref float ShootTimer => ref npc.ai[2];

		ref float KillTimer => ref npc.ai[3];

		private bool contactdamage = false;

		private bool detatched = false;

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => contactdamage;

		public override void AI()
		{
			if (!Active) {
				npc.active = false;
				npc.NPCLoot();
				return;
			}
			npc.knockBackResist = 0.5f;

			npc.realLife = Parent.whoAmI;

			float ChainLength = (chain.Texture.Height * chain.Segments.Count);
			if (detatched) {
				chain.FirstVertex.Static = false;
				contactdamage = true;
				KillTimer++;
				if(KillTimer > 270) {
					npc.active = false;
					npc.NPCLoot();
					return;
				}

				npc.velocity = Vector2.Lerp(npc.velocity, npc.DirectionTo(Target.Center) * 22, 0.015f);
			}

			else {
				float pullbackstrength = 0.25f;
				switch ((BloodGazerAiStates)Parent.ai[0]) {
					case (BloodGazerAiStates.Hostile):
						npc.velocity = Vector2.Lerp(npc.velocity, npc.DirectionTo(Target.Center) * 6, 0.02f * (1 / npc.ai[1]));
						break;
					case (BloodGazerAiStates.EyeSwing):
						pullbackstrength = 1.5f;
						npc.knockBackResist = 0f;
						float chandistratio = 0.3f;
						Vector2 targetPos = Parent.Center + Parent.DirectionTo(Target.Center).RotatedBy(MathHelper.PiOver4 * ((npc.ai[1] % 2 == 0) ? -1 : 1)) * ChainLength * chandistratio;
						//entire attack takes 60 ticks, offset by eye number so only 1 swing happens at a time, and offset by a static 20 ticks so the first swing takes longer
						int SwingTime = 60;
						int StaticDelay = 20;
						int starthomingtime = (int)(SwingTime * (EyeNumber - 2f)) + StaticDelay;
						int startpullbacktime = (int)(SwingTime * (EyeNumber - 0.3f)) + StaticDelay;
						int startswingtime = (int)(SwingTime * (EyeNumber - 0.25f)) + StaticDelay;
						int startslowdowntime = (int)(SwingTime * (EyeNumber + 0.05f)) + StaticDelay;

						if (AiTimer >= starthomingtime && AiTimer < startpullbacktime)  //half time spent homing in on target position
							npc.velocity = Vector2.Lerp(npc.velocity, npc.DirectionTo(targetPos) * MathHelper.Clamp(npc.Distance(targetPos) / 40, 6, 12), 0.1f);

						if (AiTimer >= startpullbacktime && AiTimer < startswingtime) //weak pull back before the swing
							npc.velocity = Vector2.Lerp(npc.velocity, 25 * (npc.Distance(Parent.Center) / ChainLength) * Parent.DirectionTo(Target.Center).RotatedBy(MathHelper.PiOver4 * ((npc.ai[1] % 2 == 0) ? -1 : 1)), 0.2f);

						if (AiTimer == startswingtime) //swing
						{
							npc.velocity = 50 * (npc.Distance(Parent.Center) / ChainLength) * Parent.DirectionTo(Target.Center).RotatedBy(MathHelper.PiOver4 * ((npc.ai[1] % 2 == 0) ? 1 : -1));
							contactdamage = true;
							(Parent.modNPC as BloodGazer).trailing = true;
							Parent.velocity = Parent.DirectionTo(Target.Center) * 6 * npc.Distance(Parent.Center) / (ChainLength * chandistratio);
							if (Main.netMode != NetmodeID.Server)
								Main.PlaySound(SoundLoader.customSoundType, (int)Parent.Center.X, (int)Parent.Center.Y, mod.GetSoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/VanillaBossRoar"), 0.65f, -0.25f);
						}

						if (AiTimer >= startswingtime && AiTimer <= startslowdowntime) //swing
						{
							ShootTimer++;
							int numshots = 5;
							if (ShootTimer >= (startslowdowntime - startswingtime) / numshots) {
								ShootTimer = 0;
								if (Main.netMode != NetmodeID.Server)
									Main.PlaySound(SoundID.Item, npc.Center, 95);

								if (Main.netMode != NetmodeID.MultiplayerClient)
									Projectile.NewProjectileDirect(npc.Center, npc.DirectionFrom(Parent.Center) * 5, ModContent.ProjectileType<BloodGazerEyeShot>(), NPCUtils.ToActualDamage(40, 1.5f), 1, Main.myPlayer).netUpdate = true;

								npc.velocity += npc.DirectionTo(Parent.Center);
							}
						}

						if (AiTimer == startslowdowntime) {
							contactdamage = false;
							(Parent.modNPC as BloodGazer).trailing = false;
						}

						if (AiTimer > startslowdowntime)  //slow down after a bit
							npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Zero, 0.03f);

						break;
					case (BloodGazerAiStates.EyeSpin):
						npc.velocity = Vector2.Lerp(npc.velocity, npc.DirectionTo(Target.Center) * 4, 0.02f * (1 / npc.ai[1]));
						break;
					case (BloodGazerAiStates.DetatchingEyes):
						pullbackstrength = 0.75f;
						if (AiTimer > 30 * EyeNumber) {
							detatched = true;
							Parent.velocity = npc.DirectionTo(Parent.Center) * 4;
							if (Main.netMode != NetmodeID.Server)
								Main.PlaySound(new LegacySoundStyle(SoundID.NPCKilled, 22).WithPitchVariance(0.2f).WithVolume(0.8f), Parent.Center);

							for (int i = 0; i < 25; i++) {
								Dust dust = Dust.NewDustDirect(Parent.Center, 10, 10, ModContent.DustType<Dusts.Blood>(), 0f, -2f, 0, default, Main.rand.NextFloat(0.9f, 1.5f));
								dust.velocity = npc.DirectionFrom(Parent.Center).RotatedByRandom(MathHelper.Pi / 14) * Main.rand.NextFloat(2f, 6f);
							}
						}
						break;
				}
				npc.velocity += npc.DirectionTo(Parent.Center) * pullbackstrength * npc.Distance(Parent.Center) / ChainLength;
			}
			chain.Update(Parent.Center, npc.Center);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			chain.Draw(spriteBatch, out float endrot, out Vector2 endpos);
			Texture2D tex = Main.npcTexture[npc.type];

			spriteBatch.Draw(tex, endpos - new Vector2(chain.Texture.Height / 2, -chain.Texture.Width / 2).RotatedBy(endrot) - Main.screenPosition, tex.Bounds, drawColor, endrot, tex.Bounds.Size() / 2, npc.scale, SpriteEffects.None, 0);
			return false;
		}

		public override void NPCLoot()
		{

		}
	}

	internal class BloodGazerEyeShot : ModProjectile
	{
		public override string Texture => "Terraria/Projectile_1";
		public override void SetStaticDefaults() => DisplayName.SetDefault("Blood Shot");

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.WoodenArrowHostile);
			projectile.width = 10;
			projectile.height = 10;
			projectile.hostile = true;
			projectile.alpha = 255;
			projectile.penetrate = 1;
			projectile.timeLeft = 120;
		}

		bool primsCreated = false;
		public override void AI()
		{
			if (projectile.velocity.Length() < 22)
				projectile.velocity *= 1.03f;

			if (!primsCreated) {
				primsCreated = true;
				SpiritMod.primitives.CreateTrail(new RipperPrimTrail(projectile));
			}
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(new LegacySoundStyle(SoundID.NPCHit, 8).WithPitchVariance(0.2f).WithVolume(0.3f), projectile.Center);
			for (int i = 0; i < 20; i++) {
				Dust.NewDustPerfect(projectile.Center, 5, Main.rand.NextFloat(0.25f, 0.5f) * projectile.velocity.RotatedBy(3.14f + Main.rand.NextFloat(-0.4f, 0.4f)));
			}
		}
	}
}
