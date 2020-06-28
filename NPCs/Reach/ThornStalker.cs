using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Hostile;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Reach
{
	public class ThornStalker : ModNPC
	{
		bool attack = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thorn Stalker");
			Main.npcFrameCount[npc.type] = 13;
		}

		public override void SetDefaults()
		{
			npc.width = 48;
			npc.height = 58;
			npc.damage = 15;
			npc.defense = 6;
			npc.lifeMax = 150;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath16;
			npc.value = 600f;
			npc.knockBackResist = .35f;
		}
		int frame = 0;
		int timer = 0;
		int shootTimer = 0;
		public override void AI()
		{
			npc.spriteDirection = npc.direction;
			Player target = Main.player[npc.target];
			shootTimer++;
			if(shootTimer % 200 == 150) {
				attack = true;
			}
			if(attack) {
				npc.velocity.X = .008f * npc.direction;
				//shootTimer++;
				if(frame == 11 && timer == 0) {
					// Main.PlaySound(2, npc.Center, 95);
					for(int i = 0; i < 2; i++) {
						int knife = Terraria.Projectile.NewProjectile(npc.Center.X + Main.rand.Next(-50, 50), npc.Center.Y - Main.rand.Next(60), 0, 0, ModContent.ProjectileType<ThornKnife>(), npc.damage, 0);
						Projectile p = Main.projectile[knife];
						Vector2 direction = Main.player[npc.target].Center - p.Center;
						direction.Normalize();
						direction *= Main.rand.NextFloat(7, 10);
						p.velocity = direction;
						Main.PlaySound(2, new Vector2(npc.Center.X + Main.rand.Next(-50, 50), npc.Center.Y - Main.rand.Next(60)), 1);
						Main.PlaySound(2, new Vector2(npc.Center.X + Main.rand.Next(-50, 50), npc.Center.Y - Main.rand.Next(60)), 1);

					}
					timer++;
				}
				timer++;
				if(timer >= 11) {
					frame++;
					timer = 0;
				}
				if(frame > 12) {
					attack = false;
					frame = 12;
				}
				if(frame < 7) {
					frame = 7;
				}
				if(target.position.X > npc.position.X) {
					npc.direction = 1;
				} else {
					npc.direction = -1;
				}
			} else {
				//shootTimer = 0;
				npc.aiStyle = 3;
				aiType = NPCID.WalkingAntlion;
				timer++;
				if(timer >= 7) {
					frame++;
					timer = 0;
				}
				if(frame > 6) {
					frame = 1;
				}
			}
			if(!attack && !npc.collideY && npc.velocity.Y > 0) {
				frame = 0;
			}
			/*if (shootTimer > 120)
            {
                shootTimer = 120;
            }
            if (shootTimer < 0)
            {
                shootTimer = 0;
            }*/
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frame.Y = frameHeight * frame;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.player;
			if(!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.spawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.spawnTileY > Main.worldSurface || !Main.dayTime) && (SpawnCondition.GoblinArmy.Chance == 0)) {
				return spawnInfo.player.GetSpiritPlayer().ZoneReach ? 0.2f : 0f;
			}
			return 0f;
		}
	}
}
