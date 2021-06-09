using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Prim;
using SpiritMod.VerletChains;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.BloodGazer
{
	public class BloodGazerEye : ModNPC
	{
		private Chain chain;

		private NPC Parent => Main.npc[(int)npc.ai[0]];

		private Player Target => Main.player[Parent.target];

		private bool Active => Parent.active && Parent.type == ModContent.NPCType<BloodGazer>() &&
			((Parent.ai[0] != (float)BloodGazerAiStates.Passive && Parent.ai[0] != (float)BloodGazerAiStates.Despawn && Parent.ai[0] != (float)BloodGazerAiStates.Phase2Transition) ||
			detatched);
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
			npc.netAlways = true;
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
			if(npc.localAI[0] == 0){
				InitializeChain(Parent.Center);
				npc.localAI[0]++;
			}

			if (!Active)
			{
				NPCLoot();
				npc.active = false;
				return;
			}
			npc.knockBackResist = 0.5f;

			npc.realLife = (int)npc.ai[0];

			float ChainLength = (chain.Texture.Height * chain.Segments.Count);
			if (detatched)
			{
				chain.FirstVertex.Static = false;
				contactdamage = true;
				KillTimer++;
				if (KillTimer > 270)
				{
					NPCLoot();
					npc.active = false;
					return;
				}

				npc.velocity = Vector2.Lerp(npc.velocity, npc.DirectionTo(Target.Center) * 22, 0.015f);
			}

			else
			{
				float pullbackstrength = 0.25f;
				switch ((BloodGazerAiStates)Parent.ai[0])
				{
					case (BloodGazerAiStates.Hostile):
						npc.velocity = Vector2.Lerp(npc.velocity, npc.DirectionTo(Target.Center) * 6, 0.02f * (1 / npc.ai[1]));
						break;

					case (BloodGazerAiStates.EyeSwing):
						npc.knockBackResist = 0f;
						npc.position += Parent.velocity;
						pullbackstrength = 2f;
						npc.knockBackResist = 0f;
						float chandistratio = 0.3f;
						Vector2 targetPos = Parent.Center + Parent.DirectionTo(Target.Center).RotatedBy(MathHelper.PiOver4 * ((npc.ai[1] % 2 == 0) ? -1 : 1)) * ChainLength * chandistratio;
						//entire attack takes 60 ticks, offset by eye number so only 1 swing happens at a time, and offset by a static 20 ticks so the first swing takes longer
						int SwingTime = 60;
						int StaticDelay = 20;
						int starthomingtime = (int)(SwingTime * (EyeNumber - 2f)) + StaticDelay;
						int startswingtime = (int)(SwingTime * (EyeNumber - 0.25f)) + StaticDelay;
						int startslowdowntime = (int)(SwingTime * (EyeNumber + 0.05f)) + StaticDelay;

						if (AiTimer >= starthomingtime && AiTimer < startswingtime)  //homing in on target position
							npc.velocity = Vector2.Lerp(npc.velocity, npc.DirectionTo(targetPos) * MathHelper.Clamp(npc.Distance(targetPos) / 20, 8, 14), 0.07f);

						if (AiTimer == startswingtime) //swing
						{
							npc.velocity = 50 * (npc.Distance(Parent.Center) / ChainLength) * Parent.DirectionTo(Target.Center).RotatedBy(MathHelper.PiOver4 * ((npc.ai[1] % 2 == 0) ? 1 : -1));
							contactdamage = true;
							(Parent.modNPC as BloodGazer).trailing = true;
							Parent.velocity = Parent.DirectionTo(Target.Center) * 15 * npc.Distance(Parent.Center) / (ChainLength * chandistratio);
							Parent.netUpdate = true;
							npc.netUpdate = true;
							if (Main.netMode != NetmodeID.Server)
								Main.PlaySound(SoundLoader.customSoundType, (int)Parent.Center.X, (int)Parent.Center.Y, mod.GetSoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/VanillaBossRoar"), 0.65f, -0.25f);
						}

						if (AiTimer >= startswingtime && AiTimer <= startslowdowntime) //swing
						{
							ShootTimer++;
							int numshots = 5;
							if (ShootTimer >= (startslowdowntime - startswingtime) / numshots)
							{
								ShootTimer = 0;
								if (Main.netMode != NetmodeID.Server)
									Main.PlaySound(SoundID.Item, npc.Center, 95);

								if (Main.netMode != NetmodeID.MultiplayerClient)
									Projectile.NewProjectileDirect(npc.Center, npc.DirectionFrom(Parent.Center) * 5, ModContent.ProjectileType<BloodGazerEyeShot>(), NPCUtils.ToActualDamage(40, 1.5f), 1, Main.myPlayer).netUpdate = true;

								npc.velocity += npc.DirectionTo(Parent.Center);
								npc.netUpdate = true;
							}
						}

						if (AiTimer == startslowdowntime)
						{
							contactdamage = false;
							(Parent.modNPC as BloodGazer).trailing = false;
							Parent.netUpdate = true;
							npc.netUpdate = true;
						}

						if (AiTimer > startslowdowntime)  //slow down after a bit
							npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Zero, 0.03f);

						break;

					case (BloodGazerAiStates.EyeSpin):
						npc.knockBackResist = 0f;
						float prespintime = 60; //time spent preparing for the spin
						if (AiTimer <= prespintime)
							npc.velocity = Vector2.Lerp(npc.velocity, npc.DirectionTo(Parent.Center) * MathHelper.Clamp(npc.Distance(Parent.Center) / 20, 4, 8), 0.07f);

						else
						{
							float degreespertick = 5f;
							Vector2 position = Vector2.UnitX.RotatedBy(MathHelper.ToRadians((AiTimer * degreespertick) - prespintime) + (EyeNumber * MathHelper.TwoPi / 3));
							position *= MathHelper.Min((AiTimer - prespintime) * 2, ChainLength * 0.175f);
							npc.position = position + Parent.Center;
							ShootTimer++;
							if (ShootTimer > 30)
							{
								npc.netUpdate = true;
								ShootTimer = 0;
								if (Main.netMode != NetmodeID.Server)
									Main.PlaySound(SoundID.Item, npc.Center, 95);

								if (Main.netMode != NetmodeID.MultiplayerClient)
									Projectile.NewProjectileDirect(npc.Center, npc.DirectionFrom(Parent.Center) * 2f, ModContent.ProjectileType<BloodGazerEyeShotWavy>(), NPCUtils.ToActualDamage(40, 1.5f), 1, Main.myPlayer, Main.rand.NextBool() ? -1 : 1).netUpdate = true;
							}
						}
						break;

					case (BloodGazerAiStates.DetatchingEyes):
						npc.knockBackResist = 0.5f;
						pullbackstrength = 0.75f;
						if (AiTimer > 30 * EyeNumber)
						{
							detatched = true;
							Parent.velocity = npc.DirectionTo(Parent.Center) * 6;
							if (Main.netMode != NetmodeID.Server)
								Main.PlaySound(new LegacySoundStyle(SoundID.NPCKilled, 22).WithPitchVariance(0.2f).WithVolume(0.8f), Parent.Center);

							for (int i = 0; i < 25; i++)
							{
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

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(npc.knockBackResist);
			writer.Write(contactdamage);
			writer.Write(detatched);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			npc.knockBackResist = reader.ReadSingle();
			contactdamage = reader.ReadBoolean();
			detatched = reader.ReadBoolean();
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
			if (Main.netMode != NetmodeID.Server)
				Main.PlaySound(SoundID.NPCDeath22, npc.Center);

			Gore.NewGore(npc.position, npc.velocity / 2, mod.GetGoreSlot("Gores/Gazer/GazerEye"), 1f);
			foreach (var segment in chain.Segments)
				Gore.NewGore(segment.Vertex2.Position, npc.velocity / 2, mod.GetGoreSlot("Gores/Gazer/GazerChain"), 1f);
		}
	}
}