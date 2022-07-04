using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Sets.CryoliteSet;
using SpiritMod.Dusts;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Buffs.DoT;
using SpiritMod.Mechanics.QuestSystem;

namespace SpiritMod.NPCs.WinterbornHerald
{
	public class WinterbornMagic : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Winterborn Herald");
			Main.npcFrameCount[NPC.type] = 4;
		}

		public override void SetDefaults()
		{
			NPC.width = 30;
			NPC.height = 44;
			NPC.lifeMax = 400;
			NPC.defense = 6;
			NPC.damage = 0;
			NPC.HitSound = SoundID.NPCDeath15;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.buffImmune[BuffID.OnFire] = true;
			NPC.buffImmune[BuffID.Frostburn] = true;
			NPC.buffImmune[ModContent.BuffType<CryoCrush>()] = true;
			NPC.value = 289f;
			NPC.knockBackResist = 0.15f;
			NPC.noGravity = false;
			NPC.netAlways = true;
			NPC.chaseable = true;
			NPC.lavaImmune = true;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.WinterbornHeraldBanner>();
		}

		public override void OnKill()
		{
			if (QuestManager.GetQuest<Mechanics.QuestSystem.Quests.IceDeityQuest>().IsActive && Main.rand.NextBool(5)) //Item spawn not part of NPCLoot but it feels odd to put a quest drop there
				Item.NewItem(NPC.GetSource_Death("Quest"), NPC.Center, ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.IceDeityShard1>());
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon<CryoliteOre>(1, 3, 6);
			npcLoot.AddCommon<WintryCharmMage>(5);
		}

		public override bool PreAI()
		{
			bool expertMode = Main.expertMode;
			NPC.TargetClosest(true);
			NPC.velocity.X = NPC.velocity.X * 0.93f;
			if (NPC.velocity.X > -0.1F && NPC.velocity.X < 0.1F)
				NPC.velocity.X = 0;
			if (NPC.ai[0] == 0)
				NPC.ai[0] = 500f;

			if (NPC.ai[2] != 0 && NPC.ai[3] != 0)
			{
				// Teleport effects: away.
				SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
				for (int index1 = 0; index1 < 50; ++index1)
				{
					int newDust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Flare_Blue, 0.0f, 0.0f, 100, new Color(), 1.5f);
					Main.dust[newDust].velocity *= 3f;
					Main.dust[newDust].noGravity = true;
				}
				NPC.position.X = (NPC.ai[2] * 16 - (NPC.width / 2) + 8);
				NPC.position.Y = NPC.ai[3] * 16f - NPC.height;
				NPC.velocity.X = 0.0f;
				NPC.velocity.Y = 0.0f;
				NPC.ai[2] = 0.0f;
				NPC.ai[3] = 0.0f;
				// Teleport effects: arrived.
				SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
				for (int index1 = 0; index1 < 50; ++index1)
				{
					int newDust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Flare_Blue, 0.0f, 0.0f, 100, new Color(), 1.5f);
					Main.dust[newDust].velocity *= 3f;
					Main.dust[newDust].noGravity = true;
				}
			}

			++NPC.ai[0];

			if (NPC.ai[0] == 100 || NPC.ai[0] == 300)
			{
				NPC.ai[1] = 30f;
				NPC.netUpdate = true;
			}

			bool teleport = false;
			if (NPC.ai[0] >= 500 && Main.netMode != NetmodeID.MultiplayerClient)
				teleport = true;

			if (teleport)
			{
				Teleport();
				NPC.ai[0] = 1;
				NPC.ai[1] = 0;
			}

			if (NPC.ai[1] > 0)
			{
				--NPC.ai[1];
				if (NPC.ai[1] == 15)
				{
					SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						int amountOfProjectiles = 1;
						int flakenum = Main.rand.Next(3);
						DustHelper.DrawDustImage(new Vector2(NPC.Center.X, NPC.Center.Y - 40), ModContent.DustType<WinterbornDust>(), 0.25f, "SpiritMod/Effects/Snowflakes/Flake" + flakenum, 1.33f);
						for (int i = 0; i < amountOfProjectiles; ++i)
						{
							if (Main.rand.Next(2) == 0)
							{
								int somedamage = expertMode ? 15 : 30;
								int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), Main.player[NPC.target].Center.X, Main.player[NPC.target].Center.Y - 300, 0, 0, ModContent.ProjectileType<IceCloudHostile>(), somedamage, 1, Main.myPlayer, 0, 0);
								Main.projectile[p].hostile = true;
								Main.projectile[p].friendly = false;
								Main.projectile[p].tileCollide = false;
							}
							else
							{
								Vector2 direction = Main.player[NPC.target].Center - (NPC.Center - new Vector2(0, 30));
								direction.Normalize();
								direction.X *= 4.9f;
								direction.Y *= 4.9f;
								int somedamage = expertMode ? 17 : 34;
								int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y - 30, direction.X, direction.Y, ProjectileID.IceBolt, somedamage, 1, Main.myPlayer, 0, 0);
								Main.projectile[p].hostile = true;
								Main.projectile[p].friendly = false;
								Main.projectile[p].tileCollide = false;
							}
						}
					}
				}
			}

			if (Main.rand.Next(3) == 0)
				return false;
			Dust dust = Main.dust[Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y + 2f), NPC.width, NPC.height, DustID.Flare_Blue, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 100, new Color(), 0.9f)];
			dust.noGravity = true;
			dust.velocity.X = dust.velocity.X * 0.3f;
			dust.velocity.Y = (dust.velocity.Y * 0.2f) - 1;

			return false;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame,
							 drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.Assets.Request<Texture2D>("NPCs/WinterbornHerald/WinterbornMagic_Glow").Value);
		}

		public void Teleport()
		{
			NPC.ai[0] = 1f;
			int num1 = (int)Main.player[NPC.target].position.X / 16;
			int num2 = (int)Main.player[NPC.target].position.Y / 16;
			int num3 = (int)NPC.position.X / 16;
			int num4 = (int)NPC.position.Y / 16;
			int num5 = 20;
			int num6 = 0;
			bool flag1 = false;
			if (Math.Abs(NPC.position.X - Main.player[NPC.target].position.X) + Math.Abs(NPC.position.Y - Main.player[NPC.target].position.Y) > 2000.0)
			{
				num6 = 100;
				flag1 = true;
			}
			while (!flag1 && num6 < 100)
			{
				++num6;
				int index1 = Main.rand.Next(num1 - num5, num1 + num5);
				for (int index2 = Main.rand.Next(num2 - num5, num2 + num5); index2 < num2 + num5; ++index2)
				{
					if ((index2 < num2 - 4 || index2 > num2 + 4 || (index1 < num1 - 4 || index1 > num1 + 4)) && (index2 < num4 - 1 || index2 > num4 + 1 || (index1 < num3 - 1 || index1 > num3 + 1)) && Main.tile[index1, index2].HasUnactuatedTile)
					{
						bool flag2 = true;
						if ((Main.tile[index1, index2 - 1].LiquidType == LiquidID.Lava))
							flag2 = false;
						if (flag2 && Main.tileSolid[(int)Main.tile[index1, index2].TileType] && !Collision.SolidTiles(index1 - 1, index1 + 1, index2 - 4, index2 - 1))
						{
							NPC.ai[1] = 20f;
							NPC.ai[2] = (float)index1;
							NPC.ai[3] = (float)index2;
							flag1 = true;
							break;
						}
					}
				}
			}
			NPC.netUpdate = true;
		}

		public override void FindFrame(int frameHeight)
		{
			int currShootFrame = (int)NPC.ai[1];
			if (currShootFrame >= 25)
				NPC.frame.Y = frameHeight * 3;
			else if (currShootFrame >= 10)
				NPC.frame.Y = frameHeight * 2;
			else if (currShootFrame >= 5)
				NPC.frame.Y = frameHeight;
			else
				NPC.frame.Y = 0;

			NPC.spriteDirection = NPC.direction;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => NPC.downedBoss3 && (spawnInfo.SpawnTileY > Main.rockLayer && spawnInfo.Player.ZoneSnow) ? 0.035f : 0f;

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 5; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.UnusedWhiteBluePurple, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Flare_Blue, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
			}

			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SpiritMod/Gores/Winterborn/WinterbornGore1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SpiritMod/Gores/Winterborn/WinterbornGore2").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SpiritMod/Gores/Winterborn/WinterbornGore2").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SpiritMod/Gores/Winterborn/WinterbornGore3").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SpiritMod/Gores/Winterborn/WinterbornGore3").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SpiritMod/Gores/Winterborn/WinterbornGore4").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SpiritMod/Gores/Winterborn/WinterbornGore5").Type, 1f);

				NPC.position.X = NPC.position.X + (float)(NPC.width / 2);
				NPC.position.Y = NPC.position.Y + (float)(NPC.height / 2);
				NPC.width = 30;
				NPC.height = 30;
				NPC.position.X = NPC.position.X - (float)(NPC.width / 2);
				NPC.position.Y = NPC.position.Y - (float)(NPC.height / 2);

				for (int num621 = 0; num621 < 20; num621++)
				{
					int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Flare_Blue, 0f, 0f, 100, default, .8f);
					if (Main.rand.Next(2) == 0)
						Main.dust[num622].scale = 0.35f;
				}
				for (int num623 = 0; num623 < 40; num623++)
				{
					int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.BlueCrystalShard, 0f, 0f, 100, default, .43f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 3f;
				}
			}
		}
	}
}