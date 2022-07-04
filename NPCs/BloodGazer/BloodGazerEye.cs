using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Prim;
using SpiritMod.VerletChains;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.BloodGazer
{
	public class BloodGazerEye : ModNPC
	{
		private Chain _chain;

		private NPC Parent => Main.npc[(int)NPC.ai[0]];

		private Player Target => Main.player[Parent.target];

		private bool Active => Parent.active && Parent.type == ModContent.NPCType<BloodGazer>() &&
			((Parent.ai[0] != (float)BloodGazerAiStates.Passive && Parent.ai[0] != (float)BloodGazerAiStates.Despawn && Parent.ai[0] != (float)BloodGazerAiStates.Phase2Transition) ||
			_detatched);
		public override void SetStaticDefaults() => DisplayName.SetDefault("Blood Gazer");

		public override void SetDefaults()
		{
			NPC.Size = new Vector2(26, 24);
			NPC.damage = 45;
			NPC.defense = 13;
			NPC.lifeMax = 800;
			NPC.knockBackResist = 0.5f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.dontCountMe = true;
			NPC.aiStyle = -1;
			NPC.HitSound = SoundID.NPCHit19;
			NPC.DeathSound = SoundID.NPCDeath22;
			NPC.netAlways = true;
			for (int i = 0; i < BuffLoader.BuffCount; i++)
			{
				NPC.buffImmune[i] = true;
			}
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale) => NPC.damage = (int)(NPC.damage * bossLifeScale * 0.66f);

		private const int chainSegments = 16;
		public void InitializeChain(Vector2 position) => _chain = new Chain(8, chainSegments, position, new ChainPhysics(0.9f, 0.5f, 0f));

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;

		public override bool CheckActive() => !Active;

		public override bool CheckDead() => !Active;

		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) => damage = 0;

		public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit) => damage = 0;

		public override bool PreKill() => false;

		ref float AiTimer => ref Parent.ai[1];

		ref float EyeNumber => ref NPC.ai[1];

		ref float ShootTimer => ref NPC.ai[2];

		ref float KillTimer => ref NPC.ai[3];

		private bool _contactDamageAllowed = false;

		private bool _detatched = false;

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => _contactDamageAllowed;

		public override void AI()
		{
			if(NPC.localAI[0] == 0 && Main.netMode != NetmodeID.Server){
				InitializeChain(Parent.Center);
				NPC.localAI[0]++;
			}

			if (!Active)
			{
				//this.NPCLoot(); Not sure what this did?
				NPC.active = false;
				return;
			}
			NPC.knockBackResist = 0.5f;

			NPC.realLife = (int)NPC.ai[0];

			float ChainLength = 32 * chainSegments;
			if (_detatched)
			{
				if(Main.netMode != NetmodeID.Server)
					_chain.FirstVertex.Static = false;

				_contactDamageAllowed = true;
				KillTimer++;
				if (KillTimer > 270)
				{
					//NPCLoot(); Or this?
					NPC.active = false;
					return;
				}

				NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(Target.Center) * 22, 0.015f);
			}

			else
			{
				float pullbackstrength = 0.25f;
				switch ((BloodGazerAiStates)Parent.ai[0])
				{
					case (BloodGazerAiStates.Hostile):
						NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(Target.Center) * 6, 0.02f * (1 / NPC.ai[1]));
						break;

					case (BloodGazerAiStates.EyeSwing):
						NPC.knockBackResist = 0f;
						NPC.position += Parent.velocity;
						pullbackstrength = 2f;
						NPC.knockBackResist = 0f;
						float chandistratio = 0.3f;
						Vector2 targetPos = Parent.Center + Parent.DirectionTo(Target.Center).RotatedBy(MathHelper.PiOver4 * ((NPC.ai[1] % 2 == 0) ? -1 : 1)) * ChainLength * chandistratio;
						//entire attack takes 60 ticks, offset by eye number so only 1 swing happens at a time, and offset by a static 20 ticks so the first swing takes longer
						int SwingTime = 60;
						int StaticDelay = 20;
						int starthomingtime = (int)(SwingTime * (EyeNumber - 2f)) + StaticDelay;
						int startswingtime = (int)(SwingTime * (EyeNumber - 0.25f)) + StaticDelay;
						int startslowdowntime = (int)(SwingTime * (EyeNumber + 0.05f)) + StaticDelay;

						if (AiTimer >= starthomingtime && AiTimer < startswingtime)  //homing in on target position
							NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(targetPos) * MathHelper.Clamp(NPC.Distance(targetPos) / 20, 8, 14), 0.07f);

						if (AiTimer == startswingtime) //swing
						{
							NPC.velocity = 50 * (NPC.Distance(Parent.Center) / ChainLength) * Parent.DirectionTo(Target.Center).RotatedBy(MathHelper.PiOver4 * ((NPC.ai[1] % 2 == 0) ? 1 : -1));
							_contactDamageAllowed = true;
							(Parent.ModNPC as BloodGazer).trailing = true;
							Parent.velocity = Parent.DirectionTo(Target.Center) * 15 * NPC.Distance(Parent.Center) / (ChainLength * chandistratio);
							Parent.netUpdate = true;
							NPC.netUpdate = true;
							if (Main.netMode != NetmodeID.Server)
								SoundEngine.PlaySound(new SoundStyle("SpiritModSounds/VanillaBossRoar") with { Volume = 0.65f }, Parent.Center);
						}

						if (AiTimer >= startswingtime && AiTimer <= startslowdowntime) //swing
						{
							ShootTimer++;
							int numshots = 5;
							if (ShootTimer >= (startslowdowntime - startswingtime) / numshots)
							{
								ShootTimer = 0;
								if (Main.netMode != NetmodeID.Server)
									SoundEngine.PlaySound(SoundID.Item95, NPC.Center);

								if (Main.netMode != NetmodeID.MultiplayerClient)
								{
									Projectile p = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionFrom(Parent.Center) * 5, ModContent.ProjectileType<BloodGazerEyeShot>(), NPCUtils.ToActualDamage(40, 1.5f), 1, Main.myPlayer);

									if (Main.netMode != NetmodeID.SinglePlayer)
										NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, p.whoAmI);
								}

								NPC.velocity += NPC.DirectionTo(Parent.Center);
								NPC.netUpdate = true;
							}
						}

						if (AiTimer == startslowdowntime)
						{
							_contactDamageAllowed = false;
							(Parent.ModNPC as BloodGazer).trailing = false;
							Parent.netUpdate = true;
							NPC.netUpdate = true;
						}

						if (AiTimer > startslowdowntime)  //slow down after a bit
							NPC.velocity = Vector2.Lerp(NPC.velocity, Vector2.Zero, 0.03f);

						break;

					case (BloodGazerAiStates.EyeSpin):
						NPC.knockBackResist = 0f;
						float prespintime = 60; //time spent preparing for the spin
						if (AiTimer <= prespintime)
							NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(Parent.Center) * MathHelper.Clamp(NPC.Distance(Parent.Center) / 20, 4, 8), 0.07f);

						else
						{
							float degreespertick = 5f;
							Vector2 position = Vector2.UnitX.RotatedBy(MathHelper.ToRadians((AiTimer * degreespertick) - prespintime) + (EyeNumber * MathHelper.TwoPi / 3));
							position *= MathHelper.Min((AiTimer - prespintime) * 2, ChainLength * 0.175f);
							NPC.position = position + Parent.Center;
							ShootTimer++;
							if (ShootTimer > 30)
							{
								NPC.netUpdate = true;
								ShootTimer = 0;
								if (Main.netMode != NetmodeID.Server)
									SoundEngine.PlaySound(SoundID.Item95, NPC.Center);

								if (Main.netMode != NetmodeID.MultiplayerClient)
								{
									Projectile p = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionFrom(Parent.Center) * 2f, ModContent.ProjectileType<BloodGazerEyeShotWavy>(), NPCUtils.ToActualDamage(40, 1.5f), 1, Main.myPlayer, Main.rand.NextBool() ? -1 : 1);

									if(Main.netMode != NetmodeID .SinglePlayer)
										NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, p.whoAmI);
								}
							}
						}
						break;

					case (BloodGazerAiStates.DetatchingEyes):
						NPC.knockBackResist = 0.5f;
						pullbackstrength = 0.75f;
						if (AiTimer > 30 * EyeNumber)
						{
							_detatched = true;
							Parent.velocity = NPC.DirectionTo(Parent.Center) * 6;
							if (Main.netMode != NetmodeID.Server)
								SoundEngine.PlaySound(SoundID.NPCDeath22 with { PitchVariance = 0.2f, Volume = 0.8f }, Parent.Center);

							for (int i = 0; i < 25; i++)
							{
								Dust dust = Dust.NewDustDirect(Parent.Center, 10, 10, ModContent.DustType<Dusts.Blood>(), 0f, -2f, 0, default, Main.rand.NextFloat(0.9f, 1.5f));
								dust.velocity = NPC.DirectionFrom(Parent.Center).RotatedByRandom(MathHelper.Pi / 14) * Main.rand.NextFloat(2f, 6f);
							}
						}
						break;
				}
				NPC.velocity += NPC.DirectionTo(Parent.Center) * pullbackstrength * NPC.Distance(Parent.Center) / ChainLength;
			}

			if(Main.netMode != NetmodeID.Server)
				_chain.Update(Parent.Center, NPC.Center);
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(NPC.knockBackResist);
			writer.Write(_contactDamageAllowed);
			writer.Write(_detatched);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			NPC.knockBackResist = reader.ReadSingle();
			_contactDamageAllowed = reader.ReadBoolean();
			_detatched = reader.ReadBoolean();
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (_chain == null)
				return false;

			Texture2D chaintex = ModContent.Request<Texture2D>(Texture + "_chain", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			_chain.Draw(spriteBatch, chaintex);
			Texture2D tex = TextureAssets.Npc[NPC.type].Value;

			spriteBatch.Draw(tex, _chain.EndPosition - new Vector2(chaintex.Height / 2, -chaintex.Width / 2).RotatedBy(_chain.EndRotation) - Main.screenPosition, tex.Bounds, drawColor, _chain.EndRotation, tex.Bounds.Size() / 2, NPC.scale, SpriteEffects.None, 0);
			return false;
		}

		public override void OnKill()
		{
			if (!Main.dedServ)
			{
				SoundEngine.PlaySound(SoundID.NPCDeath22, NPC.Center);

				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity / 2, Mod.Find<ModGore>("SpiritMod/Gores/Gazer/GazerEye").Type, 1f);
				foreach (var segment in _chain.Segments)
					Gore.NewGore(NPC.GetSource_Death(), segment.Vertex2.Position, NPC.velocity / 2, Mod.Find<ModGore>("SpiritMod/Gores/Gazer/GazerChain").Type, 1f);
			}
		}
	}
}