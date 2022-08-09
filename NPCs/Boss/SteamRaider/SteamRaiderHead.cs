using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Sets.StarplateDrops;
using SpiritMod.Items.Consumable;
using SpiritMod.Items.Placeable.MusicBox;
using SpiritMod.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Buffs;
using SpiritMod.Projectiles;
using Terraria.GameContent.Bestiary;
using SpiritMod.Utilities.PhaseIndicatorCompat;

namespace SpiritMod.NPCs.Boss.SteamRaider
{
	[PhaseIndicator(null, 0.2f)]
	[AutoloadBossHead]
	public class SteamRaiderHead : ModNPC, IBCRegistrable
	{
		bool Charge
		{
			get => NPC.ai[2] == 1;
			set => NPC.ai[2] = value ? 1 : 0;
		}

		int timer = 20;
		public bool flies = true;
		public bool directional = false;
		public float speed = 10.5f;
		public float turnSpeed = 0.19f;
		public bool tail = false;
		public int minLength = 46;
		public int midLength = 48;
		public int maxLength = 49;
		public bool spawnedProbes = false;
		int chargetimer;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starplate Voyager");
			Main.npcFrameCount[NPC.type] = 1; //new //new
		}

		public override void SetDefaults()
		{
			NPC.damage = 25; //150
			NPC.npcSlots = 20f;
			NPC.width = 64; //324
			NPC.height = 56; //216
			NPC.defense = 0;
			NPC.lifeMax = 8000; //250000 //new
			AnimationType = 10; //new
			NPC.knockBackResist = 0f;
			NPC.boss = true;
			NPC.value = 40000;
			NPC.alpha = 255;
			NPC.behindTiles = true;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.netAlways = true;
			Music = MusicLoader.GetMusicSlot(Mod,"Sounds/Music/Starplate");

			for (int k = 0; k < NPC.buffImmune.Length; k++)
				NPC.buffImmune[k] = true;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
				new FlavorTextBestiaryInfoElement("A strange automaton of unknown origin, designed for mining a precious metal from the stars. It utilizes the untapped energy found within the ore to power itself and perpetuate an endless search."),
			});
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(chargetimer);
			writer.Write(NPC.localAI[0]);
			writer.Write(NPC.localAI[1]);
			writer.Write(NPC.localAI[2]);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			chargetimer = reader.ReadInt32();
			NPC.localAI[0] = reader.ReadSingle();
			NPC.localAI[1] = reader.ReadSingle();
			NPC.localAI[2] = reader.ReadSingle();
		}
		public override void AI()
		{
			NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];

			for(int i = 0; i < Main.maxPlayers; i++)
			{
				if (i < Main.player.Length && Main.player[i] != null && Main.player[i].active && !Main.player[i].dead)
				{
					Main.player[i].AddBuff(ModContent.BuffType<StarplateGravity>(), 60);
				}
			}

			/*if (crashY < npc.position.Y && charging)
			{
				 for (int i = 0; i < 40; i++)
                    {
                        int num = Dust.NewDust(npc.position, npc.width, npc.height, 226, 0f, -2f, 0, default, 1.1f);
                        Main.dust[num].noGravity = true;
                        Dust dust = Main.dust[num];
                        dust.position.X = dust.position.X + ((float)(Main.rand.Next(-30, 31) / 20) - 1.5f);
                        Dust expr_92_cp_0 = Main.dust[num];
                        expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-30, 31) / 20) - 1.5f);
                        if (Main.dust[num].position != npc.Center)
                        {
                            Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 20f;
                        }
                    }
					Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 89);
					Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 27);
					Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 14);
					charging = false;
			}
			if (charging)
			{
				npc.velocity.X = 0;
				npc.velocity.Y = 0;
				musicTimer++;
				if (musicTimer > 690) //change 720 if the music changes
				{
					npc.velocity.Y = 25;
					for (int i = 0; i < npc.height; i++)
					{
						if (Main.rand.Next(10) == 1)
						{
						int num = Dust.NewDust(new Vector2(npc.position.X + i, npc.Center.Y), 0, 0, 226);
						Main.dust[num].velocity.X = -10;
						Main.dust[num].velocity.Y = Main.rand.Next(-3,3);
						Main.dust[num].noGravity = true;
						
						num = Dust.NewDust(new Vector2(npc.position.X + i, npc.Center.Y), 0, 0, 226);
						Main.dust[num].velocity.X = 10;
						Main.dust[num].velocity.Y = Main.rand.Next(-3,3);
						Main.dust[num].noGravity = true;
						}
					}
				}
			}
            else
            {
                crashY = (int)player.position.Y + 48;
                npc.position.X = player.position.X;
                return;
            }*/

			if (!Charge)
			{
				timer++;
				if ((timer == 100 || timer == 400) && NPC.life > NPC.lifeMax * .2f)
				{
					if (Main.expertMode)
					{
						SoundEngine.PlaySound(SoundID.Item91, NPC.Center);
						Vector2 direction = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center) * 4f;

						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, direction.X, direction.Y, ModContent.ProjectileType<SteamBeam>(), NPCUtils.ToActualDamage(18, 1.5f), 0, Main.myPlayer, 0, 0);
					}
				}
				if (timer == 600)
				{
					if (NPC.life > NPC.lifeMax * .2f && NPC.life < NPC.lifeMax * .6f)
					{
						for (int i = 0; i < 2; i++)
							NPC.NewNPC(NPC.GetSource_FromAI(), (int)Main.player[NPC.target].Center.X + Main.rand.Next(-700, 700), (int)Main.player[NPC.target].Center.Y + Main.rand.Next(-700, 700), ModContent.NPCType<LaserBase>(), NPC.whoAmI);
					}
				}
			}

			if (timer == 700)
			{
				timer = 0;
				NPC.netUpdate = true;
			}

			chargetimer++;

			if (NPC.life >= NPC.lifeMax * .2f)
			{
				NPC.aiStyle = 6; //new
				AIType = -1;
				if (NPC.Distance(player.Center) > 1800 && chargetimer < 700 && player.active && !player.dead) //use its charge attack as an "enrage" when the player is too far away, to quickly gain distance
					chargetimer = 700;

				if (chargetimer == 700)
				{
					NPC.netUpdate = true;
					SoundEngine.PlaySound(SoundID.Roar, NPC.Center);

					CombatText.NewText(new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height), new Color(255, 155, 0, 100), "Target Engaged");
				}
				if (chargetimer >= 700 && chargetimer <= 900)
				{
					Charge = true;
				}
				else if (chargetimer > 900)
				{
					Charge = false;
					chargetimer = 0;
				}
				Lighting.AddLight((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f), 0f, 0.075f, 0.25f);
				if (NPC.ai[3] > 0f)
					NPC.realLife = (int)NPC.ai[3];

				if (NPC.target < 0 || NPC.target == 255 || player.dead)
					NPC.velocity.Y -= 700f;

				NPC.velocity.Length();
				if (NPC.alpha != 0)
				{
					for (int num934 = 0; num934 < 2; num934++)
					{
						int num935 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Electric, 0f, 0f, 100, default, 2f);
						Main.dust[num935].noGravity = true;
						Main.dust[num935].noLight = true;
					}
				}
				NPC.alpha -= 12;
				if (NPC.alpha < 0)
					NPC.alpha = 0;

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					if (!tail && NPC.ai[0] == 0f)
					{

						int after = NPC.whoAmI;
						for (int num36 = 0; num36 < maxLength; num36++)
						{
							int before;
							if (num36 >= 0 && num36 < minLength)
								before = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X + (NPC.width / 2), (int)NPC.position.Y + (NPC.height / 2), ModContent.NPCType<SteamRaiderBody>(), NPC.whoAmI);
							else if (num36 >= minLength && num36 < midLength)
								before = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X + (NPC.width / 2), (int)NPC.position.Y + (NPC.height / 2), ModContent.NPCType<SteamRaiderBody2>(), NPC.whoAmI);
							else
								before = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X + (NPC.width / 2), (int)NPC.position.Y + (NPC.height / 2), ModContent.NPCType<SteamRaiderTail>(), NPC.whoAmI);

							Main.npc[before].realLife = NPC.whoAmI;
							Main.npc[before].ai[2] = NPC.whoAmI;
							Main.npc[before].ai[1] = after;
							Main.npc[after].ai[0] = before;
							NPC.netUpdate = true;
							after = before;
						}
						tail = true;
					}
					if (!NPC.active && Main.netMode == NetmodeID.Server)
						NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, NPC.whoAmI, -1f, 0f, 0f, 0, 0, 0);
				}

				int num180 = (int)(NPC.position.X / 16f) - 1;
				int num181 = (int)((NPC.position.X + (float)NPC.width) / 16f) + 2;
				int num182 = (int)(NPC.position.Y / 16f) - 1;
				int num183 = (int)((NPC.position.Y + (float)NPC.height) / 16f) + 2;
				if (num180 < 0)
					num180 = 0;

				if (num181 > Main.maxTilesX)
					num181 = Main.maxTilesX;

				if (num182 < 0)
					num182 = 0;

				if (num183 > Main.maxTilesY)
					num183 = Main.maxTilesY;

				bool flag94 = flies;
				NPC.localAI[1] = 0f;
				if (directional)
				{
					if (NPC.velocity.X < 0f)
						NPC.spriteDirection = 1;

					else if (NPC.velocity.X > 0f)
						NPC.spriteDirection = -1;
				}


				if (player.dead || !player.active)
				{

					NPC.TargetClosest(false);
					NPC.velocity.Y--;

					if (NPC.position.Y < 0)
						NPC.active = false;
				}
				Vector2 value = NPC.Center + (NPC.rotation - 1.57079637f).ToRotationVector2() * 8f;
				Vector2 value2 = NPC.rotation.ToRotationVector2() * 16f;
				Dust dust = Main.dust[Dust.NewDust(value + value2, 0, 0, DustID.Electric, NPC.velocity.X, NPC.velocity.Y, 100, Color.Transparent, 0.5f + Main.rand.NextFloat() * 1.5f)];
				dust.noGravity = true;
				dust.noLight = true;
				dust.position -= new Vector2(2f); //4
				dust.fadeIn = 1f;
				dust.scale *= .6f;
				dust.velocity = Vector2.Zero;
				dust = Main.dust[Dust.NewDust(value - value2, 0, 0, DustID.Electric, NPC.velocity.X, NPC.velocity.Y, 100, Color.Transparent, 0.5f + Main.rand.NextFloat() * 1.5f)];
				dust.noGravity = true;
				dust.noLight = true;
				dust.position -= new Vector2(2f); //4
				dust.fadeIn = 1f;
				dust.velocity = Vector2.Zero;
				float num188 = speed;
				float num189 = turnSpeed;
				Vector2 vector18 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
				float num191 = player.position.X + (player.width / 2);
				float num192 = player.position.Y + (player.height / 2);
				int num42 = -1;
				int num43 = (int)(player.Center.X / 16f);
				int num44 = (int)(player.Center.Y / 16f);
				for (int num45 = num43 - 2; num45 <= num43 + 2; num45++)
				{
					for (int num46 = num44; num46 <= num44 + 15; num46++)
					{
						if (WorldGen.SolidTile2(num45, num46))
						{
							num42 = num46;
							break;
						}
					}
					if (num42 > 0)
						break;
				}
				if (num42 > 0)
				{
					NPC.defense = 15;
					num42 *= 16;
					float num47 = (float)(num42 - 560); //was 800
					if (player.position.Y > num47 && !Charge)
					{
						num192 = num47;
						if (Math.Abs(NPC.Center.X - player.Center.X) < 170f) //was 500
						{
							if (NPC.velocity.X > 0f)
								num191 = player.Center.X + 170f; //was 600
							else
								num191 = player.Center.X - 170f; //was 600
						}
					}
					else if (Charge && player.position.Y < num47)
					{
						num192 = num47;
						if (Math.Abs(NPC.Center.X - player.Center.X) < 450f) //was 500
						{
							if (NPC.velocity.X > 0f)
								num191 = player.Center.X + 450f; //was 600
							else
								num191 = player.Center.X - 450f; //was 600
						}
					}
				}
				else
				{
					NPC.defense = 0;
					num188 = Main.expertMode ? 12.5f : 10f; //added 2.5
					num189 = Main.expertMode ? 0.25f : 0.2f; //added 0.05
				}
				float num48 = num188 * 1.23f;
				float num49 = num188 * 0.7f;
				float num50 = NPC.velocity.Length();
				if (num50 > 0f)
				{
					if (num50 > num48)
					{
						NPC.velocity.Normalize();
						NPC.velocity *= num48;
					}
					else if (num50 < num49)
					{
						NPC.velocity.Normalize();
						NPC.velocity *= num49;
					}
				}
				if (num42 > 0)
				{
					for (int num51 = 0; num51 < 200; num51++)
					{
						if (Main.npc[num51].active && Main.npc[num51].type == NPC.type && num51 != NPC.whoAmI)
						{
							Vector2 vector3 = Main.npc[num51].Center - NPC.Center;
							if (vector3.Length() < 200f)
							{
								vector3.Normalize();
								vector3 *= 1000f;
								num191 -= vector3.X;
								num192 -= vector3.Y;
							}
						}
					}
				}
				else
				{
					for (int num52 = 0; num52 < 200; num52++)
					{
						if (Main.npc[num52].active && Main.npc[num52].type == NPC.type && num52 != NPC.whoAmI)
						{
							Vector2 vector4 = Main.npc[num52].Center - NPC.Center;
							if (vector4.Length() < 60f)
							{
								vector4.Normalize();
								vector4 *= 200f;
								num191 -= vector4.X;
								num192 -= vector4.Y;
							}
						}
					}
				}
				num191 = ((int)(num191 / 16f) * 16);
				num192 = ((int)(num192 / 16f) * 16);
				vector18.X = ((int)(vector18.X / 16f) * 16);
				vector18.Y = ((int)(vector18.Y / 16f) * 16);
				num191 -= vector18.X;
				num192 -= vector18.Y;
				float num193 = (float)System.Math.Sqrt((num191 * num191 + num192 * num192));
				if (NPC.ai[1] > 0f && NPC.ai[1] < Main.npc.Length)
				{
					try
					{
						vector18 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
						num191 = Main.npc[(int)NPC.ai[1]].position.X + (Main.npc[(int)NPC.ai[1]].width / 2) - vector18.X;
						num192 = Main.npc[(int)NPC.ai[1]].position.Y + (Main.npc[(int)NPC.ai[1]].height / 2) - vector18.Y;
					}
					catch
					{
					}
					NPC.rotation = (float)Math.Atan2(num192, num191) + 1.57f;
					num193 = (float)Math.Sqrt(num191 * num191 + num192 * num192);
					int num194 = NPC.width;
					num193 = (num193 - num194) / num193;
					num191 *= num193;
					num192 *= num193;
					NPC.velocity = Vector2.Zero;
					NPC.position.X = NPC.position.X + num191;
					NPC.position.Y = NPC.position.Y + num192;
					if (directional)
					{
						if (num191 < 0f)
							NPC.spriteDirection = 1;

						if (num191 > 0f)
							NPC.spriteDirection = -1;
					}
				}
				else
				{
					if (Charge && player.active && !player.dead)
					{
						timer = 0;
						if (chargetimer == 700) //use localai[0] to store the rotation of the charge, and set it to the npc's velocity.torotation at the very first tick of the attack
							NPC.localAI[0] = NPC.velocity.ToRotation();
						float toplayer = (chargetimer > 750) ? NPC.AngleTo(player.Center) : NPC.AngleFrom(player.Center); //for first 5/6 second of attack, move away from the player, then move towards them
						toplayer = MathHelper.WrapAngle(toplayer);

						float lerpspeed = (chargetimer > 770) ? 0.0275f : 0.1f; //turn faster for first part of attack, then have really weak turning speed
						float length = Math.Min((chargetimer - 700) / 5, 24) + 5; //increase in speed as attack goes on, with a cap
						length *= (chargetimer > 750) ? MathHelper.Clamp(1, NPC.Distance(player.Center) / 1200, 2) : 0.8f; //when charging directly at player, also factor in distance
						NPC.localAI[0] = Utils.AngleLerp(NPC.localAI[0], toplayer, lerpspeed); //change the angle of the boss's velocity over time
						NPC.velocity = Vector2.UnitX.RotatedBy(NPC.localAI[0]) * length;
						return; //dont proceed with the rest of its ai while charging, to prevent unwanted movement
					}
					num193 = (float)Math.Sqrt((num191 * num191 + num192 * num192));
					float num196 = Math.Abs(num191);
					float num197 = Math.Abs(num192);
					float num198 = num188 / num193;
					num191 *= num198;
					num192 *= num198;
					bool flag21 = false;
					if (!flag21)
					{
						if ((NPC.velocity.X > 0f && num191 > 0f) || (NPC.velocity.X < 0f && num191 < 0f) || (NPC.velocity.Y > 0f && num192 > 0f) || (NPC.velocity.Y < 0f && num192 < 0f))
						{
							if (NPC.velocity.X < num191)
								NPC.velocity.X = NPC.velocity.X + num189;
							else
							{
								if (NPC.velocity.X > num191)
									NPC.velocity.X = NPC.velocity.X - num189;
							}

							if (NPC.velocity.Y < num192)
								NPC.velocity.Y = NPC.velocity.Y + num189;
							else
							{
								if (NPC.velocity.Y > num192)
								{
									NPC.velocity.Y = NPC.velocity.Y - num189;
								}
							}

							if (Math.Abs(num192) < num188 * 0.2 && ((NPC.velocity.X > 0f && num191 < 0f) || (NPC.velocity.X < 0f && num191 > 0f)))
							{
								if (NPC.velocity.Y > 0f)
									NPC.velocity.Y = NPC.velocity.Y + num189 * 2f;
								else
									NPC.velocity.Y = NPC.velocity.Y - num189 * 2f;
							}
							if (Math.Abs(num191) < num188 * 0.2 && ((NPC.velocity.Y > 0f && num192 < 0f) || (NPC.velocity.Y < 0f && num192 > 0f)))
							{
								if (NPC.velocity.X > 0f)
									NPC.velocity.X = NPC.velocity.X + num189 * 2f; //changed from 2
								else
									NPC.velocity.X = NPC.velocity.X - num189 * 2f; //changed from 2
							}
						}
						else
						{
							if (num196 > num197)
							{
								if (NPC.velocity.X < num191)
									NPC.velocity.X = NPC.velocity.X + num189 * 1.1f; //changed from 1.1
								else if (NPC.velocity.X > num191)
									NPC.velocity.X = NPC.velocity.X - num189 * 1.1f; //changed from 1.1

								if ((Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y)) < num188 * 0.5)
								{
									if (NPC.velocity.Y > 0f)
										NPC.velocity.Y = NPC.velocity.Y + num189;
									else
										NPC.velocity.Y = NPC.velocity.Y - num189;
								}
							}
							else
							{
								if (NPC.velocity.Y < num192)
									NPC.velocity.Y = NPC.velocity.Y + num189 * 1.1f;
								else if (NPC.velocity.Y > num192)
									NPC.velocity.Y = NPC.velocity.Y - num189 * 1.1f;
								if ((Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y)) < num188 * 0.5)
								{
									if (NPC.velocity.X > 0f)
										NPC.velocity.X = NPC.velocity.X + num189;
									else
										NPC.velocity.X = NPC.velocity.X - num189;
								}
							}
						}
					}
				}
			}
			#region Phase2
			else
			{
				if (NPC.target < 0 || NPC.target == 255 || player.dead)
				{
					NPC.active = false;
					NPC.netUpdate = true;
					timer = 0;
				}
				if (NPC.localAI[2] == 0)
				{
					CombatText.NewText(new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height), new Color(255, 155, 0, 100),
	"Instability Detected");
					NPC.localAI[2]++;
					NPC.netUpdate = true;
				}
				alphaCounter += 0.08f;
				NPC.netUpdate = true;
				Player nearby = Main.LocalPlayer;
				if (Main.expertMode)
				{
					int distance = (int)Vector2.Distance(NPC.Center, nearby.Center);
					if (distance <= 1000)
					{
						nearby.GetSpiritPlayer().starplateGlitchEffect = true;
						nearby.GetSpiritPlayer().starplateGlitchIntensity = (float)MathHelper.Clamp((1200 / NPC.life) * .002f, 0f, .025f);
					}
					else
					{
						nearby.GetSpiritPlayer().starplateGlitchIntensity = 0f;
						nearby.GetSpiritPlayer().starplateGlitchEffect = false;
					}
				}

				NPC.aiStyle = -1;
				atkCounter++;
				//  shootCounter++;
				if (atkCounter % 2000 > 0 && atkCounter % 2000 < 1000) //if it's in the teleport phase
				{
					int dust1 = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Electric);

					Main.dust[dust1].velocity *= -1f;
					Main.dust[dust1].noGravity = true;
					Main.dust[dust1].scale *= .8f;
					Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
					vector2_1.Normalize();
					Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
					Main.dust[dust1].velocity = vector2_2;
					vector2_2.Normalize();
					Vector2 vector2_3 = vector2_2 * 34f;
					Main.dust[dust1].position = (NPC.Center) - vector2_3;
					NPC.velocity = Vector2.Zero; //sets his velocity to 0 in the teleport phase
					if (atkCounter % 200 == 1) //teleport and create laser boys
					{
						SoundEngine.PlaySound(SoundID.Item94, NPC.Center);
						for (int i = 0; i < 5; i++)
						{
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								int LaserBase = NPC.NewNPC(NPC.GetSource_FromAI(), (int)Main.player[NPC.target].Center.X + Main.rand.Next(-700, 700), (int)Main.player[NPC.target].Center.Y + Main.rand.Next(-700, 700), ModContent.NPCType<LaserBase>(), NPC.whoAmI);
								Main.npc[LaserBase].netUpdate = true;
							}
						}
						bool outOfBlock = false;
						while (!outOfBlock)
						{
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								int angle = Main.rand.Next(360);
								double anglex = Math.Sin(angle * (Math.PI / 180));
								double angley = Math.Cos(angle * (Math.PI / 180));
								NPC.position.X = player.Center.X + (int)(480 * anglex);
								NPC.position.Y = player.Center.Y + (int)(480 * angley);
							}
							if (!Main.tile[(int)(NPC.position.X / 16), (int)(NPC.position.Y / 16)].HasTile)
							{
								outOfBlock = true;
								NPC.netUpdate = true;
							}
						}
						CombatText.NewText(new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height), new Color(255, 155, 0, 100), "Initiating Laser Protocol");
					}
					direction9 = Main.player[NPC.target].Center - NPC.Center;
					direction9.Normalize();
					NPC.rotation = direction9.ToRotation() + 1.57f;
				}
				else if (atkCounter % 2000 >= 1000 && atkCounter % 2000 < 1500)
				{
					Charge = true;
					if (atkCounter % 250 == 0)
					{
						distAbove = 425;
						if (Main.rand.NextBool(2))
						{
							NPC.position.X = Main.player[NPC.target].Center.X - 500;
							NPC.position.Y = Main.player[NPC.target].Center.Y - distAbove;
							NPC.velocity.X = 4f;
						}
						else
						{
							NPC.position.X = Main.player[NPC.target].Center.X + 500;
							NPC.position.Y = Main.player[NPC.target].Center.Y - distAbove;
							NPC.velocity.X = -4f;
						}
						NPC.rotation = 3.14f;
						SoundEngine.PlaySound(SoundID.Item94, NPC.Center);
						if (Main.netMode != NetmodeID.MultiplayerClient)
							for (int i = 0; i < 3; i++)
								NPC.NewNPC(NPC.GetSource_FromAI(), (int)Main.player[NPC.target].Center.X + Main.rand.Next(-300, 300), (int)Main.player[NPC.target].Center.Y + Main.rand.Next(-300, 300), ModContent.NPCType<ArcadeProbe>(), NPC.whoAmI, 0f, 0f, 0f, 0f, 255);
						NPC.netUpdate = true;
					}
					//npc.position.Y = Main.player[npc.target].Center.Y -  distAbove;
					if (atkCounter % 20 == 0)
					{
						SoundEngine.PlaySound(SoundID.Item91, NPC.Center);
						for (int i = 0; i < 16; i++)
						{
							int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.GoldCoin);

							Main.dust[dust].velocity *= -1f;
							Main.dust[dust].noGravity = true;
							//        Main.dust[dust].scale *= 2f;
							Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
							vector2_1.Normalize();
							Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
							Main.dust[dust].velocity = vector2_2;
							vector2_2.Normalize();
							Vector2 vector2_3 = vector2_2 * 34f;
							Main.dust[dust].position = (NPC.Center) - vector2_3;
						}
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							int gLaser = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, 10), ModContent.ProjectileType<GlitchLaser>(), NPCUtils.ToActualDamage(25, 1.5f), 1, Main.myPlayer, 0, 0);
							NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, gLaser);
						}
					}
					//shootCounter = 180; //make sure he fires lasers immediately after ending this section
				}
				else if (atkCounter % 2000 >= 1500 && atkCounter % 2000 < 1850)
				{
					Charge = false;
					NPC.velocity = Vector2.Zero; //sets his velocity to 0 in the teleport phase
					if (atkCounter % 50 == 0)
					{
						bool outOfBlock = false;
						while (!outOfBlock)
						{
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								int angle = Main.rand.Next(360);
								double anglex = Math.Sin(angle * (Math.PI / 180));
								double angley = Math.Cos(angle * (Math.PI / 180));
								NPC.position.X = player.Center.X + (int)(480 * anglex);
								NPC.position.Y = player.Center.Y + (int)(480 * angley);
							}
							if (!Main.tile[(int)(NPC.position.X / 16), (int)(NPC.position.Y / 16)].HasTile)
							{
								outOfBlock = true;
								NPC.netUpdate = true;
							}
						}
						direction9 = Main.player[NPC.target].Center - NPC.Center;
						direction9.Normalize();
						NPC.rotation = direction9.ToRotation() + 1.57f;
					}
					if (atkCounter % 50 < 30)
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, (float)direction9.X * 30, (float)direction9.Y * 30, ModContent.ProjectileType<StarLaserTrace>(), NPCUtils.ToActualDamage(27, 1.5f), 1, Main.myPlayer);
					if (atkCounter % 50 == 30) //change to frame related later
					{
						SoundEngine.PlaySound(SoundID.NPCHit53, NPC.Center);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, (float)direction9.X * 40, (float)direction9.Y * 40, ModContent.ProjectileType<StarLaser>(), NPCUtils.ToActualDamage(55, 1.5f), 1, Main.myPlayer);
					}
					if (atkCounter % 50 == 49 && Main.expertMode)
						NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<SuicideLaser>(), NPC.whoAmI);
				}
			}
			#endregion
		}
		float alphaCounter;
		int atkCounter = 0;
		int distAbove = 500;
		Vector2 direction9 = Vector2.Zero;

		//int shootCounter = 150;
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (NPC.life <= 1200)
			{
				Vector2 drawOrigin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width * 0.5f, (NPC.height / Main.npcFrameCount[NPC.type]) * 0.5f);
				{
					float sineAdd = (float)Math.Sin(alphaCounter * 2) + 3;
					Vector2 drawPos1 = NPC.Center - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY);
					Main.spriteBatch.Draw(TextureAssets.Extra[49].Value, (NPC.Center - Main.screenPosition), null, new Color((int)(7.5f * sineAdd), (int)(16.5f * sineAdd), (int)(18f * sineAdd), 0), 0f, new Vector2(50, 50), 0.25f * (sineAdd + .65f), SpriteEffects.None, 0f);
				}
			}
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame,
							 drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => NPC.life >= NPC.lifeMax * .2f;
		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (NPC.life < NPC.lifeMax * .2f)
				damage = (int)(damage * 0.8f);
		}
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (Charge)
			{
				Color color1 = Lighting.GetColor((int)((double)NPC.position.X + (double)NPC.width * 0.5) / 16, (int)(((double)NPC.position.Y + (double)NPC.height * 0.5) / 16.0));
				Vector2 drawOrigin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width * 0.5f, NPC.height * 0.5f);
				int r1 = (int)color1.R;
				drawOrigin.Y += 30f;
				drawOrigin.Y += 8f;
				--drawOrigin.X;
				Vector2 position1 = NPC.Bottom - Main.screenPosition;
				Texture2D texture2D2 = TextureAssets.GlowMask[239].Value;
				float num11 = (float)((double)Main.GlobalTimeWrappedHourly % 1.0 / 1.0);
				float num12 = num11;
				if ((double)num12 > 0.5)
					num12 = 1f - num11;
				if ((double)num12 < 0.0)
					num12 = 0.0f;
				float num13 = (float)(((double)num11 + 0.5) % 1.0);
				float num14 = num13;
				if ((double)num14 > 0.5)
					num14 = 1f - num13;
				if ((double)num14 < 0.0)
					num14 = 0.0f;
				Rectangle r2 = texture2D2.Frame(1, 1, 0, 0);
				drawOrigin = r2.Size() / 2f;
				Vector2 position3 = position1 + new Vector2(0.0f, -24f);
				Color color3 = new Color(255, 138, 36) * 1.6f;
				Main.spriteBatch.Draw(texture2D2, position3, r2, color3, NPC.rotation, drawOrigin, NPC.scale * 0.5f, SpriteEffects.FlipHorizontally, 0.0f);
				float num15 = 1f + num11 * 0.75f;
				Main.spriteBatch.Draw(texture2D2, position3, r2, color3 * num12, NPC.rotation, drawOrigin, NPC.scale * 0.5f * num15, SpriteEffects.FlipHorizontally, 0.0f);
				float num16 = 1f + num13 * 0.75f;
				Main.spriteBatch.Draw(texture2D2, position3, r2, color3 * num14, NPC.rotation, drawOrigin, NPC.scale * 0.5f * num16, SpriteEffects.FlipHorizontally, 0.0f);
			}
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.Assets.Request<Texture2D>("NPCs/Boss/SteamRaider/SteamRaiderHead_Glow").Value, screenPos);

		}
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 5; k++)
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, hitDirection, -1f, 0, default, 1f);
		}

		public override bool PreKill()
		{
			if (!MyWorld.downedRaider)
				Main.NewText("The Astralite in the Asteroids hums with energy.", new Color(61, 255, 142));

			MyWorld.downedRaider = true;
			if (Main.netMode != NetmodeID.SinglePlayer)
				NetMessage.SendData(MessageID.WorldData);

			NPC.PlayDeathSound("StarplateDeathSound");
			NPC.NewNPC(NPC.GetSource_Death(), (int)NPC.position.X + NPC.width - 20, (int)NPC.position.Y + NPC.height, ModContent.NPCType<SteamRaiderHeadDeath>(), NPC.whoAmI);
			return true;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale) => NPC.lifeMax = (int)(NPC.lifeMax * 0.6f * bossLifeScale);

		public void RegisterToChecklist(out BossChecklistDataHandler.EntryType entryType, out float progression,
			out string name, out Func<bool> downedCondition, ref BossChecklistDataHandler.BCIDData identificationData,
			ref string spawnInfo, ref string despawnMessage, ref string texture, ref string headTextureOverride,
			ref Func<bool> isAvailable)
		{
			entryType = BossChecklistDataHandler.EntryType.Boss;
			progression = 5.9f;
			name = "Starplate Voyager";
			downedCondition = () => MyWorld.downedRaider;
			identificationData = new BossChecklistDataHandler.BCIDData(
				new List<int> {
					ModContent.NPCType<SteamRaiderHead>()
				},
				new List<int> {
					ModContent.ItemType<StarWormSummon>()
				},
				new List<int> {
					ModContent.ItemType<Trophy3>(),
					ModContent.ItemType<StarplateMask>(),
					ModContent.ItemType<StarplateBox>()
				},
				new List<int> {
					ModContent.ItemType<StarMap>(),
					ModContent.ItemType<CosmiliteShard>(),
					ItemID.LesserHealingPotion
				});
			spawnInfo =
				$"Use a [i:{ModContent.ItemType<StarWormSummon>()}] at an Astralite Beacon, located in the Asteroids, at nighttime.";
			texture = "SpiritMod/Textures/BossChecklist/StarplateTexture";
			headTextureOverride = "SpiritMod/NPCs/Boss/SteamRaider/SteamRaiderHead_Head_Boss";
		}
	}
}