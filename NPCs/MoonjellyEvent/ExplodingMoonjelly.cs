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
using SpiritMod.Items.Weapon.Club;

namespace SpiritMod.NPCs.MoonjellyEvent
{
	public class ExplodingMoonjelly : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moonlight Rupturer");
			Main.npcFrameCount[npc.type] = 7;
		}

		public override void SetDefaults()
		{
			npc.width = 26;
			npc.height = 40;
			npc.damage = 0;
			npc.defense = 10;
			npc.lifeMax = 48;
			npc.HitSound = SoundID.NPCHit25;
			npc.DeathSound = SoundID.NPCDeath28;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.buffImmune[BuffID.Venom] = true;
            npc.value = 10f;
			npc.knockBackResist = .45f;
            npc.scale = 1f;
			npc.noGravity = true;
            npc.noTileCollide = true;
            banner = npc.type;
            bannerItem = ModContent.ItemType<Items.Banners.MoonlightRupturerBanner>();
        }

		public override void HitEffect(int hitDirection, double damage)
        {
            for (int k = 0; k < 15; k++)
            {
                Dust d = Dust.NewDustPerfect(npc.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(5), 0, default, 0.45f);
                d.noGravity = true;
            }
            if (npc.life <= 0) {
                for (int k = 0; k < 30; k++)
                {
                    Dust d = Dust.NewDustPerfect(npc.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(7), 0, default, 0.65f);
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
        bool aggro = false;
        int tremorItem;
		float alphaCounter;
		public override void AI()
        {
            alphaCounter += .04f;
            if (npc.Distance(Main.player[npc.target].Center) <= 450 || npc.life < npc.lifeMax)
            {
                aggro = true;
            }
			else
            {
                aggro = false;
            }
            if (!aggro)
            {
                aiType = NPCID.Firefly;
                npc.aiStyle = 64;
            }
			else
            {
                npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;

                npc.TargetClosest(true);
                float speed = 5.5f;
                float acceleration = 0.07f;
                Vector2 vector2 = new Vector2(npc.position.X + npc.width * 0.5F, npc.position.Y + npc.height * 0.5F);
                float xDir = Main.player[npc.target].position.X + (Main.player[npc.target].width * 0.5F) - vector2.X;
                float yDir = Main.player[npc.target].position.Y + (Main.player[npc.target].height * 0.5F) - vector2.Y;
                float length = (float)Math.Sqrt(xDir * xDir + yDir * yDir);

                float num10 = speed / length;
                xDir = xDir * num10;
                yDir = yDir * num10;
                if (npc.velocity.X < xDir)
                {
                    npc.velocity.X = npc.velocity.X + acceleration;
                    if (npc.velocity.X < 0 && xDir > 0)
                        npc.velocity.X = npc.velocity.X + acceleration;
                }
                else if (npc.velocity.X > xDir)
                {
                    npc.velocity.X = npc.velocity.X - acceleration;
                    if (npc.velocity.X > 0 && xDir < 0)
                        npc.velocity.X = npc.velocity.X - acceleration;
                }

                if (npc.velocity.Y < yDir)
                {
                    npc.velocity.Y = npc.velocity.Y + acceleration;
                    if (npc.velocity.Y < 0 && yDir > 0)
                        npc.velocity.Y = npc.velocity.Y + acceleration;
                }
                else if (npc.velocity.Y > yDir)
                {
                    npc.velocity.Y = npc.velocity.Y - acceleration;
                    if (npc.velocity.Y > 0 && yDir < 0)
                        npc.velocity.Y = npc.velocity.Y - acceleration;
                }
                if (Main.player[npc.target].Hitbox.Intersects(npc.Hitbox))
                {
                    int p = Terraria.Projectile.NewProjectile(Main.player[npc.target].Center.X, Main.player[npc.target].Center.Y, 0f, 0f, mod.ProjectileType("UnstableWisp_Explosion"), 15, 0f, Main.myPlayer);
                    Main.projectile[p].hide = true;
                    Main.projectile[p].timeLeft = 20;

                    npc.life = 0;
                    npc.HitEffect(0, 10.0);
                    npc.checkDead();
                    npc.active = false;
                }
            }
            Lighting.AddLight(new Vector2(npc.Center.X, npc.Center.Y), 0.075f * 2, 0.231f * 2, 0.255f * 2);
        }
        public override bool CheckDead()
        {
            Vector2 center = npc.Center;
            for (int k = 0; k < 30; k++)
            {
                Dust d = Dust.NewDustPerfect(npc.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(7), 0, default, 0.75f);
                d.noGravity = true;
            }
            return true;
        }
        public override void NPCLoot()
        {
            npc.DropItem(ItemID.Gel, Main.rand.Next(1, 4));

			if(Main.rand.NextBool(2))
				npc.DropItem(mod.ItemType("MoonJelly"));

			if (Main.rand.NextBool(25))
            {
                npc.DropItem(ModContent.ItemType<NautilusClub>());
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                             drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
			Main.spriteBatch.Draw(
            mod.GetTexture("NPCs/MoonjellyEvent/ExplodingMoonjelly_Glow"),
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
