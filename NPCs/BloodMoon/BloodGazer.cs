using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.BloodMoon
{
    public class BloodGazer : ModNPC
	{
		int timer = 0;
		int moveSpeed = 0;
		int moveSpeedY = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Gazer");
		}

		public override void SetDefaults()
		{
			npc.width = 50;
			npc.height = 68;
			npc.damage = 45;
			npc.defense = 13;
			npc.lifeMax = 800;
			npc.knockBackResist = 0.1f;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.npcSlots = 3;
			Main.npcFrameCount[npc.type] = 4;
			npc.HitSound = SoundID.NPCHit19;
			npc.DeathSound = SoundID.NPCDeath22;
		}

		public override void AI()
		{
			npc.spriteDirection = npc.direction;
			Player player = Main.player[npc.target];
			if (npc.Center.X >= player.Center.X && moveSpeed >= -30) // flies to players x position
				moveSpeed--;
			else if (npc.Center.X <= player.Center.X && moveSpeed <= 30)
				moveSpeed++;

			npc.velocity.X = moveSpeed * 0.1f;

			if (npc.Center.Y >= player.Center.Y - 50f && moveSpeedY >= -30) //Flies to players Y position
				moveSpeedY--;
			else if (npc.Center.Y <= player.Center.Y - 50f && moveSpeedY <= 30)
				moveSpeedY++;

			npc.velocity.Y = moveSpeedY * 0.1f;
            npc.ai[0] += 4f;
            if (npc.ai[0] > 96f)
            {
                npc.ai[0] = 0f;
                int num1169 = (int)(npc.position.X + 10f + (float)Main.rand.Next(npc.width - 20));
                int num1170 = (int)(npc.position.Y + (float)npc.height + 4f);
                int num184 = 26;
                if (Main.expertMode)
                {
                    num184 = 18;
                }
                int bloodproj;
                bloodproj = Main.rand.Next(new int[] { ModContent.ProjectileType<GazerEye>(), mod.ProjectileType("GazerEye1")});
                Projectile.NewProjectile((float)num1169, (float)num1170, 0f, 5f, bloodproj, num184, 0f, Main.myPlayer, 0f, 0f);
                return;
            }
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0 || npc.life >= 0)
            {
                int d = 5;
                for (int k = 0; k < 25; k++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.47f);
                    Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, .97f);
                }
            }

            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Gazer/Gazer1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Gazer/Gazer2"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Gazer/Gazer3"), 1f);
                int d = 5;
                for (int k = 0; k < 25; k++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.97f);
                    Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 1.27f);
                }
                for (int i = 0; i < 16; i++)
                {
                    int bloodproj;
                    bloodproj = Main.rand.Next(new int[] { mod.ProjectileType("Feeder1"), mod.ProjectileType("Feeder2"), mod.ProjectileType("Feeder3") });
                    float rotation = (float)(Main.rand.Next(0, 361) * (Math.PI / 180));
                    Vector2 velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
                    int proj = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, velocity.X, velocity.Y, bloodproj, npc.damage/2, 1, Main.myPlayer, 0, 0);
                    Main.projectile[proj].friendly = false;
                    Main.projectile[proj].hostile = true;
                    Main.projectile[proj].velocity *= 4f;
                }
            }
        }
        public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.2f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.spawnTileY < Main.rockLayer && (Main.bloodMoon) && Main.hardMode && !NPC.AnyNPCs(ModContent.NPCType<BloodGazer>()) ? 0.024f : 0f;
		}
	}
}
