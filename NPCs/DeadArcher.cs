using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpiritMod.NPCs
{
	public class DeadArcher : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Deadeye Marksman");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.GoblinArcher];
		}

		public override void SetDefaults()
		{
			npc.width = 36;
			npc.height = 46;
			npc.damage = 23;
			npc.defense = 9;
			npc.lifeMax = 43;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 120f;
			npc.knockBackResist = .30f;
			npc.aiStyle = 3;
			aiType = NPCID.GoblinArcher;
			animationType = NPCID.GoblinArcher;
		}

		public override void NPCLoot()
		{
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("OldLeather"));
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.spawnTileY < Main.rockLayer && (!Main.dayTime) && spawnInfo.player.ZoneOverworldHeight ? 0.03f : 0f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 40; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, 5, hitDirection, -1f, 0, default(Color), .45f);
			}
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Archer2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Archer1"), 1f);
				for (int k = 0; k < 80; k++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, 5, hitDirection, -1f, 0, default(Color), .85f);
				}
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
            GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/DeadArcher_Glow"));
        }
		public override void AI()
		{
	 	bool flag6;
        bool flag8 = true;
        int num1 = -1;
        int num2 = -1;
        if (npc.confused)
        {
          npc.ai[2] = 0.0f;
        }
        else
        {
          if ((double) npc.ai[1] > 0.0)
            --npc.ai[1];
          if (npc.justHit)
          {
            npc.ai[1] = 30f;
            npc.ai[2] = 0.0f;
          }
          int num3 = 70;
          bool flag9 = false;
          int num4 = num3 / 2;
          if ((double) npc.ai[2] > 0.0)
          {
            if (flag8)
              npc.TargetClosest(true);
            if ((double) npc.ai[1] == (double) num4)
            {
		 	float num5 = 11f;
			Vector2 vector2 = new Vector2(npc.position.X + (float) npc.width * 0.5f, npc.position.Y + (float) npc.height * 0.5f);  
            float num6 = Main.player[npc.target].position.X + (float) Main.player[npc.target].width * 0.5f - vector2.X;
            float num7 = Math.Abs(num6) * 0.1f;  
            float num8 = Main.player[npc.target].position.Y + (float) Main.player[npc.target].height * 0.5f - vector2.Y - num7; 
            float num14 = (float) Math.Sqrt((double) num6 * (double) num6 + (double) num8 * (double) num8);
            npc.netUpdate = true;
            float num15 = num5 / num14;
            float num16 = num6 * num15;
            float SpeedY = num8 * num15;
            int Damage = 7;
            int Type = 81;    
            vector2.X += num16;
            vector2.Y += SpeedY;  
            int p = Projectile.NewProjectile(vector2.X, vector2.Y, num16, SpeedY, Type, Damage, 0.0f, Main.myPlayer, 0.0f, 0.0f);
            Main.projectile[p].hostile = true;
			Main.projectile[p].friendly = false;
			npc.ai[2] = (double) Math.Abs(SpeedY) <= (double) Math.Abs(num16) * 2.0 ? ((double) Math.Abs(num16) <= (double) Math.Abs(SpeedY) * 2.0 ? ((double) SpeedY <= 0.0 ? 4f : 2f) : 3f) : ((double) SpeedY <= 0.0 ? 5f : 1f);
            
            if ((double) npc.velocity.Y != 0.0 || (double) npc.ai[1] <= 0.0)
            {
              npc.ai[2] = 0.0f;
              npc.ai[1] = 0.0f;
            }
            else if ( num1 != -1 && (double) npc.ai[1] >= (double) num1 && (double) npc.ai[1] < (double) (num1 + num2) && ( (double) npc.velocity.Y == 0.0))
            {
              npc.velocity.X *= 0.9f;
              npc.spriteDirection = npc.direction;
            }
          }
		}
		}
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(4) == 0)
			{
				target.AddBuff(BuffID.Darkness, 180);
			}
		}
	}
}
