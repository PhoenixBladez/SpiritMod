using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using SpiritMod.Projectiles.Hostile;

namespace SpiritMod.NPCs.WhirlingWorlds
{
	public class MycelialBotanist : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mycelial Botanist");
			Main.npcFrameCount[npc.type] = 11;
		}
		int timer = 0;
		bool shooting = false;
		public override void SetDefaults()
		{
			npc.aiStyle = 3;
			npc.lifeMax = 120;
			npc.defense = 6;
			aiType = NPCID.Skeleton;
			npc.value = 65f;
			npc.knockBackResist = 0.7f;
			npc.width = 56;
			npc.height = 50;
			npc.damage = 15;
			npc.lavaImmune = false;
			npc.noTileCollide = false;
			npc.alpha = 0;
			npc.dontTakeDamage = false;
			npc.HitSound = new Terraria.Audio.LegacySoundStyle(3, 1);
			npc.DeathSound = new Terraria.Audio.LegacySoundStyle(29, 76);
		}
		public override bool PreAI()
		{
			npc.TargetClosest();
			if (shooting) {
				npc.velocity.Y = 6;
				npc.velocity.X *= 0.08f;
			}
			if (timer == 240) {
				shooting = true;
				timer = 0;
			}
			if (!shooting) {
				timer++;
			}
			if (npc.velocity.X < 0f) {
				npc.spriteDirection = -1;
			}
			else if (npc.velocity.X > 0f) {
				npc.spriteDirection = 1;
			}
			return base.PreAI();
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			int x = spawnInfo.spawnTileX;
			int y = spawnInfo.spawnTileY;
			int tile = (int)Main.tile[x, y].type;
			return (tile == TileID.MushroomGrass) && spawnInfo.spawnTileY > Main.rockLayer ? 2f : 0f;

		}
		public override void HitEffect(int hitDirection, double damage)
		{
			int d1 = 129;
			for (int k = 0; k < 30; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, Color.White, Main.rand.NextFloat(.3f, 1.1f));
				Dust.NewDust(npc.position, npc.width, npc.height, 42, 2.5f * hitDirection, -2.5f, 0, Color.White, Main.rand.NextFloat(.3f, 1.1f));
			}
			//Main.PlaySound(3, (int)npc.position.X, (int)npc.position.Y, 45, 1f, 0f);
			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/MycelialBotanistGore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/MycelialBotanistGore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/MycelialBotanistGore3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/MycelialBotanistGore4"), 1f);
			}
		}
		int frame = 0;
		public override void FindFrame(int frameHeight)
		{
			Player player = Main.player[npc.target];
			npc.frameCounter++;

			if (!shooting) {
				if (npc.frameCounter >= 7) {
					frame++;
					npc.frameCounter = 0;
				}
				if (frame >= 5) {
					frame = 0;
				}
			}
			else {
				if (Main.player[npc.target].Center.X < npc.Center.X) {
					npc.spriteDirection = -1;
				}
				else {
					npc.spriteDirection = 1;
				}
				if (npc.frameCounter >= 7) {
					frame++;
					npc.frameCounter = 0;
				}
				if (frame == 10 && npc.frameCounter == 6) {
					Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 1);
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						Vector2 direction = Main.player[npc.target].Center - npc.Center;
						float distance = MathHelper.Clamp(direction.Length(), -250, 250);
						direction.Normalize();
						direction *= distance / 20;
						int p = Projectile.NewProjectile(npc.Center.X + (npc.direction * 12), npc.Center.Y, direction.X, direction.Y, ModContent.ProjectileType<MyceliumHat>(), 15, 1, Main.myPlayer, npc.whoAmI);
					}
				}
				if (frame >= 11) {
					shooting = false;
					frame = 0;
				}
				if (frame < 5) {
					frame = 5;
				}
			}
			npc.frame.Y = frameHeight * frame;
		}
	}
}