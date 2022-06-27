using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using SpiritMod.Buffs;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using SpiritMod.Mechanics.BoonSystem;
using SpiritMod.Buffs.DoT;

namespace SpiritMod.NPCs.Automata
{
	public class AutomataSpinner : ModNPC, IBoonable
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Trochmaton");
			Main.npcFrameCount[NPC.type] = 11;
            NPCID.Sets.TrailCacheLength[NPC.type] = 3;
            NPCID.Sets.TrailingMode[NPC.type] = 0;
		}

		public override void SetDefaults()
		{
			NPC.width = 66;
			NPC.height = 56;
			NPC.damage = 55;
			NPC.defense = 35;
			NPC.lifeMax = 300;
            NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.value = 180f;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[BuffID.Venom] = true;
			NPC.buffImmune[ModContent.BuffType<FesteringWounds>()] = true;
			NPC.buffImmune[ModContent.BuffType<BloodCorrupt>()] = true;
			NPC.buffImmune[ModContent.BuffType<BloodInfusion>()] = true;
			NPC.knockBackResist = .25f;
			NPC.aiStyle = 3;
			AIType = NPCID.WalkingAntlion;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.TrochmatonBanner>();
		}
		int timer;
		int frame = 0;
		int frameTimer;
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(timer);
			writer.Write(frame);
			writer.Write(frameTimer);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			timer = reader.ReadInt32();
			frame = reader.ReadInt32();
			frameTimer = reader.ReadInt32();
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(5) == 0)
			{
				target.AddBuff(BuffID.BrokenArmor, 1800);
			}
		}
		public override void OnKill()
		{
			if (Main.rand.NextBool(100))
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.ArmorPolish);
			if (Main.rand.NextBool(85))
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<Items.Accessory.GoldenApple>());
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 10; k++) {
	            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Sunflower, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.47f);
				 Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Wraith, 2.5f * hitDirection, -2.5f, 0, default, Main.rand.NextFloat(.45f, .55f));
			}
            if (NPC.life <= 0) {
                SoundEngine.PlaySound(new LegacySoundStyle(4, 6).WithPitchVariance(0.2f), NPC.Center);
				for (int i = 0; i < 4; ++i)
                {
					Gore.NewGore(NPC.position, new Vector2(NPC.velocity.X * .5f, NPC.velocity.Y * .5f), 99);
                }
                Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Trochmaton/AutomataSpinner1").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Trochmaton/AutomataSpinner2").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Trochmaton/AutomataSpinner3").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Trochmaton/AutomataSpinner4").Type, 1f);
                Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Trochmaton/AutomataSpinner5").Type, 1f);
			}
		}
		public override void AI()
		{	
			NPC.spriteDirection = NPC.direction;
			timer++;
            FrameControl();
			Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
			direction.Normalize();

			if (timer == 300 && Main.netMode != NetmodeID.MultiplayerClient) {
				SoundEngine.PlaySound(SoundID.NPCHit, (int)NPC.position.X, (int)NPC.position.Y, 4);
                SoundEngine.PlaySound(SoundID.DD2_GoblinBomberThrow, (int)NPC.position.X, (int)NPC.position.Y);
				NPC.netUpdate = true;
			}
            if (timer >= 270 && timer < 300)
            {
                NPC.velocity *= .97f;
            }
			if (timer >= 300 && timer <= 320) {
				direction.X = direction.X * Main.rand.NextFloat(6.5f, 8.4f);
				direction.Y = 0 - Main.rand.NextFloat(.5f, 1.5f);
				NPC.velocity.X = direction.X;
				NPC.velocity.Y = direction.Y;
                NPC.noGravity = true;
				NPC.knockBackResist = 0f;
			}
			if (timer >= 370) {
                NPC.rotation = 0f;
				frame = 0;
				timer = 0;
                NPC.noGravity = false;
				NPC.knockBackResist = .2f;
				NPC.netUpdate = true;
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => (spawnInfo.SpawnTileType == TileID.Marble) && spawnInfo.SpawnTileY > Main.rockLayer && Main.hardMode ? 1f : 0f;

		public void FrameControl()
        {
            frameTimer++;
            if (frameTimer >= 4)
            {
                frameTimer = 0;
                frame++;
            }
			if (timer < 296)
            {
                if (frame > 5 || frame < 0)
                {
                    frame = 0;
                }
            }
			if (timer > 300 && timer < 364)
			{
				if (timer % 15 == 0)
				{
					SoundEngine.PlaySound(SoundID.DD2_GoblinBomberThrow, (int)NPC.position.X, (int)NPC.position.Y);
				}
                NPC.rotation = NPC.velocity.X * .05f;
			    NPC.knockBackResist = 0f;
                if (frame > 8 || frame < 6)
                {
                    frame = 7;
                }
			}
            if (timer == 296)
            {
                frame = 6;
            }
            if (timer > 364)
            {
                frame = 10;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (timer > 300)
            {
                Vector2 drawOrigin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width * 0.5f, (NPC.height * 0.5f));
                for (int k = 0; k < NPC.oldPos.Length; k++)
                {
                    var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                    Vector2 drawPos = NPC.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY);
                    Color color = NPC.GetAlpha(lightColor) * (float)(((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length) / 2f);
                    spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawPos, new Microsoft.Xna.Framework.Rectangle?(NPC.frame), color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
                }
            }
            return true;
        }
		public override void FindFrame(int frameHeight)
		{
			NPC.frame.Y = frameHeight * frame;
		}
	}
}
