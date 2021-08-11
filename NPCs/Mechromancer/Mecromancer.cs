using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Armor;
using SpiritMod.Items.Sets.CoilSet;
using SpiritMod.Items.Sets.LaunchersMisc.Freeman;
using SpiritMod.Projectiles;
using SpiritMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Mechromancer
{
	[AutoloadBossHead]
	public class Mecromancer : ModNPC, IBCRegistrable
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mechromancer");
			Main.npcFrameCount[npc.type] = 17;
			NPCID.Sets.TrailCacheLength[npc.type] = 3;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		int moveSpeed = 0;
		int moveSpeedY = 0;
		float HomeY = 150f;

		public override void SetDefaults()
		{
			npc.width = 32;
			npc.height = 48;
			npc.rarity = 3;
			npc.damage = 20;
			npc.defense = 8;
			npc.lifeMax = 270;
			npc.HitSound = SoundID.NPCHit40;
			npc.DeathSound = SoundID.NPCDeath2;
			npc.value = Item.buyPrice(0, 1, 38, 58);
			npc.knockBackResist = 0.1f;
			npc.noTileCollide = false;
			animationType = 471;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.MechromancerBanner>();
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => NPC.AnyNPCs(ModContent.NPCType<Mecromancer>()) ? 0 : SpawnCondition.GoblinArmy.Chance * 0.0266f;

		public override void NPCLoot()
		{
			if (Main.invasionType == InvasionID.GoblinArmy)
			{
				Main.invasionSize -= 5;
				if (Main.invasionSize < 0)
					Main.invasionSize = 0;
				if (Main.netMode != NetmodeID.MultiplayerClient)
					Main.ReportInvasionProgress(Main.invasionSizeStart - Main.invasionSize, Main.invasionSizeStart, 4, 0);
				if (Main.netMode == NetmodeID.Server)
					NetMessage.SendData(MessageID.InvasionProgressReport, -1, -1, null, Main.invasionProgress, Main.invasionProgressMax, Main.invasionProgressIcon, 0f, 0, 0, 0);
			}

			if (Main.rand.NextBool(2))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<KnocbackGun>());
			else
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Accessory.UnstableTeslaCoil.Unstable_Tesla_Coil>());
			
			if (Main.rand.NextBool(25))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.RocketBoots);

			int[] lootTable = { ModContent.ItemType<CoiledMask>(), ModContent.ItemType<CoiledChestplate>(), ModContent.ItemType<CoiledLeggings>() };
			npc.DropItem(Main.rand.Next(lootTable));

			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<TechDrive>(), Main.rand.Next(7, 12));
		}

		int timer;
		bool flying;

		public override void AI()
		{
			if (Main.rand.Next(250) == 2)
				Main.PlaySound(SoundID.Zombie, (int)npc.position.X, (int)npc.position.Y, 7);

			timer++;
			if (timer == 100 || timer == 300)
			{
				Main.PlaySound(SoundID.Zombie, (int)npc.position.X, (int)npc.position.Y, 7);
				Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 92);
				npc.TargetClosest();
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Vector2 direction = Main.player[npc.target].Center - npc.Center;
					direction.Normalize();
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -6, ModContent.ProjectileType<MechBat>(), 11, 0);
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 3, -6, ModContent.ProjectileType<MechBat>(), 11, 0);
					if (Main.rand.Next(3) == 0)
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -3, -6, ModContent.ProjectileType<MechBat>(), 11, 0);
				}
			}

			if (timer > 420 && timer < 840) {
				npc.noTileCollide = true;
				if (Main.rand.Next(60) == 0 && Main.netMode != NetmodeID.MultiplayerClient)
					Projectile.NewProjectile(npc.position.X, npc.position.Y + 40, 0, 1, ProjectileID.GreekFire1, (int)(npc.damage * 0.5f), 0);

				Player player = Main.player[npc.target];
				if (Main.rand.Next(20) == 0)
					Main.PlaySound(SoundID.Item13, npc.position);

				npc.noGravity = true;

				if (npc.Center.X >= player.Center.X && moveSpeed >= -40) // flies to players x position
					moveSpeed--;
				if (npc.Center.X <= player.Center.X && moveSpeed <= 40)
					moveSpeed++;

				npc.velocity.X = moveSpeed * 0.1f;

				if (npc.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -30) //Flies to players Y position
				{
					moveSpeedY--;
					HomeY = 185f;
				}
				if (npc.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 30)
					moveSpeedY++;

				npc.direction = player.Center.X > npc.Center.X ? 1 : -1;
				npc.spriteDirection = npc.direction;
				npc.velocity.Y = moveSpeedY * 0.1f;
				flying = true;
				
				int num220 = Dust.NewDust(new Vector2(npc.Center.X + 2f, npc.position.Y + npc.height - 10f), 8, 8, DustID.Fire, 0f, 0f, 100, default, 1.95f);
				Main.dust[num220].noGravity = true;
				Main.dust[num220].velocity.X = Main.dust[num220].velocity.X * 1f - 2f - npc.velocity.X * 0.3f;
				Main.dust[num220].velocity.Y = Main.dust[num220].velocity.Y * 1f + 2f * -npc.velocity.Y * 0.3f;
				int num221 = Dust.NewDust(new Vector2(npc.Center.X - 3f, npc.position.Y + npc.height - 10f), 8, 8, DustID.Fire, 0f, 0f, 100, default, 1.95f);
				Main.dust[num221].noGravity = true;
				Main.dust[num221].velocity.X = Main.dust[num220].velocity.X * 1f - 2f - npc.velocity.X * 0.3f;
				Main.dust[num221].velocity.Y = Main.dust[num220].velocity.Y * 1f + 2f * -npc.velocity.Y * 0.3f;
			}
			else
			{
				npc.noTileCollide = false;
				flying = false;
				npc.rotation = 0f;
				npc.noGravity = false;
				npc.aiStyle = 3;
				aiType = NPCID.GoblinThief;
			}

			if (timer >= 840)
			{
				int damage = Main.expertMode ? 13 : 22;
				timer = 0;
				Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/CoilRocket"));

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					int add = Main.rand.Next(1, 4);
					for (int i = 0; i < 4 + add; i++)
					{
						Vector2 vel = Vector2.Normalize(Main.player[npc.target].Center - npc.Center) * 2;
						float xAdj = Main.rand.Next(-50, 50) * 0.23f;
						float yAdj = Main.rand.Next(-50, 50) * 0.23f;
						Projectile.NewProjectile(npc.Center.X + Main.rand.Next(-50, 50), npc.Center.Y + Main.rand.Next(-50, 50), vel.X + xAdj, vel.Y + yAdj, ModContent.ProjectileType<CoilRocket>(), damage, 1, Main.myPlayer, 0, 0);
					}
				}
			}
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (flying)
			{
				var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame, lightColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
				var drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height / Main.npcFrameCount[npc.type]) * 0.5f);
				for (int k = 0; k < npc.oldPos.Length; k++)
				{
					Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
					Color color = npc.GetAlpha(lightColor) * ((npc.oldPos.Length - k) / npc.oldPos.Length / 2f);
					spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, npc.frame, color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
				}
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
				for (int i = 1; i < 7; ++i)
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Mech" + i), 1f);
		}

		public override void FindFrame(int frameHeight)
		{
			if (flying)
				npc.frame.Y = frameHeight * 10;
		}

        public override bool PreNPCLoot()
        {
            MyWorld.downedMechromancer = true;
            Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/DownedMiniboss"));
            return true;
        }

        public void RegisterToChecklist(out BossChecklistDataHandler.EntryType entryType, out float progression,
	        out string name, out Func<bool> downedCondition, ref BossChecklistDataHandler.BCIDData identificationData,
	        ref string spawnInfo, ref string despawnMessage, ref string texture, ref string headTextureOverride,
	        ref Func<bool> isAvailable)
        {
	        entryType = BossChecklistDataHandler.EntryType.Miniboss;
	        progression = 2.8f;
	        name = "Mechromancer";
	        downedCondition = () => MyWorld.downedMechromancer;
	        identificationData = new BossChecklistDataHandler.BCIDData(
		        new List<int> {
			        ModContent.NPCType<Mecromancer>()
		        },
		        null,
		        new List<int> {
			        ModContent.ItemType<CoiledMask>(),
			        ModContent.ItemType<CoiledChestplate>(),
			        ModContent.ItemType<CoiledLeggings>()
		        },
		        new List<int> {
			        ModContent.ItemType<KnocbackGun>(),
			        ModContent.ItemType<TechDrive>(),
			        ItemID.RocketBoots
		        });
	        spawnInfo = "The Mechromancer spawns rarely during a Goblin Army after the Eye of Cthulhu has been defeated.";
	        texture = "SpiritMod/Textures/BossChecklist/MechromancerTexture";
	        headTextureOverride = "SpiritMod/NPCs/Mecromancer_Head_Boss";
        }
	}
}
