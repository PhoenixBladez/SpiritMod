using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.IO;

namespace SpiritMod.NPCs.Boulder_Termagant
{
	public class Boulder_Termagant : ModNPC
	{
		public bool hasGottenColor = false;
		public bool resetFrames = false;
		public bool isRoaring = false;
		public int r = 0;
		public int g = 0;
		public int b = 0;
		public int randomColor = 0;
		public int boulderTimer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boulder Behemoth");
			Main.npcFrameCount[NPC.type] = 11;
			NPCID.Sets.TrailCacheLength[NPC.type] = 30;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
		}

		public override void SetDefaults()
		{
			NPC.aiStyle = 3;
			NPC.lifeMax = 365;
			NPC.defense = 28;
			NPC.value = 1250f;
			AIType = 0;
			NPC.knockBackResist = 0.1f;
			NPC.width = 50;
			NPC.height = 38;
			NPC.damage = 70;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.lavaImmune = false;
			NPC.noTileCollide = false;
			NPC.HitSound = new Terraria.Audio.LegacySoundStyle(3, 6);
			NPC.DeathSound = new Terraria.Audio.LegacySoundStyle(4, 5);
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.BoulderBehemothBanner>();
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(hasGottenColor);
			writer.Write(resetFrames);
			writer.Write(isRoaring);
			writer.Write(r);
			writer.Write(g);
			writer.Write(b);
			writer.Write(randomColor);
			writer.Write(boulderTimer);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			hasGottenColor = reader.ReadBoolean();
			resetFrames = reader.ReadBoolean();
			isRoaring = reader.ReadBoolean();

			r = reader.ReadInt32();
			g = reader.ReadInt32();
			b = reader.ReadInt32();

			randomColor = reader.ReadInt32();
			boulderTimer = reader.ReadInt32();

		}
		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			if (projectile.type != ProjectileID.Boulder && projectile.type != ProjectileID.BoulderStaffOfEarth)
				return base.CanBeHitByProjectile(projectile);
			return false;
		}
		public override void AI()
		{
			Player player = Main.player[NPC.target];

			NPC.TargetClosest(true);
			NPC.spriteDirection = NPC.direction;

			if (!hasGottenColor)
			{
				hasGottenColor = true;
				randomColor = Main.rand.Next(6);
				if (randomColor == 0)
				{
					r = 219;
					g = 97;
					b = 255;
				}
				if (randomColor == 1)
				{
					r = 255;
					g = 198;
					b = 0;
				}
				if (randomColor == 2)
				{
					r = 23;
					g = 147;
					b = 234;
				}
				if (randomColor == 3)
				{
					r = 33;
					g = 184;
					b = 115;
				}
				if (randomColor == 4)
				{
					r = 238;
					g = 51;
					b = 53;
				}
				if (randomColor == 5)
				{
					r = 223;
					g = 230;
					b = 238;
				}
			}
			Lighting.AddLight(new Vector2(NPC.Center.X, NPC.Center.Y), r * 0.002f, g * 0.002f, b * 0.002f);

			if (Vector2.Distance(NPC.Center, player.Center) < 800f && Collision.CanHitLine(NPC.Center, 0, 0, Main.player[NPC.target].Center, 0, 0))
			{
				boulderTimer++;
			}
			if (boulderTimer > 299 && boulderTimer < 421)
			{
				if (boulderTimer == 300)
				{
					SoundEngine.PlaySound(SoundID.Trackable, (int)NPC.position.X, (int)NPC.position.Y, 180, 1f, -0.9f);
				}
				NPC.aiStyle = -1;
				NPC.velocity.X = 0f;
				isRoaring = true;
				NPC.defense = 60;
				if (!resetFrames)
				{
					NPC.netUpdate = true;
					NPC.frameCounter = 0;
					resetFrames = true;
				}
				if (boulderTimer == 420)
				{
					NPC.netUpdate = true;
					boulderTimer = 0;
					resetFrames = false;
					isRoaring = false;
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						for (int i = 0; i < 5; i++)
						{
							int proj;

							if (player.GetModPlayer<MyPlayer>().ZoneGranite)
								proj = Projectile.NewProjectile(player.Center.X - Main.rand.Next(-300, 300), player.Center.Y - Main.rand.Next(800, 1200), 0f, 2f + Main.rand.Next(1, 3), ModContent.ProjectileType<Granite_Boulder>(), 15, 0, Main.myPlayer, 0, 0);
							else if (player.GetModPlayer<MyPlayer>().ZoneMarble)
								proj = Projectile.NewProjectile(player.Center.X - Main.rand.Next(-300, 300), player.Center.Y - Main.rand.Next(800, 1200), 0f, 2f + Main.rand.Next(1, 3), ModContent.ProjectileType<Marble_Boulder>(), 15, 0, Main.myPlayer, 0, 0);
							else
								proj = Projectile.NewProjectile(player.Center.X - Main.rand.Next(-300, 300), player.Center.Y - Main.rand.Next(800, 1200), 0f, 2f + Main.rand.Next(1, 3), ModContent.ProjectileType<Cavern_Boulder>(), 15, 0, Main.myPlayer, 0, 0);

							Main.projectile[proj].netUpdate = true;
						}
					}
				}
			}
			else
			{
				NPC.aiStyle = 3;
				NPC.defense = 28;
			}
		}
		public override void OnKill()
		{
			Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.StoneBlock, Main.rand.Next(10, 26));
			if (Main.rand.NextBool(4))
			{
				if (r == 219)
					Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, 181, Main.rand.Next(1, 5));
				if (r == 255)
					Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, 180, Main.rand.Next(1, 5));
				if (r == 23)
					Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, 177, Main.rand.Next(1, 5));
				if (r == 33)
					Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, 179, Main.rand.Next(1, 5));
				if (r == 238)
					Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, 178, Main.rand.Next(1, 5));
				if (r == 223)
					Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, 182, Main.rand.Next(1, 5));
			}

		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/BoulderTermagent/RockTermagantGore4").Type, 1f);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/BoulderTermagent/RockTermagantGore3").Type, 1f);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/BoulderTermagent/RockTermagantGore2").Type, 1f);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/BoulderTermagent/RockTermagantGore1").Type, 1f);
			}
			for (int k = 0; k < 7; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Stone, 2.5f * hitDirection, -2.5f, 0, default, 1.2f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Stone, 2.5f * hitDirection, -2.5f, 0, default, 0.5f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Stone, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => false;
		public override float SpawnChance(NPCSpawnInfo spawnInfo) => !spawnInfo.PlayerSafe && spawnInfo.SpawnTileY > Main.rockLayer && Main.hardMode && !spawnInfo.Player.ZoneUnderworldHeight ? 0.045f : 0f;

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Vector2 vector2_3 = new Vector2((float)(TextureAssets.Npc[NPC.type].Value.Width / 2), (float)(TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2));
			float addHeight = 4f;
			Color color1 = Lighting.GetColor((int)(NPC.position.X + NPC.width * 0.5) / 16, (int)((NPC.position.Y + NPC.height * 0.5) / 16.0));
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Bottom - Main.screenPosition + new Vector2((float)(-TextureAssets.Npc[NPC.type].Value.Width * NPC.scale / 2.0 + vector2_3.X * (double)NPC.scale), (float)(-TextureAssets.Npc[NPC.type].Value.Height * (double)NPC.scale / Main.npcFrameCount[NPC.type] + 4.0 + vector2_3.Y * NPC.scale) + addHeight + NPC.gfxOffY), NPC.frame,
							drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			Main.spriteBatch.Draw(Mod.GetTexture("NPCs/Boulder_Termagant/Boulder_Termagant_Glow"), NPC.Bottom - Main.screenPosition + new Vector2((float)(-TextureAssets.Npc[NPC.type].Value.Width * NPC.scale / 2.0 + vector2_3.X * (double)NPC.scale), (-TextureAssets.Npc[NPC.type].Value.Height * NPC.scale / Main.npcFrameCount[NPC.type] + 4.0f + vector2_3.Y * NPC.scale) + addHeight + NPC.gfxOffY), NPC.frame, new Color(r - NPC.alpha, byte.MaxValue - NPC.alpha, g - NPC.alpha, b - NPC.alpha), NPC.rotation, vector2_3, NPC.scale, effects, 0.0f);
			float num = (float)(0.25 + (NPC.GetAlpha(color1).ToVector3() - new Vector3(1.25f)).Length() * 0.25);
			for (int index = 0; index < 16; ++index)
				Main.spriteBatch.Draw(Mod.GetTexture("NPCs/Boulder_Termagant/Boulder_Termagant_Glow"), NPC.Bottom - Main.screenPosition + new Vector2((float)(-TextureAssets.Npc[NPC.type].Value.Width * NPC.scale / 2.0 + vector2_3.X * NPC.scale), (-TextureAssets.Npc[NPC.type].Value.Height * NPC.scale / Main.npcFrameCount[NPC.type] + 4.0f + vector2_3.Y * NPC.scale) + addHeight + NPC.gfxOffY) + NPC.velocity.RotatedBy((double)index * 47079637050629, new Vector2()) * num, NPC.frame, new Color(r, g, b, 0), NPC.rotation, vector2_3, NPC.scale, effects, 0.0f);
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter++;
			if (!isRoaring)
			{
				if (NPC.velocity.Y == 0f)
				{
					if (NPC.velocity.X != 0f)
					{
						if (NPC.frameCounter < 6)
							NPC.frame.Y = 0 * frameHeight;
						else if (NPC.frameCounter < 12)
							NPC.frame.Y = 1 * frameHeight;
						else if (NPC.frameCounter < 18)
							NPC.frame.Y = 2 * frameHeight;
						else if (NPC.frameCounter < 24)
							NPC.frame.Y = 3 * frameHeight;
						else if (NPC.frameCounter < 30)
							NPC.frame.Y = 4 * frameHeight;
						else if (NPC.frameCounter < 36)
							NPC.frame.Y = 5 * frameHeight;
						else if (NPC.frameCounter < 42)
							NPC.frame.Y = 6 * frameHeight;
						else
							NPC.frameCounter = 0;
					}
				}
				else
					NPC.frame.Y = 4 * frameHeight;
			}
			else
			{
				if (NPC.frameCounter < 15)
					NPC.frame.Y = 7 * frameHeight;
				else if (NPC.frameCounter < 30)
					NPC.frame.Y = 8 * frameHeight;
				else if (NPC.frameCounter < 45)
					NPC.frame.Y = 9 * frameHeight;
				else if (NPC.frameCounter < 120)
					NPC.frame.Y = 10 * frameHeight;
				else
					NPC.frameCounter = 0;
			}
		}
	}
}