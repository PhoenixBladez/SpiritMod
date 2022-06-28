using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Festerfly
{
	public class Vilemoth : ModNPC
	{
		int timer = 0;
		int moveSpeed = 0;
		int moveSpeedY = 0;
		float HomeY = 120f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Festerfly");
			Main.npcFrameCount[NPC.type] = 4;
		}

		public override void SetDefaults()
		{
			NPC.width = 40;
			NPC.height = 30;
			NPC.damage = 32;
			NPC.defense = 8;
			NPC.lifeMax = 59;
			NPC.HitSound = SoundID.NPCHit35; //Dr Man Fly
			NPC.DeathSound = SoundID.NPCDeath22;
			NPC.value = 110f;
			NPC.noGravity = true;
			NPC.noTileCollide = false;
			NPC.buffImmune[BuffID.Confused] = true;
			NPC.knockBackResist = .45f;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.FesterflyBanner>();
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 30; k++)
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Plantera_Green, 2.5f * hitDirection, -2.5f, 0, Color.Purple, 0.3f);

			if (NPC.life <= 0)
			{
				SoundEngine.PlaySound(SoundID.NPCDeath38, NPC.Center);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Pesterfly/Pesterfly1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Pesterfly/Pesterfly2").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Pesterfly/Pesterfly3").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Pesterfly/Pesterfly4").Type, 1f);
			}
		}
		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

		public override void AI()
		{
			if (Main.rand.NextFloat() < 0.431579f)
			{
				Vector2 position = NPC.Center;
				int d = Dust.NewDust(NPC.position, NPC.width, NPC.height + 10, DustID.ScourgeOfTheCorruptor, 0, 1f, 0, new Color(), 0.7f);
				Main.dust[d].velocity *= .1f;
			}
			Player player = Main.player[NPC.target];
			NPC.rotation = NPC.velocity.X * 0.1f;
			if (NPC.Center.X >= player.Center.X && moveSpeed >= -60) // flies to players x position
				moveSpeed--;

			if (NPC.Center.X <= player.Center.X && moveSpeed <= 60)
				moveSpeed++;

			NPC.velocity.X = moveSpeed * 0.06f;

			if (NPC.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -50) //Flies to players Y position
			{
				moveSpeedY--;
				HomeY = 120f;
			}

			if (NPC.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 50)
				moveSpeedY++;

			NPC.velocity.Y = moveSpeedY * 0.06f;
			timer++;
			if (timer >= 100 && Main.netMode != NetmodeID.MultiplayerClient)
			{
				Vector2 vector2_2 = Vector2.UnitY.RotatedByRandom(1.57079637050629f) * new Vector2(5f, 3f);
				int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, vector2_2.X, vector2_2.Y, ModContent.ProjectileType<VileWaspProjectile>(), 0, 0.0f, Main.myPlayer, 0.0f, (float)NPC.whoAmI);
				Main.projectile[p].hostile = true;
				timer = 0;
			}

			NPC.ai[3]++;
			if (NPC.ai[3] >= 300 && NPC.CountNPCS(ModContent.NPCType<VileWasp>()) < 2)
			{
				NPC.ai[3] = 0;
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					SoundEngine.PlaySound(SoundID.Zombie51, NPC.Center);
					for (int j = 0; j < 10; j++)
					{
						Vector2 vector2 = Vector2.UnitX * -NPC.width / 2f;
						vector2 += -Utils.RotatedBy(Vector2.UnitY, (j * 3.141591734f / 6f), default) * new Vector2(8f, 16f);
						vector2 = Utils.RotatedBy(vector2, (NPC.rotation - 1.57079637f), default);
						int num8 = Dust.NewDust(NPC.Center, 0, 0, DustID.ScourgeOfTheCorruptor, 0f, 0f, 160, new Color(), 1f);
						Main.dust[num8].scale = 1.3f;
						Main.dust[num8].noGravity = true;
						Main.dust[num8].position = NPC.Center + vector2;
						Main.dust[num8].velocity = NPC.velocity * 0.1f;
						Main.dust[num8].noLight = true;
						Main.dust[num8].velocity = Vector2.Normalize(NPC.Center - NPC.velocity * 3f - Main.dust[num8].position) * 1.25f;
					}
					NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<VileWasp>());
				}
			}

			NPC.spriteDirection = NPC.direction;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.Player.ZoneCorrupt && spawnInfo.Player.ZoneOverworldHeight && !NPC.AnyNPCs(ModContent.NPCType<Vilemoth>()) ? .05f : 0f;
	}
}