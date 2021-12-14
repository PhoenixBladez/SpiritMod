using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace SpiritMod.NPCs.Critters
{
	public class Grouper : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Grouper");
			Main.npcFrameCount[npc.type] = 6;
		}

		public override void SetDefaults()
		{
			npc.width = 32;
			npc.height = 22;
			npc.damage = 0;
			npc.defense = 0;
			npc.lifeMax = 35;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = .35f;
			npc.aiStyle = 16;
			npc.noGravity = true;
			npc.dontCountMe = true;
			npc.npcSlots = 0;
			aiType = NPCID.Goldfish;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
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
            Player player = Main.player[npc.target];
            {
                Player target = Main.player[npc.target];
                int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));
                if (distance < 65 && target.wet && npc.wet)
                {
                    Vector2 vel = npc.DirectionFrom(target.Center);
                    vel.Normalize();
                    vel *= 4.5f;
                    npc.velocity = vel;
                    npc.rotation = npc.velocity.X * .06f;
                    if (target.position.X > npc.position.X)
                    {
                        npc.spriteDirection = -1;
                        npc.direction = -1;
                        npc.netUpdate = true;
                    }
                    else if (target.position.X < npc.position.X)
                    {
                        npc.spriteDirection = 1;
                        npc.direction = 1;
                        npc.netUpdate = true;
                    }
                }
            }
        }

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0) {

				for (int num621 = 0; num621 < 40; num621++) {
					int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0.5f;
					Main.dust[dust].scale *= .6f;
				}
			}
		}
		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("RawFish"), Main.rand.Next(2, 4));

		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.player.ZoneBeach && spawnInfo.water ? 0.029f : 0f;
		}
	}
}