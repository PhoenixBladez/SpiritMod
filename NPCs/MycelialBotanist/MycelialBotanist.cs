using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using SpiritMod.Projectiles.Hostile;

namespace SpiritMod.NPCs.MycelialBotanist
{
	public class MycelialBotanist : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mycelial Botanist");
			Main.npcFrameCount[NPC.type] = 11;
		}
		int timer = 0;
		bool shooting = false;
		public override void SetDefaults()
		{
			NPC.aiStyle = 3;
			NPC.lifeMax = 65;
			NPC.defense = 6;
			AIType = NPCID.Skeleton;
			NPC.value = 65f;
			NPC.knockBackResist = 0.7f;
			NPC.width = 56;
			NPC.height = 50;
			NPC.damage = 15;
			NPC.lavaImmune = false;
			NPC.noTileCollide = false;
			NPC.alpha = 0;
			NPC.dontTakeDamage = false;
			NPC.HitSound = new Terraria.Audio.LegacySoundStyle(3, 1);
			NPC.DeathSound = new Terraria.Audio.LegacySoundStyle(29, 76);
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Items.Banners.MyceliumBotanistBanner>();
        }
		public override bool PreAI()
		{
			NPC.TargetClosest();
			if (shooting) {
				NPC.velocity.Y = 6;
				NPC.velocity.X *= 0.08f;
			}
			if (timer == 240) {
				shooting = true;
				timer = 0;
			}
			if (!shooting) {
				timer++;
			}
			if (NPC.velocity.X < 0f) {
				NPC.spriteDirection = -1;
			}
			else if (NPC.velocity.X > 0f) {
				NPC.spriteDirection = 1;
			}
			return base.PreAI();
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			int x = spawnInfo.SpawnTileX;
			int y = spawnInfo.SpawnTileY;
			int tile = (int)Main.tile[x, y].TileType;
			return (tile == TileID.MushroomGrass) && spawnInfo.SpawnTileY > Main.rockLayer ? 2f : 0f;

		}
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 30; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Rope, 2.5f * hitDirection, -2.5f, 0, Color.White, Main.rand.NextFloat(.3f, 1.1f));
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Harpy, 2.5f * hitDirection, -2.5f, 0, Color.White, Main.rand.NextFloat(.3f, 1.1f));
			}
			//Main.PlaySound(3, (int)npc.position.X, (int)npc.position.Y, 45, 1f, 0f);
			if (NPC.life <= 0) {
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/MycelialBotanistGore1").Type, 1f);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/MycelialBotanistGore2").Type, 1f);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/MycelialBotanistGore3").Type, 1f);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/MycelialBotanistGore4").Type, 1f);
			}
		}
		int frame = 0;
		public override void FindFrame(int frameHeight)
		{
			Player player = Main.player[NPC.target];
			NPC.frameCounter++;

			if (!shooting) {
				if (NPC.frameCounter >= 7) {
					frame++;
					NPC.frameCounter = 0;
				}
				if (frame >= 5) {
					frame = 0;
				}
			}
			else {
				if (Main.player[NPC.target].Center.X < NPC.Center.X) {
					NPC.spriteDirection = -1;
				}
				else {
					NPC.spriteDirection = 1;
				}
				if (NPC.frameCounter >= 7) {
					frame++;
					NPC.frameCounter = 0;
				}
				if (frame == 10 && NPC.frameCounter == 6) {
					SoundEngine.PlaySound(SoundID.Item, (int)NPC.position.X, (int)NPC.position.Y, 1);
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
						float distance = MathHelper.Clamp(direction.Length(), -250, 250);
						direction.Normalize();
						direction *= distance / 20;
						int p = Projectile.NewProjectile(NPC.Center.X + (NPC.direction * 12), NPC.Center.Y, direction.X, direction.Y, ModContent.ProjectileType<MyceliumHat>(), 15, 1, Main.myPlayer, NPC.whoAmI);
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
			NPC.frame.Y = frameHeight * frame;
		}
	}
}