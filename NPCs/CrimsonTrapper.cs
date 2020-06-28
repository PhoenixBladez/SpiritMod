using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Hostile;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class CrimsonTrapper : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arterial Grasper");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 34;
			npc.height = 34;
			npc.damage = 35;
			npc.defense = 8;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.lifeMax = 110;
			npc.noGravity = true;
			npc.HitSound = SoundID.NPCHit19;
			npc.DeathSound = new Terraria.Audio.LegacySoundStyle(42, 39);
			npc.value = 220f;
			npc.knockBackResist = 0f;
			npc.behindTiles = true;
		}
		bool spawnedHooks = false;
		bool attack = false;
		public override void AI()
		{
			if(npc.localAI[0] == 0f) {
				npc.localAI[0] = npc.Center.Y;
				npc.netUpdate = true; //localAI probably isnt affected by this... buuuut might as well play it safe
			}
			if(npc.Center.Y >= npc.localAI[0]) {
				npc.localAI[1] = -1f;
				npc.netUpdate = true;
			}
			if(npc.Center.Y <= npc.localAI[0] - 2f) {
				npc.localAI[1] = 1f;
				npc.netUpdate = true;
			}
			npc.velocity.Y = MathHelper.Clamp(npc.velocity.Y + 0.009f * npc.localAI[1], -.85f, .85f);
			if(!spawnedHooks) {
				for(int i = 0; i < Main.rand.Next(2, 4); i++) {
					Terraria.Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 10, Main.rand.Next(-10, 10), -6, ModContent.ProjectileType<TendonEffect>(), 0, 0);
				}
				for(int i = 0; i < Main.rand.Next(2, 3); i++) {
					Terraria.Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 10, Main.rand.Next(-10, 10), -6, ModContent.ProjectileType<TendonEffect1>(), 0, 0);
				}
				spawnedHooks = true;
			}
			npc.spriteDirection = -npc.direction;
			Player target = Main.player[npc.target];
			int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));

			if(distance < 480) {
				float num395 = Main.mouseTextColor / 200f - 0.35f;
				num395 *= 0.2f;
				npc.scale = num395 + 0.95f;
				attack = true;
				npc.ai[2]++;
				if(npc.ai[2] == 30) {

					Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), .153f * 1, .028f * 1, 0.055f * 1);
					Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/HeartbeatFx"));
				}
				if(npc.ai[2] >= 60) {
					Main.PlaySound(2, npc.Center, 95);
					npc.ai[2] = 0;
					Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), .153f * 1, .028f * 1, 0.055f * 1);
					Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/HeartbeatFx"));
					for(int i = 0; i < 5; i++) {
						float rotation = (float)(Main.rand.Next(0, 361) * (Math.PI / 180));
						Vector2 velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
						int proj = Projectile.NewProjectile(npc.Center.X, npc.Center.Y,
							velocity.X, velocity.Y, mod.ProjectileType("ArterialBloodClump"), 12, 1, Main.myPlayer, 0, 0);
						Main.projectile[proj].friendly = false;
						Main.projectile[proj].hostile = true;
						Main.projectile[proj].velocity *= 5f;
					}
				}
			}
			if(distance > 490) {
				float num395 = Main.mouseTextColor / 200f - 0.35f;
				num395 *= 0.2f;
				npc.scale = num395 + 0.95f;
				attack = false;
				npc.ai[2]++;
				if(npc.ai[2] >= 90) {
					npc.ai[2] = 0;
					Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), .153f * .5f, .028f * .5f, 0.055f * .5f);
					Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/HeartbeatFx"));
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
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			int x = spawnInfo.spawnTileX;
			int y = spawnInfo.spawnTileY;
			int tile = (int)Main.tile[x, y].type;
			return (tile == TileID.Crimstone) && spawnInfo.player.ZoneCrimson && spawnInfo.player.ZoneRockLayerHeight && !NPC.AnyNPCs(ModContent.NPCType<CrimsonTrapper>()) ? 2f : 0f;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			int d = 5;
			int d1 = 5;
			for(int k = 0; k < 30; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.Purple, 0.3f);
				Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, default(Color), .34f);
			}
			if(npc.life <= 0) {
				{
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Grasper/Grasper1"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Grasper/Grasper2"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Grasper/Grasper3"), 1f);
				}
			}
		}
	}
}
