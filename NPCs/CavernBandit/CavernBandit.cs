using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.IO;

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
			Main.npcFrameCount[npc.type] = 16;
			NPCID.Sets.TrailCacheLength[npc.type] = 20;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}
		public override void SetDefaults()
		{
			npc.aiStyle =  3;
			npc.lifeMax = 45;
			npc.defense = 6;
			npc.value = 65f;
			aiType = NPCID.Skeleton;
			npc.knockBackResist = 0.7f;
			npc.width = 30;
			npc.height = 42;
			npc.damage = 15;
			npc.lavaImmune = false;
			npc.noTileCollide = false;
			npc.alpha = 0;
			npc.dontTakeDamage = false;
			npc.DeathSound = new Terraria.Audio.LegacySoundStyle(4, 1);
            banner = npc.type;
            bannerItem = ModContent.ItemType<Items.Banners.CavernBanditBanner>();
		}

		private const float SWING_RADIUS = 60f; //How far away from the targetted player the npc must be to begin its attack
		private const float AWAKE_RADIUS = 100f; //How far away from the targetted player the npc must be to wake up
		public override bool PreAI()
		{
			_timer += 0.05;
			Player player = Main.player[npc.target];

			npc.TargetClosest(true);

			if (_activated)
			{
				if (Vector2.Distance(player.Center, npc.Center) <= SWING_RADIUS)
					npc.velocity.X = 0f;
			}

			else if (Vector2.Distance(player.Center, npc.Center) < AWAKE_RADIUS && !_activated) 
				_activated = true;

			if (npc.life < npc.lifeMax)
				_activated = true;

			Lighting.AddLight(new Vector2(npc.Center.X, npc.Center.Y - 40), 255 * 0.002f, 255 * 0.002f, 0 * 0.001f);


			if (npc.velocity.X < 0f)
				_spriteDirection = 1;

			else if (npc.velocity.X > 0f)
				_spriteDirection = -1;

			if (!_activated) 
				return false;

			return base.PreAI();
		}

        public override void NPCLoot()
        {
            if (Main.rand.NextBool(24))
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.MagicLantern);
        }

        public override void HitEffect(int hitDirection, double damage)
		{
			Main.PlaySound(SoundID.NPCHit, (int)npc.position.X, (int)npc.position.Y, 4, 1f, 0f);
			if (npc.life <= 0) 
				for(int i = 1; i <= 4; i++)
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot($"Gores/CavernBanditGore{i}"), 1f);

			for (int k = 0; k < 7; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Iron, 2.5f * hitDirection, -2.5f, 0, default, 1.2f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Iron, 2.5f * hitDirection, -2.5f, 0, default, 0.5f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Iron, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => SpawnCondition.Cavern.Chance * 0.0234f;

		public override void FindFrame(int frameHeight)
		{
			Player player = Main.player[npc.target];
			npc.frameCounter++;

			//Method to look less repetitive
			void IncrementFrame(int frameCounterThreshold, int maxFrame, int minFrame = 0)
			{
				if(npc.frameCounter >= frameCounterThreshold)
				{
					_frame++;
					npc.frameCounter = 0;
					npc.netUpdate = true;
				}

				if(_frame >= maxFrame)
				{
					_frame = minFrame;
					npc.netUpdate = true;
				}

				if(_frame < minFrame)
				{
					_frame = minFrame;
					npc.netUpdate = true;
				}
			}

			if (_activated) {
				if (Vector2.Distance(player.Center, npc.Center) >= SWING_RADIUS) //Update between first 7 frames when not swinging
					IncrementFrame(7, 7);

				else //Update between the next 5 frames if swinging
				{
					IncrementFrame(5, 13, 7);
					if (_frame == 9 && npc.frameCounter == 4 && Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0)) //Damage target on specific frame
						player.Hurt(PlayerDeathReason.LegacyDefault(), (int)npc.damage * 2, npc.direction * -1, false, false, false, -1);
				}
			}
			else
				if (!_pickedFrame) { //Choose a random frame if not awakened
					_frame = Main.rand.Next(13, 16);
					_pickedFrame = true;
					npc.netUpdate = true;
				}

			npc.frame.Y = frameHeight * _frame;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (_spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;
			int xpos = (int)((npc.Center.X + 59) - Main.screenPosition.X) - (int)(Main.npcTexture[npc.type].Width / 2);
			int ypos = (int)((npc.Center.Y - 60) - Main.screenPosition.Y) + (int)(Math.Sin(_timer) * 12);
			Texture2D ripple = mod.GetTexture("Effects/Ripple");
			Texture2D lantern = mod.GetTexture("NPCs/CavernBandit/CavernLantern");
			Main.spriteBatch.Draw(ripple, new Vector2(xpos, ypos), new Microsoft.Xna.Framework.Rectangle?(), Color.Yellow, npc.rotation, ripple.Size() / 2f, 1f, spriteEffects, 0);
			Main.spriteBatch.Draw(lantern, new Vector2(xpos, ypos), new Microsoft.Xna.Framework.Rectangle?(), Color.White, npc.rotation, lantern.Size() / 2f, 1f, spriteEffects, 0);

			Texture2D npcTex = Main.npcTexture[npc.type];
			Vector2 yOffset = new Vector2(0, npc.gfxOffY);
			yOffset.Y += 22; //hardcoded offset so its on ground properly
			spriteBatch.Draw(npcTex, npc.Center - Main.screenPosition + yOffset, npc.frame, npc.GetAlpha(drawColor), npc.rotation, new Vector2(npc.frame.Width/2f, npc.frame.Height), npc.scale, spriteEffects, 0);
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