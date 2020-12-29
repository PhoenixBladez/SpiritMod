using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Consumable;
using SpiritMod.Tiles.Block;
using SpiritMod.NPCs.Boss.MoonWizard.Projectiles;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.MoonjellyEvent
{
	public class DreamlightJelly : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dreamlight Jelly");
			Main.npcFrameCount[npc.type] = 6;
		}

		public override void SetDefaults()
		{
			npc.width = 12;
			npc.height = 20;
            npc.rarity = 3;
            npc.damage = 0;
			npc.defense = 0;
			npc.lifeMax = 12;
			npc.HitSound = SoundID.NPCHit25;
			npc.DeathSound = SoundID.NPCDeath28;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.buffImmune[BuffID.Venom] = true;
            npc.value = 60f;
			Main.npcCatchable[npc.type] = true;
			npc.catchItem = (short)ModContent.ItemType<DreamlightJellyItem>();
			npc.knockBackResist = .45f;
			npc.aiStyle = 64;
            npc.scale = 1f;
			npc.noGravity = true;
            npc.noTileCollide = true;
			aiType = NPCID.Firefly;
		}

		public override void HitEffect(int hitDirection, double damage)
        {
            for (int k = 0; k < 15; k++)
            {
                Dust d = Dust.NewDustPerfect(npc.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(2), 0, default, 0.65f);
                d.noGravity = true;
            }
        }
        float alphaCounter;
        public override void AI()
        {
            npc.rotation = npc.velocity.X * .15f;
            npc.spriteDirection = npc.direction;
            alphaCounter += .04f;
               Lighting.AddLight(new Vector2(npc.Center.X, npc.Center.Y), 0.075f * 2, 0.231f * 2, 0.255f * 2);
        }
        public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
        bool chosen = false;
        public override void NPCLoot()
        {
            npc.DropItem(ItemID.Gel);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            float sineAdd = (float)Math.Sin(alphaCounter) + 3;
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (npc.spriteDirection == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;
            int xpos = (int)((npc.Center.X) - Main.screenPosition.X + 16) - (int)(Main.npcTexture[npc.type].Width / 2);
            int ypos = (int)((npc.Center.Y) - Main.screenPosition.Y + 10) - (int)(Main.npcTexture[npc.type].Width / 2);
            Texture2D ripple = mod.GetTexture("Effects/Masks/Extra_49");
            Main.spriteBatch.Draw(ripple, new Vector2(xpos, ypos), new Microsoft.Xna.Framework.Rectangle?(), new Color((int)(18f * sineAdd), (int)(25f * sineAdd), (int)(20f * sineAdd), 0), npc.rotation, ripple.Size() / 2f, .5f, spriteEffects, 0);

            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                             drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, SpriteEffects.None, 0);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Main.spriteBatch.Draw(
                mod.GetTexture("NPCs/MoonjellyEvent/DreamlightJelly_Glow"),
				npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY),
				npc.frame,
				Color.White,
				npc.rotation,
				npc.frame.Size() / 2,
				npc.scale,
				SpriteEffects.None,
				0
			);
            
        }
    }
}
