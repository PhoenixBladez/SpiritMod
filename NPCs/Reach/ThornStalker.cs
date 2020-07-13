using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Hostile;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Flail;
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
            NPCID.Sets.TrailCacheLength[npc.type] = 3;
            NPCID.Sets.TrailingMode[npc.type] = 0;
        }

		public override void SetDefaults()
		{
			npc.width = 48;
			npc.height = 58;
			npc.damage = 15;
			npc.defense = 8;
			npc.lifeMax = 70;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 90f;
			npc.knockBackResist = .35f;
		}
		int frame = 0;
		int timer = 0;
		int shootTimer = 0;
        public override void HitEffect(int hitDirection, double damage)
        {
            Main.PlaySound(3, npc.Center, 7);
            for (int k = 0; k < 11; k++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, 167, hitDirection, -1f, 0, Color.Green, .61f);
            }
            if (npc.life <= 0)
            {
                Main.PlaySound(SoundID.Zombie, npc.Center, 7);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ThornStalker/ThornStalker1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ThornStalker/ThornStalker2"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ThornStalker/ThornStalker3"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ThornStalker/ThornStalker4"), 1f);
            }
        }

        public override void AI()
		{
            Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.024f, 0.088f, 0.026f);
            npc.spriteDirection = npc.direction;
			Player target = Main.player[npc.target];
			shootTimer++;
			if(shootTimer % 200 == 150) {
				attack = true;
			}
			if(attack) {
				npc.velocity.Y = 6;
				npc.velocity.X = .008f * npc.direction;
				//shootTimer++;
				if(frame == 11 && timer == 0) {
					 Main.PlaySound(SoundID.Item, npc.Center, 95);
					for(int i = 0; i < 2; i++) {
						int knife = Terraria.Projectile.NewProjectile(npc.Center.X + Main.rand.Next(-50, 50), npc.Center.Y - Main.rand.Next(60), 0, 0, ModContent.ProjectileType<ThornKnife>(), npc.damage / 4, 0);
						Projectile p = Main.projectile[knife];
						Vector2 direction = Main.player[npc.target].Center - p.Center;
						direction.Normalize();
						direction *= Main.rand.NextFloat(7, 10);
						p.velocity = direction;
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
				if(timer >= 6) {
					frame++;
					timer = 0;
				}
				if(frame > 6) {
					frame = 1;
				}
				if (Main.rand.Next(300) == 0)
				{
					Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/StalkerIdle"));
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
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height * 0.5f));
            for (int k = 0; k < npc.oldPos.Length; k++)
            {
                var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
                Color color = npc.GetAlpha(lightColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
                spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
            }
            return true;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
                GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Reach/ThornStalker_Glow"));
        }
        public override void NPCLoot()
		{
			if(Main.rand.Next(3) == 1) {
				int Bark = Main.rand.Next(2) + 1;
				for(int J = 0; J <= Bark; J++) {
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<AncientBark>());
				}
			}
			if(!Main.dayTime) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<EnchantedLeaf>());
			}
			if(Main.rand.Next(33) == 3) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<VineChain>());
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.player;
			if(!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon)) && (!Main.eclipse) && (SpawnCondition.GoblinArmy.Chance == 0)) {
				return spawnInfo.player.GetSpiritPlayer().ZoneReach ? 0.4f : 0f;
			}
			return 0f;
		}
	}
}
