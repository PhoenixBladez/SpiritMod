using Microsoft.Xna.Framework;
using Terraria;
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
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 40;
			npc.height = 30;
			npc.damage = 32;
			npc.defense = 8;
			npc.lifeMax = 59;
			npc.HitSound = SoundID.NPCHit35; //Dr Man Fly
			npc.DeathSound = SoundID.NPCDeath22;
			npc.value = 110f;
			npc.noGravity = true;
			npc.noTileCollide = false;
			npc.knockBackResist = .45f;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.FesterflyBanner>();
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			int d = 167;
			for (int k = 0; k < 30; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.Purple, 0.3f);
			}
			if (npc.life <= 0) {
				Main.PlaySound(SoundID.NPCKilled, npc.Center, 38);
				{
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Pesterfly/Pesterfly1"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Pesterfly/Pesterfly2"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Pesterfly/Pesterfly3"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Pesterfly/Pesterfly4"), 1f);
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
		public override void AI()
		{
			if (Main.rand.NextFloat() < 0.431579f) {
				{
					Vector2 position = npc.Center;
					int d = Dust.NewDust(npc.position, npc.width, npc.height + 10, DustID.ScourgeOfTheCorruptor, 0, 1f, 0, new Color(), 0.7f);
					Main.dust[d].velocity *= .1f;
				}
			}
			Player player = Main.player[npc.target];
			npc.rotation = npc.velocity.X * 0.1f;
			if (npc.Center.X >= player.Center.X && moveSpeed >= -60) // flies to players x position
				moveSpeed--;

			if (npc.Center.X <= player.Center.X && moveSpeed <= 60)
				moveSpeed++;

			npc.velocity.X = moveSpeed * 0.06f;

			if (npc.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -50) //Flies to players Y position
			{
				moveSpeedY--;
				HomeY = 120f;
			}

			if (npc.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 50)
				moveSpeedY++;

			npc.velocity.Y = moveSpeedY * 0.06f;
			timer++;
			if (timer >= 100 && Main.netMode != NetmodeID.MultiplayerClient) {
				Vector2 vector2_2 = Vector2.UnitY.RotatedByRandom(1.57079637050629f) * new Vector2(5f, 3f);
				int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, vector2_2.X, vector2_2.Y, mod.ProjectileType("VileWaspProjectile"), 0, 0.0f, Main.myPlayer, 0.0f, (float)npc.whoAmI);
				Main.projectile[p].hostile = true;
                timer = 0;
			}
            npc.ai[3]++;
            {
				if (npc.ai[3] >= 300 && NPC.CountNPCS(ModContent.NPCType<VileWasp>()) < 2)
                {
                    npc.ai[3] = 0;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Main.PlaySound(SoundID.Zombie, npc.Center, 51);
                        for (int j = 0; j < 10; j++)
                        {
                            Vector2 vector2 = Vector2.UnitX * -npc.width / 2f;
                            vector2 += -Utils.RotatedBy(Vector2.UnitY, (j * 3.141591734f / 6f), default) * new Vector2(8f, 16f);
                            vector2 = Utils.RotatedBy(vector2, (npc.rotation - 1.57079637f), default);
                            int num8 = Dust.NewDust(npc.Center, 0, 0, DustID.ScourgeOfTheCorruptor, 0f, 0f, 160, new Color(), 1f);
                            Main.dust[num8].scale = 1.3f;
                            Main.dust[num8].noGravity = true;
                            Main.dust[num8].position = npc.Center + vector2;
                            Main.dust[num8].velocity = npc.velocity * 0.1f;
                            Main.dust[num8].noLight = true;
                            Main.dust[num8].velocity = Vector2.Normalize(npc.Center - npc.velocity * 3f - Main.dust[num8].position) * 1.25f;
                        }
                        NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, ModContent.NPCType<VileWasp>());
                    }
                }
            }

			npc.spriteDirection = npc.direction;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.player.ZoneCorrupt && spawnInfo.player.ZoneOverworldHeight && !NPC.AnyNPCs(ModContent.NPCType<Vilemoth>()) ? .05f : 0f;
	}
}
