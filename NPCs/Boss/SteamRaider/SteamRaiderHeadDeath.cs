using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Projectiles;
using Terraria.Graphics.Shaders;
using Terraria.World.Generation;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Boss;
using Terraria.GameContent.Generation;
using SpiritMod.Tiles;
using SpiritMod;
using SpiritMod.Effects;

namespace SpiritMod.NPCs.Boss.SteamRaider
{
	public class SteamRaiderHeadDeath : ModNPC
	{
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starplate Voyager");
		}

		public override void SetDefaults() {
            npc.width = 64; //324
			npc.height = 56; //216
            bossBag = mod.ItemType("SteamRaiderBag");
            npc.boss = true;
            npc.damage = 0;
            npc.defense = 12;
            npc.noTileCollide = true;
            npc.dontTakeDamage = true;
            npc.lifeMax = 65;
            npc.HitSound = SoundID.NPCHit4;
            npc.value = 160f;
            npc.knockBackResist = .16f;
            npc.noGravity = true;
            npc.dontCountMe = true;
        }
		int timeLeft = 200;
        float alphaCounter;
         public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
                Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height / Main.npcFrameCount[npc.type]) * 0.5f);
                    float sineAdd = alphaCounter + 2;
                    Vector2 drawPos1 = npc.Center - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
                    Main.spriteBatch.Draw(SpiritMod.instance.GetTexture("Effects/Masks/Extra_49"), (npc.Center - Main.screenPosition) - new Vector2(-2, 8), null, new Color((int)(7.5f * sineAdd), (int)(16.5f * sineAdd), (int)(18f * sineAdd), 0), 0f, new Vector2(50, 50), 0.25f * (sineAdd + .65f), SpriteEffects.None, 0f);
                    return true;
        }
		public override void AI()
		{
             alphaCounter += 0.025f;
             npc.alpha = 255 - timeLeft;
			if (timeLeft == 200)
            {
                npc.rotation = 3.14f;
            }
            npc.rotation += Main.rand.Next(-20,20) / 100f;
            Dust.NewDustPerfect(npc.Center, 226, new Vector2(Main.rand.Next(-10,10),Main.rand.Next(-10,10)));
            if (timeLeft < 50)
            {
                Dust.NewDustPerfect(npc.Center, 226, new Vector2(Main.rand.Next(-10,10),Main.rand.Next(-10,10)));
            }
            timeLeft--;
            if (timeLeft <= 0)
            {
                        if (Main.expertMode)
                    {
                        npc.DropBossBags();
                        return;
                    }

                    npc.DropItem(mod.ItemType("SteamParts"), 19, 25);

                    npc.DropItem(Items.Armor.Masks.StarplateMask._type, 1f / 7);
                    npc.DropItem(Items.Boss.Trophy3._type, 1f / 10);
                 Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 14);
                    for(int i = 0; i < 90; i++) {
                       Dust.NewDust(npc.position, npc.width, npc.height, 226, Main.rand.Next(-25,25),Main.rand.Next(-13,13));
                        }
                npc.position.X = npc.position.X + (float)(npc.width / 2);
				npc.position.Y = npc.position.Y + (float)(npc.height / 2);
				npc.width = 30;
				npc.height = 30;
				npc.position.X = npc.position.X - (float)(npc.width / 2);
				npc.position.Y = npc.position.Y - (float)(npc.height / 2);
				Vector2 direction = Main.player[npc.target].Center - npc.Center;
				direction.Normalize();
				direction.X *= 4f;
				direction.Y *= -4f;

				int amountOfProjectiles = Main.rand.Next(1, 2);
				for (int i = 0; i < amountOfProjectiles; ++i)
				{
					float A = (float)Main.rand.Next(-150, 150) * 0.01f;
					float B = (float)Main.rand.Next(-80, 0) * 0.01f;
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, mod.ProjectileType("SteamBodyFallingProj"), 15, 1, Main.myPlayer, 0, 0);
				}
                Main.NewText("Starplate Voyager has been defeated!", 175, 75, 255, false);
                npc.active = false;
            }
        }
        public override void NPCLoot()
		{
			if (Main.expertMode)
			{
				npc.DropBossBags();
				return;
			}

			npc.DropItem(mod.ItemType("SteamParts"), 19, 25);

			npc.DropItem(Items.Armor.Masks.StarplateMask._type, 1f / 7);
			npc.DropItem(Items.Boss.Trophy3._type, 1f / 10);
		}
	}
}