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
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Buffs;
using Terraria.ModLoader.Utilities;

namespace SpiritMod.NPCs.Mechromancer
{
	[AutoloadBossHead]
	public class Mecromancer : ModNPC, IBCRegistrable
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mechromancer");
			Main.npcFrameCount[NPC.type] = 17;
			NPCID.Sets.TrailCacheLength[NPC.type] = 3;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
		}

		int moveSpeed = 0;
		int moveSpeedY = 0;
		float HomeY = 150f;

		public override void SetDefaults()
		{
			NPC.width = 32;
			NPC.height = 48;
			NPC.rarity = 3;
			NPC.damage = 20;
			NPC.defense = 8;
			NPC.lifeMax = 270;
			NPC.HitSound = SoundID.NPCHit40;
			NPC.DeathSound = SoundID.NPCDeath2;
			NPC.buffImmune[BuffID.OnFire] = true;
			NPC.buffImmune[BuffID.Confused] = true;
			NPC.buffImmune[ModContent.BuffType<ElectrifiedV2>()] = true;
			NPC.value = Item.buyPrice(0, 1, 38, 58);
			NPC.knockBackResist = 0.1f;
			NPC.noTileCollide = false;
			AnimationType = 471;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.MechromancerBanner>();
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => NPC.AnyNPCs(ModContent.NPCType<Mecromancer>()) ? 0 : SpawnCondition.GoblinArmy.Chance * 0.0266f;

		public override void OnKill()
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
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddOneFromOptions(1, ModContent.ItemType<KnocbackGun>(), ModContent.ItemType<Items.Accessory.UnstableTeslaCoil.Unstable_Tesla_Coil>());
			npcLoot.AddCommon(ItemID.RocketBoots, 25);
			npcLoot.AddCommon<TechDrive>(1, 7, 11);
		}

		int timer;
		bool flying;

		public override void AI()
		{
			if (Main.rand.Next(250) == 2)
				SoundEngine.PlaySound(SoundID.Zombie7, NPC.Center);

			timer++;
			if (timer == 100 || timer == 300)
			{
				SoundEngine.PlaySound(SoundID.Zombie7, NPC.Center);
				SoundEngine.PlaySound(SoundID.Item92, NPC.Center);
				NPC.TargetClosest();
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
					direction.Normalize();
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, 0, -6, ModContent.ProjectileType<MechBat>(), 11, 0);
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, 3, -6, ModContent.ProjectileType<MechBat>(), 11, 0);
					if (Main.rand.Next(3) == 0)
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, -3, -6, ModContent.ProjectileType<MechBat>(), 11, 0);
				}
			}

			if (timer > 420 && timer < 840) {
				NPC.noTileCollide = true;
				if (Main.rand.Next(60) == 0 && Main.netMode != NetmodeID.MultiplayerClient)
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.position.X, NPC.position.Y + 40, 0, 1, ProjectileID.GreekFire1, (int)(NPC.damage * 0.5f), 0);

				Player player = Main.player[NPC.target];
				if (Main.rand.Next(20) == 0)
					SoundEngine.PlaySound(SoundID.Item13, NPC.position);

				NPC.noGravity = true;

				if (NPC.Center.X >= player.Center.X && moveSpeed >= -40) // flies to players x position
					moveSpeed--;
				if (NPC.Center.X <= player.Center.X && moveSpeed <= 40)
					moveSpeed++;

				NPC.velocity.X = moveSpeed * 0.1f;

				if (NPC.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -30) //Flies to players Y position
				{
					moveSpeedY--;
					HomeY = 185f;
				}
				if (NPC.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 30)
					moveSpeedY++;

				NPC.direction = player.Center.X > NPC.Center.X ? 1 : -1;
				NPC.spriteDirection = NPC.direction;
				NPC.velocity.Y = moveSpeedY * 0.1f;
				flying = true;
				
				int num220 = Dust.NewDust(new Vector2(NPC.Center.X + 2f, NPC.position.Y + NPC.height - 10f), 8, 8, DustID.Torch, 0f, 0f, 100, default, 1.95f);
				Main.dust[num220].noGravity = true;
				Main.dust[num220].velocity.X = Main.dust[num220].velocity.X * 1f - 2f - NPC.velocity.X * 0.3f;
				Main.dust[num220].velocity.Y = Main.dust[num220].velocity.Y * 1f + 2f * -NPC.velocity.Y * 0.3f;
				int num221 = Dust.NewDust(new Vector2(NPC.Center.X - 3f, NPC.position.Y + NPC.height - 10f), 8, 8, DustID.Torch, 0f, 0f, 100, default, 1.95f);
				Main.dust[num221].noGravity = true;
				Main.dust[num221].velocity.X = Main.dust[num220].velocity.X * 1f - 2f - NPC.velocity.X * 0.3f;
				Main.dust[num221].velocity.Y = Main.dust[num220].velocity.Y * 1f + 2f * -NPC.velocity.Y * 0.3f;
			}
			else
			{
				NPC.noTileCollide = false;
				flying = false;
				NPC.rotation = 0f;
				NPC.noGravity = false;
				NPC.aiStyle = 3;
				AIType = NPCID.GoblinThief;
			}

			if (timer >= 840)
			{
				int damage = Main.expertMode ? 13 : 22;
				timer = 0;
				SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/CoilRocket"), NPC.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					int add = Main.rand.Next(1, 4);
					for (int i = 0; i < 4 + add; i++)
					{
						Vector2 vel = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center) * 2;
						float xAdj = Main.rand.Next(-50, 50) * 0.23f;
						float yAdj = Main.rand.Next(-50, 50) * 0.23f;
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + Main.rand.Next(-50, 50), NPC.Center.Y + Main.rand.Next(-50, 50), vel.X + xAdj, vel.Y + yAdj, ModContent.ProjectileType<CoilRocket>(), damage, 1, Main.myPlayer, 0, 0);
					}
				}
			}
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (flying)
			{
				var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
				var drawOrigin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width * 0.5f, (NPC.height / Main.npcFrameCount[NPC.type]) * 0.5f);
				for (int k = 0; k < NPC.oldPos.Length; k++)
				{
					Vector2 drawPos = NPC.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY);
					Color color = NPC.GetAlpha(drawColor) * ((NPC.oldPos.Length - k) / NPC.oldPos.Length / 2f);
					spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawPos, NPC.frame, color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
				}
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
				for (int i = 1; i < 7; ++i)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Mech" + i).Type, 1f);
		}

		public override void FindFrame(int frameHeight)
		{
			if (flying)
				NPC.frame.Y = frameHeight * 10;
		}

        public override bool PreKill()
        {
            MyWorld.downedMechromancer = true;
            SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/DownedMiniboss"), NPC.Center);
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
