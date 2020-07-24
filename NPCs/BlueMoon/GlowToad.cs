using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Magic;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.BlueMoon
{
	public class GlowToad : ModNPC
	{
		int timer = 0;

		float frameCounter = 0; //these only appear to be used for visuals so they should be fine clientsided
		int tongueproj = 0;

		internal ref float TongueOut => ref npc.ai[0];
		internal ref float TongueActive => ref npc.ai[1];
		internal ref float Cooldown => ref npc.ai[2];
		internal ref float Jumping => ref npc.ai[3];

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => MyWorld.BlueMoon ? 7f : 0f;

		public override void SendExtraAI(BinaryWriter writer) => writer.Write(timer);

		public override void ReceiveExtraAI(BinaryReader reader) => timer = reader.ReadInt32();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glow Toad");
			Main.npcFrameCount[npc.type] = 6;
		}

		public override void SetDefaults()
		{
			npc.width = 64;
			npc.height = 60;
			npc.damage = 49;
			npc.defense = 14;
			npc.lifeMax = 380;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath5;
			npc.value = 2000f;
			npc.knockBackResist = 0.5f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			Main.PlaySound(SoundID.Frog, (int)npc.position.X, (int)npc.position.Y);

			for(int k = 0; k < 5; k++)
				Dust.NewDust(npc.position, npc.width, npc.height, 180, hitDirection, -1f, 0, default(Color), 1f);

			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GlowToad/GlowToad1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GlowToad/GlowToad2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GlowToad/GlowToad3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GlowToad/GlowToad4"), 1f);
			}
		}

		public override void AI()
		{
			npc.TargetClosest(true);
			Player player = Main.player[npc.target];

			if(Math.Abs(player.position.X - npc.position.X) < 190 && npc.collideY) TongueOut = 1;

			if(TongueOut == 0 && Cooldown > 4) timer++;

			if(!npc.collideY) npc.velocity.X *= 1.045f;

			Cooldown++;

			if(timer % 90 == 0 && TongueOut == 0) { 		

				if (Jumping == 0) {
					Jumping = 1;
					Cooldown = 0;
				}

				Cooldown++;
				npc.velocity.Y = -7;

				if (player.position.X > npc.position.X) {
					npc.velocity.X = 10;
					npc.netUpdate = true;
				}
				else {
					npc.velocity.X = -10;
					npc.netUpdate = true;
				}
			}
			else Jumping = 0;

			if (TongueActive == 0) npc.spriteDirection = player.Center.X > npc.Center.X ? 0 : 1;
		}

		public override void FindFrame(int frameHeight)
		{
			Player player = Main.player[npc.target];
			if(!npc.collideY) {
				npc.frame.Y = 0;
				frameCounter = 0;
			}
			else if(TongueOut == 0) {
				npc.frame.Y = frameHeight;
				frameCounter = 0f;
			}

			if(TongueOut == 1) {

				if(TongueActive == 0) frameCounter += 0.11f;

				npc.frame.Y = ((int)(frameCounter % 5) + 1) * frameHeight;
				if(npc.frame.Y / frameHeight == 5 && TongueActive == 0) {
					npc.TargetClosest(true);
					TongueActive = 1;
					npc.knockBackResist = 0f;

					if(npc.spriteDirection == 0) {
						Main.PlaySound(SoundID.Frog, (int)npc.position.X, (int)npc.position.Y);
						tongueproj = Projectile.NewProjectile(npc.Center + new Vector2(21, 8), new Vector2(11, 0), ModContent.ProjectileType<GlowTongue>(), (int)(npc.damage / 1.7f), 1, player.whoAmI, 1);
					}
					else {
						Main.PlaySound(SoundID.Frog, (int)npc.position.X, (int)npc.position.Y);
						tongueproj = Projectile.NewProjectile(npc.Center + new Vector2(-18, 8), new Vector2(-11, 0), ModContent.ProjectileType<GlowTongue>(), (int)(npc.damage / 1.7f), 1, player.whoAmI, 0);
					}
				}

				if(TongueActive == 1) {
					Projectile tongue = Main.projectile[tongueproj];

					if(!tongue.active) {
						TongueActive = 0;
						TongueOut = 0;
						timer = 80;
						npc.knockBackResist = 0.5f;
					}
					else {
						int i = tongue.ai[0] == 0 ? -1 : 1;
						npc.direction = i;
						npc.spriteDirection = i;
					}
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Vector2 pos = npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY);

			spriteBatch.Draw(Main.npcTexture[npc.type], pos, npc.frame, drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/BlueMoon/GlowToad_Mask"));
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if(Main.rand.Next(5) == 0) target.AddBuff(ModContent.BuffType<StarFlame>(), 200);
		}

		public override void NPCLoot()
		{
			if(Main.rand.NextBool(5))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<MoonStone>());
			if(Main.rand.NextBool(100))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<StopWatch>());
		}
	}
}
