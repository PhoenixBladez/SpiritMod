using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Armor.AstronautVanity;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Buffs;
using SpiritMod.Projectiles.Hostile;
using System;
using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Buffs.DoT;

namespace SpiritMod.NPCs.Shockhopper
{
	public class DeepspaceHopper : ModNPC
	{
		private const int TELEPORT_DISTANCE = 300;

		private int Timer {
			get => (int)NPC.ai[1];
			set => NPC.ai[1] = value;
		}

		private Vector2 AngleToPlayer {
			get => new Vector2(NPC.localAI[0], NPC.localAI[1]);
			set {
				NPC.localAI[0] = value.X;
				NPC.localAI[1] = value.Y;
			}
		}

		private AIState State {
			get => (AIState)(int)NPC.ai[0];
			set {
				NPC.ai[0] = (int)value;
				if(Main.netMode != NetmodeID.MultiplayerClient) {
					NPC.netUpdate = true;
				}
			}
		}

		private enum AIState
		{
			STANDBY,
			AIMING,
			SHOOTING,
			TELEPORT_FAIL,
			TELEPORT_SUCCESS
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shockhopper");
			Main.npcFrameCount[NPC.type] = 1;
		}

		public override void SetDefaults()
		{
			NPC.width = 24;
			NPC.height = 24;
			NPC.damage = 0;
			NPC.defense = 6;
			NPC.lifeMax = 55;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.value = 130f;
			NPC.knockBackResist = 1f;
			NPC.aiStyle = -1;
			NPC.noGravity = true;
			NPC.noTileCollide = false;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.ShockhopperBanner>();
			NPC.buffImmune[ModContent.BuffType<FesteringWounds>()] = true;
			NPC.buffImmune[ModContent.BuffType<ElectrifiedV2>()] = true;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[BuffID.Confused] = true;
			// start with 5 seconds to the first teleport
			Timer = 300;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 12; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, 2.5f * hitDirection, -2.5f, 0, default, .34f);
			}
			if (NPC.life <= 0) {
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Hopper1").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Hopper2").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Hopper3").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Hopper4").Type);
				for (int i = 0; i < 15; i++) {
					Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Electric, 0f, -2f, 0, default, 0.4f);
					dust.noGravity = true;
					dust.position.X += (Main.rand.Next(-50, 51) / 20) - 1.5f;
					dust.position.Y += (Main.rand.Next(-50, 51) / 20) - 1.5f;
					if (dust.position != NPC.Center) {
						dust.velocity = NPC.DirectionTo(dust.position) * 3f;
					}
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame,
							 drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (NPC.alpha != 255)
				GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.Assets.Request<Texture2D>("NPCs/Shockhopper/DeepspaceHopper_Glow").Value);
		}

		public override bool PreAI()
		{
			NPC.velocity.X = 0;
			NPC.velocity.Y = 0;
			NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];

			Timer++;
			// Update NPC state
			// Standby mode; doing nothing, just looking at player
			if (Timer < 100) State = AIState.STANDBY;
			// Aiming mode; preparing to shoot, look angle locked
			else if (Timer == 100) State = AIState.AIMING;
			// Shooting mode; shoots the laser
			else if (Timer == 130) State = AIState.SHOOTING;
			// Teleport mode; performs a teleport
			else if (Timer >= 280)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Vector2 angle = Vector2.UnitX.RotateRandom(Math.PI * 2);
					NPC.position.X = player.Center.X + (int)(TELEPORT_DISTANCE * angle.X);
					NPC.position.Y = player.Center.Y + (int)(TELEPORT_DISTANCE * angle.Y);
					NPC.netUpdate = true;
					if (Main.tile[(int)(NPC.position.X / 16), (int)(NPC.position.Y / 16)].HasTile)
					{
						State = AIState.TELEPORT_FAIL;
					}
					else
					{
						State = AIState.TELEPORT_SUCCESS;
					}
				}
			}

			// Look at the player
			if (State == AIState.STANDBY)
			{
				NPC.rotation = NPC.DirectionTo(player.Center).ToRotation() - MathHelper.PiOver2;
			}

			if (State == AIState.TELEPORT_FAIL)
			{
				NPC.alpha = 255;
			}

			// When we succeed at performing the teleport
			if (State == AIState.TELEPORT_SUCCESS)
			{
				Timer = 0;
				NPC.alpha = 0;
				if (Main.netMode != NetmodeID.Server)
				{
					SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
					for (int i = 0; i < 50; i++)
					{
						Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Electric, 0f, -2f, 0, default, 0.4f);
						dust.noGravity = true;
						dust.position.X += (Main.rand.Next(-50, 51) / 20) - 1.5f;
						dust.position.Y += (Main.rand.Next(-50, 51) / 20) - 1.5f;
						if (dust.position != NPC.Center)
						{
							dust.velocity = NPC.DirectionTo(dust.position) * 3f;
						}
					}
				}
				State = AIState.STANDBY;
			}

			// Lock in the targeting angle
			if (State == AIState.AIMING && Timer == 100)
			{
				SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
				var offset = player.Center - NPC.Center;
				offset.Normalize();
				AngleToPlayer = offset;
			}

			// Spawn dust while waiting to shoot
			if (Main.netMode != NetmodeID.Server && State == AIState.AIMING)
			{
				Dust dust = Dust.NewDustDirect(NPC.Center, NPC.width, NPC.height, DustID.Electric);
				dust.velocity *= -1f;
				dust.scale *= .8f;
				dust.noGravity = true;
				Vector2 vector2_1 = new Vector2(Main.rand.Next(-80, 81), Main.rand.Next(-80, 81));
				vector2_1.Normalize();
				Vector2 vector2_2 = vector2_1 * (Main.rand.Next(50, 100) * 0.04f);
				dust.velocity = vector2_2;
				vector2_2.Normalize();
				Vector2 vector2_3 = vector2_2 * 34f;
				dust.position = NPC.Center - vector2_3;
			}

			// Fire the laser
			if (State == AIState.SHOOTING)
			{
				// Play sound on client, fire projectile on server
				SoundEngine.PlaySound(SoundID.Item91, NPC.Center);
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, AngleToPlayer * 30, ModContent.ProjectileType<HopperLaser>(), 19, 1, Main.myPlayer);
				}
				State = AIState.STANDBY;
			}

			// Idle dusts while we're not failing a teleport
			if (Main.netMode != NetmodeID.Server && State != AIState.TELEPORT_FAIL)
			{
				if (Main.rand.NextBool())
				{
					// You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
					Vector2 position = new Vector2(NPC.Center.X - 10, NPC.Center.Y).RotatedBy(NPC.rotation, NPC.Center);
					var dust = Dust.NewDustPerfect(position, 226, new Vector2(0f, -6.421053f).RotatedBy(NPC.rotation), 0, new Color(255, 0, 0), 0.6578947f);
					dust.noGravity = true;
				}
				if (Main.rand.NextBool())
				{
					// You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
					Vector2 position = new Vector2(NPC.Center.X + 10, NPC.Center.Y).RotatedBy(NPC.rotation, NPC.Center);
					var dust = Dust.NewDustPerfect(position, 226, new Vector2(0f, -6.421053f).RotatedBy(NPC.rotation), 0, new Color(255, 0, 0), 0.6578947f);
					dust.noGravity = true;
				}
			}

			return false;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon(ModContent.ItemType<GateStaff>(), 40);
			npcLoot.AddCommon(ModContent.ItemType<GravityModulator>(), 400);
			npcLoot.AddOneFromOptions(40, ModContent.ItemType<AstronautHelm>(), ModContent.ItemType<AstronautBody>(), ModContent.ItemType<AstronautLegs>());
		}
	}
}
