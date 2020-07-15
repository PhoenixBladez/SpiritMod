using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.NPCs.WhirlingWorlds
{
	public class BlizzardBandit: ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blizzard Bandit");
			Main.npcFrameCount[npc.type] = 16;
		}
		int timer = 0;
		bool shooting = false;
		bool gettingballs = false;
		public override void SetDefaults()
		{
			npc.aiStyle = 3;
			npc.lifeMax = 120;
			npc.defense = 6;
			aiType = NPCID.SnowFlinx;
			npc.value = 65f;
			npc.knockBackResist = 0.7f;
			npc.width = 30;
			npc.height = 54;
			npc.damage = 15;
			npc.lavaImmune = false;
			npc.noTileCollide = false;
			npc.alpha = 0;
			npc.dontTakeDamage = false;
			npc.HitSound = new Terraria.Audio.LegacySoundStyle(3, 1);
			npc.DeathSound = new Terraria.Audio.LegacySoundStyle(4, 1);
		}
		public override bool PreAI()
		{
			if (gettingballs)
			{
				npc.velocity.Y = 6;
				npc.velocity.X *= 0.08f;
			}
			if (timer == 240)
			{
				shooting = true;
				gettingballs = true;
				timer = 0;
			}
			if (!shooting)
			{
				timer++;
			}
			if (npc.velocity.X < 0f)
			{
				npc.spriteDirection = 1;
			}
			else if (npc.velocity.X > 0f)
			{
				npc.spriteDirection = -1;
			}
			return base.PreAI();
		}
		public override void AI()
		{
			if (shooting)
			{
				if (npc.velocity.X < 0f)
				{
					npc.spriteDirection = 1;
				}
				else if (npc.velocity.X > 0f)
				{
					npc.spriteDirection = -1;
				}
			}
		}
		public override void HitEffect(int hitDirection, double damage)
        {
            int d1 = 129;
            for (int k = 0; k < 30; k++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, Color.White, Main.rand.NextFloat(.3f, 1.1f));
            }
            Main.PlaySound(4, (int)npc.position.X, (int)npc.position.Y, 15, 1f, 0f);
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BlizzardBanditGore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BlizzardBanditGore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BlizzardBanditGore3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BlizzardBanditGore4"), 1f);
			}
		}
		int frame = 0;
		public override void FindFrame(int frameHeight)
		{
			Player player = Main.player[npc.target];
			npc.frameCounter++;
			
				if (!shooting)
				{
					if(npc.frameCounter >= 7) {
						frame++;
						npc.frameCounter = 0;
					}
					if(frame >= 6) {
						frame = 0;
					}
				}
				else 
				{
					if(npc.frameCounter >= 7) {
					frame++;
					npc.frameCounter = 0;
					}
					if (frame == 10)
					{
						gettingballs = false;
					}
					if((frame == 11 || frame == 14) && npc.frameCounter == 4) 
					{
						Main.PlaySound(1, (int)npc.position.X, (int)npc.position.Y, 106);

						Vector2 direction = Main.player[npc.target].Center - npc.Center;
						direction.Normalize();
						direction.X *= 8.5f;
						direction.Y *= 8.5f;
						float A = (float)Main.rand.Next(-50, 50) * 0.02f;
						float B = (float)Main.rand.Next(-50, 50) * 0.02f;
						int p = Projectile.NewProjectile(npc.Center.X + (npc.direction * 12), npc.Center.Y, direction.X + A, direction.Y + B, 166, npc.damage / 3, 1, Main.myPlayer, 0, 0);
						Main.projectile[p].hostile = true;
						Main.projectile[p].friendly = false;

					}
					if(frame >= 16) {
						shooting = false;
						frame = 0;
					}
					if(frame < 6) {
						frame = 6;
					}
				}
			npc.frame.Y = frameHeight * frame;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.player.ZoneSnow && Main.raining && spawnInfo.player.ZoneOverworldHeight ? 0.055f : 0f;
		}
        public override void NPCLoot()
        {
            if (Main.rand.Next(20) == 0)
            {
                npc.DropItem(ModContent.ItemType<Items.Armor.Masks.WinterHat>());
            }
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Snowball, Main.rand.Next(8, 14));

        }
    }
}