using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Midknight
{
	public class Midknight : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mid-Knight");
			Main.npcFrameCount[NPC.type] = 15;
		}

		public override void SetDefaults()
		{
			NPC.width = 32;
			NPC.height = 38;
			NPC.damage = 60;
			NPC.defense = 22;
			NPC.lifeMax = 220;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = 100f;
			NPC.knockBackResist = .2f;
			NPC.aiStyle = 3;
			AIType = NPCID.SnowFlinx;
			AnimationType = NPCID.AngryBones;
		}

		/*public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.spawnTileY < Main.rockLayer && (!Main.dayTime) && spawnInfo.player.ZoneOverworldHeight && Main.hardMode ? 0.01f : 0f;
		}*/
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			{

				Texture2D ring = Mod.Assets.Request<Texture2D>("Effects/Glowmasks/Dusking_Circle").Value;
				Vector2 origin = new Vector2(ring.Width * 0.5F, ring.Height * 0.5F);
				spriteBatch.Draw(ring, (NPC.Center) - Main.screenPosition, null, new Color(255, 255, 255, 100), NPC.localAI[1], origin, .25f, SpriteEffects.None, 0);
			}
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame,
							 drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.Assets.Request<Texture2D>("NPCs/Midknight/Midknight_Glow").Value);
		public override void AI()
		{
			NPC.localAI[1] += 0.03F;
			Lighting.AddLight((int)((NPC.position.X + (float)(NPC.width / 2)) / 16f), (int)((NPC.position.Y + (float)(NPC.height / 2)) / 16f), 0.17f, .08f, 0.3f);
			NPC.spriteDirection = NPC.direction;
			Player target = Main.player[NPC.target];
			int distance = (int)Math.Sqrt((NPC.Center.X - target.Center.X) * (NPC.Center.X - target.Center.X) + (NPC.Center.Y - target.Center.Y) * (NPC.Center.Y - target.Center.Y));
			if (distance < 64) {
				target.AddBuff(BuffID.Darkness, 65);
			}
			if (distance > 640 && Main.rand.Next(6) == 1) {
				if (Main.netMode != NetmodeID.MultiplayerClient) {
					SoundEngine.PlaySound(SoundID.Zombie, (int)NPC.position.X, (int)NPC.position.Y, 53);
					NPC.position.X = target.position.X + Main.rand.Next(50, 100) * -target.direction;
					NPC.position.Y = target.position.Y - Main.rand.Next(30, 60);
					NPC.netUpdate = true;
					for (int num73 = 0; num73 < 20; num73++) {
						int index = Dust.NewDust(NPC.position, 128, 128, DustID.PurpleCrystalShard, 0.0f, 0.0f, 200, new Color(), 0.5f);
						Main.dust[index].noGravity = true;
						Main.dust[index].velocity *= 0.75f;
						Main.dust[index].fadeIn = 1.3f;
						Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
						vector2_1.Normalize();
						Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
						Main.dust[index].velocity = vector2_2;
						vector2_2.Normalize();
						Vector2 vector2_3 = vector2_2 * 34f;
						Main.dust[index].position = NPC.Center - vector2_3;
					}
				}
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(BuffID.Darkness, 120, true);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 40; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.ShadowbeamStaff, hitDirection * 6f, -1f, 0, default, .45f);
			}
			if (NPC.life <= 0) {
				for (int k = 0; k < 10; k++) {
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 99);
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 99);
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 99);
					Dust.NewDust(NPC.GetSource_Death(), NPC.position, NPC.width, NPC.height, DustID.ShadowbeamStaff, hitDirection * 6f, -1f, 0, default, 1f);
				}
			}
		}

	}
}
