using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.EvilBiomeDrops.GastricGusher;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Bloater
{
	public class Spewer : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloater");
			Main.npcFrameCount[npc.type] = 9;
		}

		public override void SetDefaults()
		{
			npc.width = 40;
			npc.height = 44;
			npc.damage = 25;
			npc.defense = 5;
			npc.knockBackResist = 0.2f;
			npc.value = 90;
			npc.lifeMax = 45;
			npc.HitSound = SoundID.NPCHit18;
			npc.DeathSound = SoundID.NPCDeath21;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.buffImmune[BuffID.Confused] = true;
			npc.noGravity = true;
			npc.noTileCollide = false;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.BloaterBanner>();
		}

		public override void FindFrame(int frameHeight) => npc.frame.Y = frameHeight * frame;
		public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.player.ZoneCrimson && spawnInfo.player.ZoneOverworldHeight ? .075f : 0f;

		int frame = 0;

		public override void AI()
		{
			npc.rotation = npc.velocity.X * .08f;
			bool expertMode = Main.expertMode;
			float velMax = 1f;
			float acceleration = 0.011f;
			npc.TargetClosest(true);
			Vector2 center = npc.Center;
			float deltaX = Main.player[npc.target].position.X + (Main.player[npc.target].width / 2) - center.X;
			float deltaY = Main.player[npc.target].position.Y + (Main.player[npc.target].height / 2) - center.Y;
			float distance = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
			npc.ai[1] += 1f;
			if (distance < 240) {
				if (npc.ai[1] >= 120 && npc.ai[1] <= 180) {
					if (Main.rand.NextBool(10) && Main.netMode != NetmodeID.MultiplayerClient) {
						Main.PlaySound(SoundID.Item34, npc.Center);
						Vector2 direction = Main.player[npc.target].Center - npc.Center;
						direction.Normalize();
						direction.X *= 11.5f;
						direction.Y *= 8;
						int damage = expertMode ? 11 : 13;
						int vomit = Projectile.NewProjectile(npc.Center.X, npc.Center.Y + 4, direction.X, direction.Y + Main.rand.NextFloat(-.5f, .5f), ModContent.ProjectileType<VomitProj>(), damage, 1, Main.myPlayer, 0, 0);
						Main.projectile[vomit].netUpdate = true;
						npc.netUpdate = true;
					}
				}
			}
			if (Main.rand.NextFloat() < 0.131579f) {
				int d = Dust.NewDust(npc.position, npc.width, npc.height + 10, DustID.Blood, 0, 1f, 0, new Color(), 0.7f);
				Main.dust[d].velocity *= .1f;
			}
			if (npc.ai[1] == 120 && distance < 240) {
				Main.PlaySound(SoundID.NPCKilled, npc.Center, 13);
				Main.PlaySound(SoundID.Zombie, npc.Center, 40);
			}
			if (npc.ai[1] > 40 && npc.ai[1] < 180) {
				if (distance < 240) {
					float num395 = Main.mouseTextColor / 200f - 0.25f;
					num395 *= 0.2f;
					npc.scale = num395 + 0.95f;
					++npc.ai[2];
					if (npc.ai[2] >= 4) {
						npc.ai[2] = 0;
						frame++;
					}
					if (frame >= 9 || frame < 5)
						frame = 7;
				}
				else {
					++npc.ai[2];
					if (npc.ai[2] >= 10) {
						npc.ai[2] = 0;
						frame++;
					}
					if (frame >= 4)
						frame = 0;
				}
			}
			else {
				++npc.ai[2];
				if (npc.ai[2] >= 10) {
					npc.ai[2] = 0;
					frame++;
				}
				if (frame >= 4)
					frame = 0;
			}
			if (npc.ai[1] > 200.0) {

				if (npc.ai[1] > 300.0) {
					npc.ai[1] = 0f;
				}
			}
			else if (distance < 120.0) {
				npc.ai[0] += 0.9f;
				if (npc.ai[0] > 0f) {
					npc.velocity.Y = npc.velocity.Y + 0.039f;
				}
				else {
					npc.velocity.Y = npc.velocity.Y - 0.019f;
				}
				if (npc.ai[0] < -100f || npc.ai[0] > 100f) {
					npc.velocity.X = npc.velocity.X + 0.029f;
				}
				else {
					npc.velocity.X = npc.velocity.X - 0.029f;
				}
				if (npc.ai[0] > 25f) {
					npc.ai[0] = -200f;
				}
			}
			if (Main.rand.Next(30) == 0 && Main.netMode != NetmodeID.MultiplayerClient) {
				if (Main.rand.NextBool(2)) {
					npc.velocity.Y = npc.velocity.Y + 0.439f;
				}
				else {
					npc.velocity.Y = npc.velocity.Y - 0.419f;
				}
				npc.netUpdate = true;
			}
			if (distance > 350.0) {
				velMax = 5f;
				acceleration = 0.2f;
			}
			else if (distance > 300.0) {
				velMax = 3f;
				acceleration = 0.25f;
			}
			else if (distance > 250.0) {
				velMax = 2.5f;
				acceleration = 0.13f;
			}
			if (distance > 500) {
				npc.noTileCollide = true;
			}
			else {
				npc.noTileCollide = false;
			}
			float stepRatio = velMax / distance;
			float velLimitX = deltaX * stepRatio;
			float velLimitY = deltaY * stepRatio;
			if (Main.player[npc.target].dead) {
				velLimitX = (float)((npc.direction * velMax) / 2.0);
				velLimitY = (float)((-velMax) / 2.0);
			}
			if (npc.velocity.X < velLimitX)
				npc.velocity.X = npc.velocity.X + acceleration;
			else if (npc.velocity.X > velLimitX)
				npc.velocity.X = npc.velocity.X - acceleration;
			if (npc.velocity.Y < velLimitY)
				npc.velocity.Y = npc.velocity.Y + acceleration;
			else if (npc.velocity.Y > velLimitY)
				npc.velocity.Y = npc.velocity.Y - acceleration;
			npc.spriteDirection = -npc.direction;

		}
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 23; k++)
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, hitDirection * 1.5f, -1f, 0, default, .91f);
			if (npc.life <= 0) {
				Main.PlaySound(SoundID.NPCKilled, npc.Center, 30);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Spewer/Spewer1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Spewer/Spewer2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Spewer/Spewer3"), 1f);
			}
		}
        public override void NPCLoot()
        {
            if (Main.rand.NextBool(3))
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Vertebrae);
			if (Main.rand.Next(100) < 4)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<GastricGusher>());
		}
    }
}
