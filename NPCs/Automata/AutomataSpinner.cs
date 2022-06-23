using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using SpiritMod.Buffs;
using SpiritMod.Items.Material;
using Terraria;
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
			Main.npcFrameCount[npc.type] = 11;
            NPCID.Sets.TrailCacheLength[npc.type] = 3;
            NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.width = 66;
			npc.height = 56;
			npc.damage = 55;
			npc.defense = 35;
			npc.lifeMax = 300;
            npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath14;
			npc.value = 180f;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.buffImmune[BuffID.Venom] = true;
			npc.buffImmune[ModContent.BuffType<FesteringWounds>()] = true;
			npc.buffImmune[ModContent.BuffType<BloodCorrupt>()] = true;
			npc.buffImmune[ModContent.BuffType<BloodInfusion>()] = true;
			npc.knockBackResist = .25f;
			npc.aiStyle = 3;
			aiType = NPCID.WalkingAntlion;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.TrochmatonBanner>();
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
		public override void NPCLoot()
		{
			if (Main.rand.NextBool(100))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.ArmorPolish);
			if (Main.rand.NextBool(85))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Accessory.GoldenApple>());
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 10; k++) {
	            Dust.NewDust(npc.position, npc.width, npc.height, DustID.Sunflower, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.47f);
				 Dust.NewDust(npc.position, npc.width, npc.height, DustID.Wraith, 2.5f * hitDirection, -2.5f, 0, default, Main.rand.NextFloat(.45f, .55f));
			}
            if (npc.life <= 0) {
                Main.PlaySound(new LegacySoundStyle(4, 6).WithPitchVariance(0.2f), npc.Center);
				for (int i = 0; i < 4; ++i)
                {
					Gore.NewGore(npc.position, new Vector2(npc.velocity.X * .5f, npc.velocity.Y * .5f), 99);
                }
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Trochmaton/AutomataSpinner1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Trochmaton/AutomataSpinner2"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Trochmaton/AutomataSpinner3"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Trochmaton/AutomataSpinner4"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Trochmaton/AutomataSpinner5"), 1f);
			}
		}
		public override void AI()
		{	
			npc.spriteDirection = npc.direction;
			timer++;
            FrameControl();
			Vector2 direction = Main.player[npc.target].Center - npc.Center;
			direction.Normalize();

			if (timer == 300 && Main.netMode != NetmodeID.MultiplayerClient) {
				Main.PlaySound(SoundID.NPCHit, (int)npc.position.X, (int)npc.position.Y, 4);
                Main.PlaySound(SoundID.DD2_GoblinBomberThrow, (int)npc.position.X, (int)npc.position.Y);
				npc.netUpdate = true;
			}
            if (timer >= 270 && timer < 300)
            {
                npc.velocity *= .97f;
            }
			if (timer >= 300 && timer <= 320) {
				direction.X = direction.X * Main.rand.NextFloat(6.5f, 8.4f);
				direction.Y = 0 - Main.rand.NextFloat(.5f, 1.5f);
				npc.velocity.X = direction.X;
				npc.velocity.Y = direction.Y;
                npc.noGravity = true;
				npc.knockBackResist = 0f;
			}
			if (timer >= 370) {
                npc.rotation = 0f;
				frame = 0;
				timer = 0;
                npc.noGravity = false;
				npc.knockBackResist = .2f;
				npc.netUpdate = true;
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => (spawnInfo.spawnTileType == TileID.Marble) && spawnInfo.spawnTileY > Main.rockLayer && Main.hardMode ? 1f : 0f;

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
					Main.PlaySound(SoundID.DD2_GoblinBomberThrow, (int)npc.position.X, (int)npc.position.Y);
				}
                npc.rotation = npc.velocity.X * .05f;
			    npc.knockBackResist = 0f;
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
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (timer > 300)
            {
                Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height * 0.5f));
                for (int k = 0; k < npc.oldPos.Length; k++)
                {
                    var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                    Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
                    Color color = npc.GetAlpha(lightColor) * (float)(((npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2f);
                    spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
                }
            }
            return true;
        }
		public override void FindFrame(int frameHeight)
		{
			npc.frame.Y = frameHeight * frame;
		}
	}
}
