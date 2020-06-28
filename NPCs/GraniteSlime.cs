using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.NPCs
{
	public class GraniteSlime : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Granite Slime");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BlueSlime];
		}

		public override void SetDefaults()
		{
			npc.width = 16;
			npc.height = 12;
			npc.damage = 22;
			npc.defense = 8;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.buffImmune[BuffID.Venom] = true;
			npc.lifeMax = 85;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath43;
			npc.value = 200f;
			npc.knockBackResist = .25f;
			npc.aiStyle = 1;
			aiType = NPCID.BlueSlime;
			animationType = NPCID.BlueSlime;
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
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/GraniteSlime_Glow"));
		}
		bool jump;
		public override void AI()
		{
			Player target = Main.player[npc.target];
			int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));
			bool expertMode = Main.expertMode;
			npc.direction = npc.spriteDirection;
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.12f, 0.29f, .42f);
			if(!npc.collideY) {
				jump = true;
			}
			if(npc.collideY && jump && Main.rand.Next(3) == 0) {
				for(int i = 0; i < 20; i++) {
					int num = Dust.NewDust(npc.position, npc.width, npc.height, 226, 0f, -2f, 0, default(Color), 2f);
					Main.dust[num].noGravity = true;
					Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].scale *= .25f;
					if(Main.dust[num].position != npc.Center)
						Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 6f;
				}
				jump = false;
				Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 110));
				int damage = expertMode ? 11 : 21;
				if(distance < 92) {
					target.AddBuff(BuffID.Confused, 180);
					target.AddBuff(BuffID.Shine, 180);
				}
				for(int i = 0; i < 2; i++) {
					float rotation = (float)(Main.rand.Next(0, 361) * (Math.PI / 180));
					Vector2 velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
					int proj = Projectile.NewProjectile(npc.Center.X, npc.Center.Y,
						velocity.X, velocity.Y, mod.ProjectileType("GraniteShard1"), 13, 1, Main.myPlayer, 0, 0);
					Main.projectile[proj].friendly = false;
					Main.projectile[proj].hostile = true;
					Main.projectile[proj].velocity *= 4f;
				}
			}
		}
		public override void NPCLoot()
		{
			if(Main.rand.Next(2) == 0) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<GraniteChunk>(), 1);
			}
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Gel, Main.rand.Next(1, 3) + 1);
			if(Main.rand.Next(1000) == 33) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.NightVisionHelmet);
			}
			if(Main.rand.Next(10000) == 0) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.SlimeStaff);
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			int x = spawnInfo.spawnTileX;
			int y = spawnInfo.spawnTileY;
			int tile = (int)Main.tile[x, y].type;
			return (tile == 368) && NPC.downedBoss2 && spawnInfo.spawnTileY > Main.rockLayer ? 0.1f : 0f;

		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if(npc.life <= 0) {
				int d = 226;
				for(int k = 0; k < 20; k++) {
					Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.27f);
					Dust.NewDust(npc.position, npc.width, npc.height, 240, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.87f);
				}
			}
			if(Main.netMode != 1 && npc.life <= 0 && Main.rand.Next(3) == 0) {
				Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 109));
				{
					for(int i = 0; i < 20; i++) {
						int num = Dust.NewDust(npc.position, npc.width, npc.height, 226, 0f, -2f, 0, default(Color), 2f);
						Main.dust[num].noGravity = true;
						Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].scale *= .25f;
						if(Main.dust[num].position != npc.Center)
							Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 6f;
					}
					Vector2 spawnAt = npc.Center + new Vector2(0f, (float)npc.height / 2f);
					NPC.NewNPC((int)spawnAt.X, (int)spawnAt.Y, ModContent.NPCType<GraniteCore>());
				}
			}
		}
	}
}
