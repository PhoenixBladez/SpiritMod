using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class Midknight : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mid-Knight");
			Main.npcFrameCount[npc.type] = 15;
		}

		public override void SetDefaults()
		{
			npc.width = 32;
			npc.height = 38;
			npc.damage = 60;
			npc.defense = 22;
			npc.lifeMax = 220;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 100f;
			npc.knockBackResist = .2f;
			npc.aiStyle = 3;
			aiType = NPCID.SnowFlinx;
			animationType = NPCID.AngryBones;
		}

		/*public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.spawnTileY < Main.rockLayer && (!Main.dayTime) && spawnInfo.player.ZoneOverworldHeight && Main.hardMode ? 0.01f : 0f;
		}*/
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			{

				Texture2D ring = mod.GetTexture("Effects/Glowmasks/Dusking_Circle");
				Vector2 origin = new Vector2(ring.Width * 0.5F, ring.Height * 0.5F);
				spriteBatch.Draw(ring, (npc.Center) - Main.screenPosition, null, new Color(255, 255, 255, 100), npc.localAI[1], origin, .25f, SpriteEffects.None, 0);
			}
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Midknight_Glow"));
		}
		public override void AI()
		{
			npc.localAI[1] += 0.03F;
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.17f, .08f, 0.3f);
			npc.spriteDirection = npc.direction;
			Player target = Main.player[npc.target];
			int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));
			if(distance < 64) {
				target.AddBuff(BuffID.Darkness, 65);
			}
			if(distance > 640 && Main.rand.Next(6) == 1) {
				if(Main.netMode != NetmodeID.MultiplayerClient) {
					Main.PlaySound(SoundID.Zombie, (int)npc.position.X, (int)npc.position.Y, 53);
					npc.position.X = target.position.X + Main.rand.Next(50, 100) * -target.direction;
					npc.position.Y = target.position.Y - Main.rand.Next(30, 60);
					npc.netUpdate = true;
					for(int num73 = 0; num73 < 20; num73++) {
						int index = Dust.NewDust(npc.position, 128, 128, 70, 0.0f, 0.0f, 200, new Color(), 0.5f);
						Main.dust[index].noGravity = true;
						Main.dust[index].velocity *= 0.75f;
						Main.dust[index].fadeIn = 1.3f;
						Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
						vector2_1.Normalize();
						Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
						Main.dust[index].velocity = vector2_2;
						vector2_2.Normalize();
						Vector2 vector2_3 = vector2_2 * 34f;
						Main.dust[index].position = npc.Center - vector2_3;
					}
				}
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			{
				target.AddBuff(BuffID.Darkness, 120, true);
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			for(int k = 0; k < 40; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, 173, hitDirection * 6f, -1f, 0, default(Color), .45f);
			}
			if(npc.life <= 0) {
				for(int k = 0; k < 10; k++) {
                    Gore.NewGore(npc.position, npc.velocity, 99);
                    Gore.NewGore(npc.position, npc.velocity, 99);
                    Gore.NewGore(npc.position, npc.velocity, 99);
                    Dust.NewDust(npc.position, npc.width, npc.height, 173, hitDirection * 6f, -1f, 0, default(Color), 1f);
				}
			}
		}

	}
}
