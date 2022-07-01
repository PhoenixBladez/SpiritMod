using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Consumable;
using SpiritMod.Tiles.Block;
using SpiritMod.NPCs.Boss.MoonWizard.Projectiles;
using System;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Sets.ClubSubclass;
using SpiritMod.NPCs.Spirit;

namespace SpiritMod.NPCs.MoonjellyEvent
{
	public class ExplodingMoonjelly : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moonlight Rupturer");
			Main.npcFrameCount[NPC.type] = 7;
		}

		public override void SetDefaults()
		{
			NPC.width = 26;
			NPC.height = 40;
			NPC.damage = 0;
			NPC.defense = 10;
			NPC.lifeMax = 48;
			NPC.HitSound = SoundID.NPCHit25;
			NPC.DeathSound = SoundID.NPCDeath28;
            NPC.buffImmune[BuffID.Poisoned] = true;
            NPC.buffImmune[BuffID.Venom] = true;
            NPC.value = 10f;
			NPC.knockBackResist = .45f;
            NPC.scale = 1f;
			NPC.noGravity = true;
            NPC.noTileCollide = true;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Items.Banners.MoonlightRupturerBanner>();
        }

		public override void HitEffect(int hitDirection, double damage)
        {
            for (int k = 0; k < 15; k++)
            {
                Dust d = Dust.NewDustPerfect(NPC.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(5), 0, default, 0.45f);
                d.noGravity = true;
            }
            if (NPC.life <= 0) {
                for (int k = 0; k < 30; k++)
                {
                    Dust d = Dust.NewDustPerfect(NPC.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(7), 0, default, 0.65f);
                    d.noGravity = true;
                }
            }
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}
        bool aggro = false;
		float alphaCounter;
		public override void AI()
        {
            alphaCounter += .04f;
            if (NPC.Distance(Main.player[NPC.target].Center) <= 450 || NPC.life < NPC.lifeMax)
                aggro = true;
			else
                aggro = false;
            if (!aggro)
            {
                AIType = NPCID.Firefly;
                NPC.aiStyle = 64;
            }
			else
            {
                NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) + 1.57f;

                NPC.TargetClosest(true);
                float speed = 5.5f;
                float acceleration = 0.07f;
                Vector2 vector2 = new Vector2(NPC.position.X + NPC.width * 0.5F, NPC.position.Y + NPC.height * 0.5F);
                float xDir = Main.player[NPC.target].position.X + (Main.player[NPC.target].width * 0.5F) - vector2.X;
                float yDir = Main.player[NPC.target].position.Y + (Main.player[NPC.target].height * 0.5F) - vector2.Y;
                float length = (float)Math.Sqrt(xDir * xDir + yDir * yDir);

                float num10 = speed / length;
                xDir = xDir * num10;
                yDir = yDir * num10;
                if (NPC.velocity.X < xDir)
                {
                    NPC.velocity.X = NPC.velocity.X + acceleration;
                    if (NPC.velocity.X < 0 && xDir > 0)
                        NPC.velocity.X = NPC.velocity.X + acceleration;
                }
                else if (NPC.velocity.X > xDir)
                {
                    NPC.velocity.X = NPC.velocity.X - acceleration;
                    if (NPC.velocity.X > 0 && xDir < 0)
                        NPC.velocity.X = NPC.velocity.X - acceleration;
                }

                if (NPC.velocity.Y < yDir)
                {
                    NPC.velocity.Y = NPC.velocity.Y + acceleration;
                    if (NPC.velocity.Y < 0 && yDir > 0)
                        NPC.velocity.Y = NPC.velocity.Y + acceleration;
                }
                else if (NPC.velocity.Y > yDir)
                {
                    NPC.velocity.Y = NPC.velocity.Y - acceleration;
                    if (NPC.velocity.Y > 0 && yDir < 0)
                        NPC.velocity.Y = NPC.velocity.Y - acceleration;
                }
                if (Main.player[NPC.target].Hitbox.Intersects(NPC.Hitbox))
                {
                    int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), Main.player[NPC.target].Center.X, Main.player[NPC.target].Center.Y, 0f, 0f, ModContent.ProjectileType<UnstableWisp_Explosion>(), 15, 0f, Main.myPlayer);
                    Main.projectile[p].hide = true;
                    Main.projectile[p].timeLeft = 20;

                    NPC.life = 0;
                    NPC.HitEffect(0, 10.0);
                    NPC.checkDead();
                    NPC.active = false;
                }
            }
            Lighting.AddLight(new Vector2(NPC.Center.X, NPC.Center.Y), 0.075f * 2, 0.231f * 2, 0.255f * 2);
        }
        public override bool CheckDead()
        {
            Vector2 center = NPC.Center;
            for (int k = 0; k < 30; k++)
            {
                Dust d = Dust.NewDustPerfect(NPC.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(7), 0, default, 0.75f);
                d.noGravity = true;
            }
            return true;
        }
        public override void OnKill()
        {
            NPC.DropItem(ItemID.Gel, Main.rand.Next(1, 4));

			if(Main.rand.NextBool(5))
				NPC.DropItem(Mod.Find<ModItem>("MoonJelly").Type);

			if (Main.rand.NextBool(25))
                NPC.DropItem(ModContent.ItemType<NautilusClub>());
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
			Main.spriteBatch.Draw(
            Mod.Assets.Request<Texture2D>("NPCs/MoonjellyEvent/ExplodingMoonjelly_Glow").Value,
            NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY),
            NPC.frame,
            Color.White,
            NPC.rotation,
            NPC.frame.Size() / 2,
            NPC.scale,
            SpriteEffects.None,
            0
			);
        }
    }
}
