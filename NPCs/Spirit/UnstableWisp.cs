using Microsoft.Xna.Framework;
using SpiritMod.Tiles.Block;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace SpiritMod.NPCs.Spirit
{
	public class UnstableWisp : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unstable Wisp");
			Main.npcFrameCount[NPC.type] = 4;
		}

		public override void SetDefaults()
		{
			NPC.width = 32;
			NPC.height = 32;
			NPC.lifeMax = 150;
			NPC.knockBackResist = 0f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.friendly = false;
			NPC.HitSound = SoundID.NPCHit3;
			NPC.DeathSound = SoundID.NPCDeath6;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.Player;
			if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.SpawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.SpawnTileY > Main.worldSurface || !Main.dayTime) && (SpawnCondition.GoblinArmy.Chance == 0)) {
				int[] TileArray2 = { ModContent.TileType<SpiritDirt>(), ModContent.TileType<SpiritStone>(), ModContent.TileType<Spiritsand>(), ModContent.TileType<SpiritGrass>(), ModContent.TileType<SpiritIce>(), };
				return TileArray2.Contains(Main.tile[spawnInfo.SpawnTileX, spawnInfo.SpawnTileY].TileType) && NPC.downedMechBossAny && spawnInfo.SpawnTileY > Main.rockLayer && player.position.Y / 16 < (Main.rockLayer + Main.maxTilesY - 330) / 2f && !spawnInfo.PlayerSafe && !spawnInfo.Invasion ? 2f : 0f;

			}
			return 0f;
		}

		public override bool PreAI()
		{
			bool inRange = false;
			Vector2 target = Vector2.Zero;
			float triggerRange = 280f;
			for (int i = 0; i < 255; i++) {
				if (Main.player[i].active && !Main.player[i].dead) {
					float playerX = Main.player[i].position.X + (float)(Main.player[i].width / 2);
					float playerY = Main.player[i].position.Y + (float)(Main.player[i].height / 2);
					float distOrth = Math.Abs(NPC.position.X + (float)(NPC.width / 2) - playerX) + Math.Abs(NPC.position.Y + (float)(NPC.height / 2) - playerY);
					if (distOrth < triggerRange) {
						if (Main.player[i].Hitbox.Intersects(NPC.Hitbox)) {
							NPC.life = 0;
							NPC.HitEffect(0, 10.0);
							NPC.checkDead();
							NPC.active = false;
							return false;
						}
						triggerRange = distOrth;
						target = Main.player[i].Center;
						inRange = true;
					}
				}
			}
			if (inRange) {
				Vector2 delta = target - NPC.Center;
				delta.Normalize();
				delta *= 0.95f;
				NPC.velocity = (NPC.velocity * 10f + delta) * (1f / 11f);
				return false;
			}
			if (NPC.velocity.Length() > 0.2f) {
				NPC.velocity *= 0.98f;
			}
			return false;
		}

		public override bool CheckDead()
		{
			Vector2 center = NPC.Center;
			Projectile.NewProjectile(NPC.GetSource_Death(), center.X, center.Y, 0f, 0f, ModContent.ProjectileType<UnstableWisp_Explosion>(), 100, 0f, Main.myPlayer);
			return true;
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.1f;
			if (NPC.frameCounter >= Main.npcFrameCount[NPC.type])
				NPC.frameCounter -= Main.npcFrameCount[NPC.type];
			int num = (int)NPC.frameCounter;
			NPC.frame.Y = num * frameHeight;
			NPC.spriteDirection = NPC.direction;
		}
	}
}
