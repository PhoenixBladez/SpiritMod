using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class Mineroid : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orbitite");
			Main.npcFrameCount[npc.type] = 6;
		}

		public override void SetDefaults()
		{
			npc.width = 40;
			npc.height = 40;
			npc.damage = 32;
			npc.defense = 15;
			npc.lifeMax = 60;
			npc.HitSound = SoundID.NPCHit7;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 80f;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.knockBackResist = .45f;
			npc.aiStyle = 44;
			aiType = NPCID.FlyingAntlion;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return (spawnInfo.player.ZoneMeteor || spawnInfo.player.GetSpiritPlayer().ZoneAsteroid ) && spawnInfo.spawnTileY < Main.rockLayer & NPC.downedBoss2 ? 0.15f : 0f;
		}

		public override void NPCLoot()
		{
			if (Main.rand.Next(20) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("OrbiterStaff"));
			}
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Meteorite, Main.rand.Next(1, 2));
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
            GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Mineroid_Glow"));
        }
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 11; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, 24, hitDirection, -1f, 0, default(Color), .61f);
			}
			if (npc.life <= 0)
			{
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Mineroid/Mineroid1"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Mineroid/Mineroid2"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Mineroid/Mineroid3"));
                Gore.NewGore(npc.position, npc.velocity, 61);
				Gore.NewGore(npc.position, npc.velocity, 62);
				Gore.NewGore(npc.position, npc.velocity, 63);
				npc.position.X = npc.position.X + (float)(npc.width / 2);
				npc.position.Y = npc.position.Y + (float)(npc.height / 2);
				npc.width = 30;
				npc.height = 30;
				npc.position.X = npc.position.X - (float)(npc.width / 2);
				npc.position.Y = npc.position.Y - (float)(npc.height / 2);
				for (int num621 = 0; num621 < 20; num621++)
				{
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 24, 0f, 0f, 100, default(Color), 1f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0)
					{
						Main.dust[num622].scale = 0.5f;
					}
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
		int timer = 0;
		public override void AI()
		{
            timer++;
            if (timer >= 90)
            {
                bool expertMode = Main.expertMode;
                int damage = expertMode ? 10 : 16;
                Vector2 vector2_2 = Vector2.UnitY.RotatedByRandom(1.57079637050629f) * new Vector2(5f, 3f);
                int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, vector2_2.X, vector2_2.Y, mod.ProjectileType("MeteorShardHostile1"), damage, 0.0f, Main.myPlayer, 0.0f, (float)npc.whoAmI);
                Main.projectile[p].hostile = true;
                timer = 0;
            }

			npc.spriteDirection = npc.direction;
		}
	}
}
