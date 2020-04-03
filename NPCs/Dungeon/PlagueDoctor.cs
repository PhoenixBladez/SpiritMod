using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Dungeon
{
	public class PlagueDoctor : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dark Alchemist");
			Main.npcFrameCount[npc.type] = 12;
		}

		public override void SetDefaults()
		{
			npc.width = 34;
			npc.height = 48;
			npc.damage = 29;
			npc.defense = 16;
			npc.lifeMax = 100;
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath2;
			npc.value = 1000f;
			npc.knockBackResist = .35f;
		}

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.spawnTileType == TileID.GreenDungeonBrick && NPC.CountNPCS(mod.NPCType("PlagueDoctor")) < 1)
            {
                return spawnInfo.player.ZoneDungeon ? 0.06f : 0f;
            }
            return spawnInfo.player.ZoneDungeon && NPC.CountNPCS(mod.NPCType("PlagueDoctor")) < 1 ? 0.01f : 0f;
        }
        public override void HitEffect(int hitDirection, double damage)
		{
            int d = 37;
            int d1 = 75;
            for (int k = 0; k < 30; k++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
                Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, default(Color), .34f);
            }
            if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/PlagueDoctor_1"), 1f);
			}
		}

		public override void NPCLoot()
		{
			int lootamt = Main.rand.Next(30, 60);
			for (int J = 0; J <= lootamt; J++)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PlagueVial"));
			}
		}

		int frame = 0;
		int timer = 0;
        int shootTimer = 0;
		public override void AI()
		{
			npc.spriteDirection = npc.direction;
			Player target = Main.player[npc.target];
			int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));
			if (distance < 200)
			{
				npc.velocity.X = .008f * npc.direction;				
				shootTimer++;
				if (shootTimer >= 55)
				{
                    Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 106);
                    Vector2 direction = Main.player[npc.target].Center - npc.Center;
                    direction.Normalize();
                    direction.X *= 7f;
                    direction.Y *= 7f;
                    float A = (float)Main.rand.Next(-50, 50) * 0.02f;
                    float B = (float)Main.rand.Next(-50, 50) * 0.02f;
                    int p = Projectile.NewProjectile(npc.Center.X + (npc.direction * 12), npc.Center.Y - 10, direction.X + A, direction.Y + B, mod.ProjectileType("ToxicFlaskHostile"), npc.damage / 3 * 2, 1, Main.myPlayer, 0, 0);
                    for (int k = 0; k < 11; k++)
                    {
                        Dust.NewDust(npc.position, npc.width, npc.height, 75, (float)direction.X + A, (float)direction.Y + B, 0, default(Color), .61f);
                    }
                    Main.projectile[p].hostile = true;
                    shootTimer = 0;
				}
				timer++;
				if(timer >= 6)
				{
					frame++;
					timer = 0;
				}
                if (frame >= 5)
                {
                    frame = 0;
                }
			}
			else
			{
                shootTimer = 0;
                npc.aiStyle = 3;
				aiType = NPCID.Skeleton;
				timer++;
				if(timer >= 4)
				{
					frame++;
					timer = 0;
				}
				if(frame >= 11)
				{
					frame = 5;
				}
                if (frame < 5)
                {
                    frame = 5;
                }
			}	
            if (shootTimer > 120)
            {
                shootTimer = 120;
            }
            if (shootTimer < 0)
            {
                shootTimer = 0;
            }
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                             drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Dungeon/PlagueDoctor_Glow"));
        }
        public override void FindFrame(int frameHeight)
		{
			npc.frame.Y = frameHeight * frame;
		}
	}
}
