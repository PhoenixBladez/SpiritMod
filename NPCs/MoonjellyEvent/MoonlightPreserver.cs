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
	public class MoonlightPreserver : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moonlight Preserver");
			Main.npcFrameCount[npc.type] = 7;
		}

		public override void SetDefaults()
		{
			npc.width = 40;
			npc.height = 60;
			npc.damage = 18;
			npc.defense = 0;
			npc.lifeMax = 59;
			npc.HitSound = SoundID.NPCHit25;
			npc.DeathSound = SoundID.NPCDeath28;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.buffImmune[BuffID.Venom] = true;
            npc.value = 60f;
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
                Dust d = Dust.NewDustPerfect(npc.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(5), 0, default, 0.65f);
                d.noGravity = true;
            }
            if (npc.life <= 0) {
                for (int i = 0; i < 5; i++)
                {
                    int p = Projectile.NewProjectile(npc.Center.X + Main.rand.Next(-10, 10), npc.Center.Y + Main.rand.Next(-10, 10), Main.rand.NextFloat(-1.1f, 1.1f), Main.rand.NextFloat(-1.1f, 1.1f), ModContent.ProjectileType<JellyfishOrbiter>(), 11, 0.0f, Main.myPlayer, 0.0f, (float)npc.whoAmI);
                    Main.projectile[p].scale = Main.rand.NextFloat(.5f, .8f);
                    Main.projectile[p].timeLeft = Main.rand.Next(75, 95);
                }
                for (int k = 0; k < 50; k++)
                {
                    Dust d = Dust.NewDustPerfect(npc.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(7), 0, default, 0.95f);
                    d.noGravity = true;
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
        bool chosen = false;
        int tremorItem;
		public override void AI()
		{
			if (!chosen)
            {
                if (Main.rand.NextBool(3))
                {
                    if (Main.rand.NextBool(9))
                    {
                        tremorItem = ModContent.ItemType<Items.Weapon.Summon.ElectricGun.ElectricGun>();
                    }
					else
                    {
                        tremorItem = Main.rand.Next(new int[] { 19, 20, 21, 22, ModContent.ItemType<Items.Placeable.Tiles.SpaceJunkItem>(), 286, ModContent.ItemType<Items.Placeable.Tiles.AsteroidBlock>() });
                    }
                }
                chosen = true;
                npc.netUpdate = true;
            }
            Lighting.AddLight(new Vector2(npc.Center.X, npc.Center.Y), 0.075f * 2, 0.231f * 2, 0.255f * 2);
        }
        public override void NPCLoot()
        {
            if (tremorItem == ModContent.ItemType<Items.Weapon.Summon.ElectricGun.ElectricGun>())
            {
                npc.DropItem(tremorItem);
            }
            else
            {
                npc.DropItem(tremorItem, Main.rand.Next(3, 9));
            }
            npc.DropItem(ItemID.Gel, Main.rand.Next(1, 4));
			npc.DropItem(mod.ItemType("MoonJelly"));
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Microsoft.Xna.Framework.Color color9 = Lighting.GetColor((int)((double)npc.position.X + (double)npc.width * 0.5) / 16, (int)(((double)npc.position.Y + (double)npc.height * 0.5) / 16.0));

            int num356 = tremorItem;
            float num355 = npc.scale;
            float num354 = 22f * npc.scale;
            float num353 = 18f * npc.scale;
            float num352 = (float)18;
            float num351 = (float)34;
            if (num352 > num354)
            {
                num355 *= num354 / num352;
                num352 *= num355;
                num351 *= num355;
            }
            if (num351 > num353)
            {
                num355 *= num353 / num351;
                num352 *= num355;
                num351 *= num355;
            }
            float num348 = -1f;
            float num347 = 1f;
            int num346 = npc.frame.Y / (Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type]);
            num347 -= (float)Math.Sin(num346) * Main.rand.NextFloat(.8f, 1.3f);
            num348 += (float)(Math.Sin(num346) * Main.rand.NextFloat(1.6f, 2.3f));
            float num349 = -1f + (float)(Math.Sin(num346) * -3f);
            float num343 = 0.2f;
            num343 -= 0.02f * (float)num346;

            Main.spriteBatch.Draw(mod.GetTexture("NPCs/Boss/MoonWizard/Projectiles/Baby_Jellyfish_1"), new Vector2(npc.Center.X - Main.screenPosition.X + num348, npc.Center.Y - Main.screenPosition.Y + npc.gfxOffY + num347), null, color9, num343, new Vector2((float)(18 / 2), (float)(34 / 2)), num355, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(mod.GetTexture("NPCs/Boss/MoonWizard/Projectiles/Baby_Jellyfish_1_Glow"), new Vector2(npc.Center.X - Main.screenPosition.X + num348, npc.Center.Y - Main.screenPosition.Y + npc.gfxOffY + num347), null, Color.White, num343, new Vector2((float)(18 / 2), (float)(34 / 2)), num355, SpriteEffects.None, 0f);

            Main.spriteBatch.Draw(mod.GetTexture("NPCs/Boss/MoonWizard/Projectiles/Baby_Jellyfish_1"), new Vector2(npc.Center.X + 5 - Main.screenPosition.X + num348, npc.Center.Y + 7 - Main.screenPosition.Y + npc.gfxOffY + num347), null, color9, num343, new Vector2((float)(18 / 2), (float)(34 / 2)), num355, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(mod.GetTexture("NPCs/Boss/MoonWizard/Projectiles/Baby_Jellyfish_1_Glow"), new Vector2(npc.Center.X + 5 - Main.screenPosition.X + num348, npc.Center.Y + 7 - Main.screenPosition.Y + npc.gfxOffY + num347), null, Color.White, num343, new Vector2((float)(18 / 2), (float)(34 / 2)), num355, SpriteEffects.None, 0f);

            Main.spriteBatch.Draw(mod.GetTexture("NPCs/Boss/MoonWizard/Projectiles/Baby_Jellyfish_2"), new Vector2(npc.Center.X + 4 - Main.screenPosition.X + num349, npc.Center.Y - 14 - Main.screenPosition.Y + npc.gfxOffY + num347), null, color9, num343, new Vector2((float)(18 / 2), (float)(34 / 2)), num355, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(mod.GetTexture("NPCs/Boss/MoonWizard/Projectiles/Baby_Jellyfish_2_Glow"), new Vector2(npc.Center.X + 4 - Main.screenPosition.X + num349, npc.Center.Y - 14 - Main.screenPosition.Y + npc.gfxOffY + num347), null, Color.White, num343, new Vector2((float)(18 / 2), (float)(34 / 2)), num355, SpriteEffects.None, 0f);

            Main.spriteBatch.Draw(mod.GetTexture("NPCs/Boss/MoonWizard/Projectiles/Baby_Jellyfish_5"), new Vector2(npc.Center.X - 8 - Main.screenPosition.X + num349, npc.Center.Y + 4 - Main.screenPosition.Y + npc.gfxOffY + num347), null, color9, num343, new Vector2((float)(18 / 2), (float)(34 / 2)), num355, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(mod.GetTexture("NPCs/Boss/MoonWizard/Projectiles/Baby_Jellyfish_5_Glow"), new Vector2(npc.Center.X - 8 - Main.screenPosition.X + num349, npc.Center.Y + 4 - Main.screenPosition.Y + npc.gfxOffY + num347), null, Color.White, num343, new Vector2((float)(18 / 2), (float)(34 / 2)), num355, SpriteEffects.None, 0f);

            Main.spriteBatch.Draw(mod.GetTexture("NPCs/Boss/MoonWizard/Projectiles/Baby_Jellyfish_4"), new Vector2(npc.Center.X - 6 - Main.screenPosition.X + num348, npc.Center.Y - 9 - Main.screenPosition.Y + npc.gfxOffY + num347), null, color9, num343, new Vector2((float)(18 / 2), (float)(34 / 2)), num355, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(mod.GetTexture("NPCs/Boss/MoonWizard/Projectiles/Baby_Jellyfish_4_Glow"), new Vector2(npc.Center.X - 6 - Main.screenPosition.X + num348, npc.Center.Y - 9 - Main.screenPosition.Y + npc.gfxOffY + num347), null, Color.White, num343, new Vector2((float)(18 / 2), (float)(34 / 2)), num355, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(Main.itemTexture[tremorItem], new Vector2(npc.Center.X - Main.screenPosition.X + num348, npc.Center.Y - Main.screenPosition.Y + npc.gfxOffY + num347), null, color9, num343, new Vector2((float)(Main.itemTexture[tremorItem].Width / 2), (float)(Main.itemTexture[tremorItem].Height / 2)), num355 * 1.3f, SpriteEffects.None, 0f);

            var effects = 1;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                             drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, SpriteEffects.None, 0);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            float num341 = 0f;
            float num340 = npc.height;
            float num108 = 4;
            float num107 = (float)Math.Cos((double)(Main.GlobalTime % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;
            float num106 = 0f;


            Texture2D texture2D6 = Main.npcTexture[npc.type];
            Vector2 vector15 = new Vector2((float)(Main.npcTexture[npc.type].Width / 2), (float)(Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
            SpriteEffects spriteEffects3 = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 vector33 = new Vector2(npc.Center.X, npc.Center.Y - 18) - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity;
            Microsoft.Xna.Framework.Color color29 = new Microsoft.Xna.Framework.Color(127 - npc.alpha, 127 - npc.alpha, 127 - npc.alpha, 0).MultiplyRGBA(Microsoft.Xna.Framework.Color.LightBlue);
            for (int num103 = 0; num103 < 4; num103++)
            {
                Microsoft.Xna.Framework.Color color28 = color29;
                color28 = npc.GetAlpha(color28);
                color28 *= 1f - num107;
                Vector2 vector29 = new Vector2(npc.Center.X, npc.Center.Y) + ((float)num103 / (float)num108 * 6.28318548f + npc.rotation + num106).ToRotationVector2() * (6f * num107 + 2f) - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity * (float)num103;
                Main.spriteBatch.Draw(mod.GetTexture("NPCs/MoonjellyEvent/MoonlightPreserver_Glow"), vector29, npc.frame, color28, npc.rotation, npc.frame.Size() / 2f, npc.scale, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(
                mod.GetTexture("NPCs/MoonjellyEvent/MoonlightPreserver_Glow"),
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
}
