using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class GraniteCore : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crackling Core");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 24;
			npc.height = 32;
			npc.damage = 19;
			npc.defense = 9;
			npc.noGravity = true;
			npc.lifeMax = 70;
			npc.HitSound = SoundID.NPCHit3;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 360f;
			npc.knockBackResist = .3f;
			npc.aiStyle = 44;
			aiType = NPCID.FlyingAntlion;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			int x = spawnInfo.spawnTileX;
			int y = spawnInfo.spawnTileY;
			int tile = (int)Main.tile[x, y].type;
			return (tile == 368) && spawnInfo.spawnTileY > Main.rockLayer && NPC.downedBoss2 ? 0.1f : 0f;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, 187);
			}
		}

		public override void AI()
		{
			npc.spriteDirection = npc.direction;
			Player target = Main.player[npc.target];
			int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));
			if (distance < 320)
			{
				npc.ai[0]++;
				if (npc.ai[0] >= 200)
				{
					int type = mod.ProjectileType("HedronBeam");
					int p = Terraria.Projectile.NewProjectile(npc.position.X, npc.position.Y, -(npc.position.X - target.position.X) / distance * 4, -(npc.position.Y - target.position.Y) / distance * 4, type, (int)((npc.damage * .5)), 0);
					Main.projectile[p].friendly = false;
					Main.projectile[p].hostile = true;
					npc.ai[0] = 0;
				}
			}
			int dust = Dust.NewDust(npc.position, npc.width, npc.height, 187);
		}

		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("GraniteChunk"), 1);
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(6) == 1)
			{
				target.AddBuff(BuffID.Confused, 160);
			}
		}
	}
}
