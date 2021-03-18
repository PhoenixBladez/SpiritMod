using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Hostile;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable.Food;

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
            npc.aiStyle = -1;
			npc.knockBackResist = 0f;
			npc.behindTiles = true;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.ArterialGrasperBanner>();
		}

		bool spawnedHooks = false;
		//bool attack = false;
		public override void AI()
        {
            npc.TargetClosest(false);
            if (npc.localAI[0] == 0f) {
				npc.localAI[0] = npc.Center.Y;
				npc.netUpdate = true; //localAI probably isnt affected by this... buuuut might as well play it safe
			}
			if (npc.Center.Y >= npc.localAI[0]) {
				npc.localAI[1] = -1f;
				npc.netUpdate = true;
			}
			if (npc.Center.Y <= npc.localAI[0] - 2f) {
				npc.localAI[1] = 1f;
				npc.netUpdate = true;
			}
			npc.velocity.Y = MathHelper.Clamp(npc.velocity.Y + 0.009f * npc.localAI[1], -.85f, .85f);
			if (!spawnedHooks) {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < Main.rand.Next(2, 4); i++)
                    {
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 10, Main.rand.Next(-10, 10), -6, ModContent.ProjectileType<TendonEffect>(), 0, 0, Main.myPlayer, 0, npc.whoAmI);
                    }
                    for (int i = 0; i < Main.rand.Next(2, 3); i++)
                    {
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 10, Main.rand.Next(-10, 10), -6, ModContent.ProjectileType<TendonEffect1>(), 0, 0, Main.myPlayer, 0, npc.whoAmI);
                    }
                }
				spawnedHooks = true;
                npc.netUpdate = true;
			}
			npc.spriteDirection = -npc.direction;
			Player target = Main.player[npc.target];
			int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));

			if (distance < 560) {
				float num395 = Main.mouseTextColor / 200f - 0.35f;
				num395 *= 0.2f;
				npc.scale = num395 + 0.95f;
				//attack = true;
				npc.ai[2]++;
				if (npc.ai[2] == 30 || npc.ai[2] == 60 || npc.ai[2] == 90 || npc.ai[2] == 120 || npc.ai[2] == 150) {
					Lighting.AddLight((int)(npc.Center.X / 16f), (int)(npc.Center.Y / 16f), .153f * 1, .028f * 1, 0.055f * 1);
					Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/HeartbeatFx"));
				}
				if (npc.ai[2] >= 180) {
					Main.PlaySound(SoundID.Item, npc.Center, 95);
					npc.ai[2] = 0;
					Lighting.AddLight((int)(npc.Center.X / 16f), (int)(npc.Center.Y / 16f), .153f * 1, .028f * 1, 0.055f * 1);
					Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/HeartbeatFx"));
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            float rotation = (float)(Main.rand.Next(0, 361) * (Math.PI / 180));
                            Vector2 velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
                            int proj = Projectile.NewProjectile(npc.Center.X, npc.Center.Y,
                                velocity.X, velocity.Y, ModContent.ProjectileType<ArterialBloodClump>(), 12, 1, Main.myPlayer, 0, 0);
                            Main.projectile[proj].friendly = false;
                            Main.projectile[proj].hostile = true;
                            Main.projectile[proj].velocity *= 5f;
                        }
                    }
				}
			}
			if (distance == 580)
            {
                npc.netUpdate = true;
            }
			if (distance > 580) {
				float num395 = Main.mouseTextColor / 200f - 0.35f;
				num395 *= 0.2f;
				npc.scale = num395 + 0.95f;
				//attack = false;
				npc.ai[2]++;
				if (npc.ai[2] >= 90) {
					npc.ai[2] = 0;
                    npc.netUpdate = true;
					Lighting.AddLight((int)(npc.Center.X / 16f), (int)(npc.Center.Y / 16f), .153f * .5f, .028f * .5f, 0.055f * .5f);
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
			=> Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY].wall > 0
			&& spawnInfo.player.ZoneCrimson
			&& (spawnInfo.player.ZoneRockLayerHeight || spawnInfo.player.ZoneDirtLayerHeight)
			? SpawnCondition.Crimson.Chance * 0.2f : 0f;

		public override void HitEffect(int hitDirection, double damage)
		{
			int d = 5;
			int d1 = 5;
			for (int k = 0; k < 30; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.Purple, 0.3f);
				Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, default, .34f);
			}
			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Grasper/Grasper1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Grasper/Grasper2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Grasper/Grasper3"), 1f);
			}
        }
        public override void NPCLoot()
        {
            if (Main.rand.NextBool(3)) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Vertebrae);
            }
            if (Main.rand.NextBool(33))
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Weapon.Summon.HeartilleryBeacon>());
            }
            if (Main.rand.NextBool(16))
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Meatballs>());
            }
        }
	}
}
