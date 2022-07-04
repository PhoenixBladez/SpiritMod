using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.IO;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;

namespace SpiritMod.NPCs.CavernBandit
{
	public class CavernBandit : ModNPC
	{
		private double _timer = 0;
		private bool _activated = false;
		private bool _pickedFrame = false;
		private int _frame = 0;
		private int _spriteDirection = 1; //Necessary due to fighter ai breaking if sprite direction is modified, somehow
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cavern Bandit");
			Main.npcFrameCount[NPC.type] = 16;
			NPCID.Sets.TrailCacheLength[NPC.type] = 20;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
		}
		public override void SetDefaults()
		{
			NPC.aiStyle =  3;
			NPC.lifeMax = 45;
			NPC.defense = 6;
			NPC.value = 65f;
			AIType = NPCID.Skeleton;
			NPC.knockBackResist = 0.7f;
			NPC.width = 30;
			NPC.height = 42;
			NPC.damage = 15;
			NPC.lavaImmune = false;
			NPC.noTileCollide = false;
			NPC.alpha = 0;
			NPC.dontTakeDamage = false;
			NPC.DeathSound = SoundID.NPCHit1;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Items.Banners.CavernBanditBanner>();
		}

		private const float SWING_RADIUS = 60f; //How far away from the targetted player the npc must be to begin its attack
		private const float AWAKE_RADIUS = 100f; //How far away from the targetted player the npc must be to wake up
		public override bool PreAI()
		{
			_timer += 0.05;
			Player player = Main.player[NPC.target];

			NPC.TargetClosest(true);

			if (_activated)
			{
				if (Vector2.Distance(player.Center, NPC.Center) <= SWING_RADIUS)
					NPC.velocity.X = 0f;
			}

			else if (Vector2.Distance(player.Center, NPC.Center) < AWAKE_RADIUS && !_activated) 
				_activated = true;

			if (NPC.life < NPC.lifeMax)
				_activated = true;

			Lighting.AddLight(new Vector2(NPC.Center.X, NPC.Center.Y - 40), 255 * 0.002f, 255 * 0.002f, 0 * 0.001f);


			if (NPC.velocity.X < 0f)
				_spriteDirection = 1;

			else if (NPC.velocity.X > 0f)
				_spriteDirection = -1;

			if (!_activated) 
				return false;

			return base.PreAI();
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.MagicLantern, 24));
			npcLoot.Add(ItemDropRule.Common(ItemID.Hook, 6));
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			SoundEngine.PlaySound(SoundID.NPCHit4, NPC.Center);
			if (NPC.life <= 0) 
				for(int i = 1; i <= 4; i++)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>($"CavernBanditGore{i}").Type, 1f);

			for (int k = 0; k < 7; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Iron, 2.5f * hitDirection, -2.5f, 0, default, 1.2f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Iron, 2.5f * hitDirection, -2.5f, 0, default, 0.5f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Iron, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => SpawnCondition.Cavern.Chance * 0.0234f;

		public override void FindFrame(int frameHeight)
		{
			Player player = Main.player[NPC.target];
			NPC.frameCounter++;

			//Method to look less repetitive
			void IncrementFrame(int frameCounterThreshold, int maxFrame, int minFrame = 0)
			{
				if(NPC.frameCounter >= frameCounterThreshold)
				{
					_frame++;
					NPC.frameCounter = 0;
					NPC.netUpdate = true;
				}

				if(_frame >= maxFrame)
				{
					_frame = minFrame;
					NPC.netUpdate = true;
				}

				if(_frame < minFrame)
				{
					_frame = minFrame;
					NPC.netUpdate = true;
				}
			}

			if (_activated) {
				if (Vector2.Distance(player.Center, NPC.Center) >= SWING_RADIUS) //Update between first 7 frames when not swinging
					IncrementFrame(7, 7);

				else //Update between the next 5 frames if swinging
				{
					IncrementFrame(5, 13, 7);
					if (_frame == 9 && NPC.frameCounter == 4 && Collision.CanHitLine(NPC.Center, 0, 0, Main.player[NPC.target].Center, 0, 0)) //Damage target on specific frame
						player.Hurt(PlayerDeathReason.LegacyDefault(), (int)NPC.damage * 2, NPC.direction * -1, false, false, false, -1);
				}
			}
			else
				if (!_pickedFrame) { //Choose a random frame if not awakened
					_frame = Main.rand.Next(13, 16);
					_pickedFrame = true;
					NPC.netUpdate = true;
				}

			NPC.frame.Y = frameHeight * _frame;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (_spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;
			int xpos = (int)((NPC.Center.X + 59) - Main.screenPosition.X) - (int)(TextureAssets.Npc[NPC.type].Value.Width / 2);
			int ypos = (int)((NPC.Center.Y - 60) - Main.screenPosition.Y) + (int)(Math.Sin(_timer) * 12);
			Texture2D ripple = Mod.Assets.Request<Texture2D>("Effects/Ripple").Value;
			Texture2D lantern = Mod.Assets.Request<Texture2D>("NPCs/CavernBandit/CavernLantern").Value;
			Main.spriteBatch.Draw(ripple, new Vector2(xpos, ypos), new Microsoft.Xna.Framework.Rectangle?(), Color.Yellow, NPC.rotation, ripple.Size() / 2f, 1f, spriteEffects, 0);
			Main.spriteBatch.Draw(lantern, new Vector2(xpos, ypos), new Microsoft.Xna.Framework.Rectangle?(), Color.White, NPC.rotation, lantern.Size() / 2f, 1f, spriteEffects, 0);

			Texture2D npcTex = TextureAssets.Npc[NPC.type].Value;
			Vector2 yOffset = new Vector2(0, NPC.gfxOffY);
			yOffset.Y += 22; //hardcoded offset so its on ground properly
			spriteBatch.Draw(npcTex, NPC.Center - Main.screenPosition + yOffset, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(NPC.frame.Width/2f, NPC.frame.Height), NPC.scale, spriteEffects, 0);
			return false;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(_timer);
			writer.Write(_activated);
			writer.Write(_pickedFrame);
			writer.Write(_frame);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			_timer = reader.ReadDouble();
			_activated = reader.ReadBoolean();
			_pickedFrame = reader.ReadBoolean();
			_frame = reader.ReadInt32();
		}
	}
}