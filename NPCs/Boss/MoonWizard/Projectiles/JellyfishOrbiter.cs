using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.MoonWizard.Projectiles
{
	public class JellyfishOrbiter : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arcane Energy");
            Main.projFrames[Projectile.type] = 5;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 1;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.penetrate = 2;
            Projectile.hide = false;
			Projectile.timeLeft = Main.rand.Next(76, 84);
		}

        float alphaCounter;

		public override void AI()
        {
            alphaCounter += .04f;
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
            Lighting.AddLight(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0.075f * 2, 0.231f * 2, 0.255f * 2);
            Projectile.frameCounter++;
            Projectile.spriteDirection = -Projectile.direction;
            if (Projectile.frameCounter >= 4)
            {
                Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
                Projectile.frameCounter = 0;
            }
            int num1 = ModContent.NPCType<MoonWizard>();
            float num2 = 60f;
            float x = 0.08f;
            float y = 0.1f;
            bool flag2 = false;

            if ((double)Projectile.ai[0] < (double)num2)
            {
                bool flag4 = true;
                int index1 = (int)Projectile.ai[1];
                if (Main.npc[index1].active && Main.npc[index1].type == num1)
                {
                    if (!flag2 && Main.npc[index1].oldPos[1] != Vector2.Zero)
                        Projectile.position = Projectile.position + Main.npc[index1].position - Main.npc[index1].oldPos[1];
                }
                else
                {
                    Projectile.ai[0] = num2;
                    flag4 = false;
                }
                if (flag4 && !flag2)
                {
                    Projectile.velocity = Projectile.velocity + new Vector2((float)Math.Sign(Main.npc[index1].Center.X - Projectile.Center.X), (float)Math.Sign(Main.npc[index1].Center.Y - Projectile.Center.Y)) * new Vector2(x, y);
                    if (Projectile.velocity.Length() > 7f)
                    {
                        Projectile.velocity *= 7f / Projectile.velocity.Length();
                    }
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            float sineAdd = (float)Math.Sin(alphaCounter) + 3;
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;
            int xpos = (int)((Projectile.Center.X + 10) - Main.screenPosition.X) - (int)(TextureAssets.Projectile[Projectile.type].Value.Width / 2);
            int ypos = (int)((Projectile.Center.Y + 10) - Main.screenPosition.Y) - (int)(TextureAssets.Projectile[Projectile.type].Value.Width / 2);
            Texture2D ripple = Mod.Assets.Request<Texture2D>("Effects/Masks/Extra_49").Value;
            Main.spriteBatch.Draw(ripple, new Vector2(xpos, ypos), new Microsoft.Xna.Framework.Rectangle?(), new Color((int)(7.5f * sineAdd), (int)(16.5f * sineAdd), (int)(18f * sineAdd), 0), Projectile.rotation, ripple.Size() / 2f, .5f, spriteEffects, 0);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            float maxDistance = 1000f; // max distance to search for a player
            int index = -1;

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player target = Main.player[i];
                if (!target.active || target.dead)
                {
                    continue;
                }
                float curDistance = Projectile.Distance(target.Center);
                if (curDistance < maxDistance)
                {
                    index = i;
                    maxDistance = curDistance;
                }
            }

			if (index != -1)
			{
				SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 69);
				float speed = Main.expertMode ? Main.rand.NextFloat(15, 17) : Main.rand.NextFloat(11.5f, 12.5f);
				Vector2 direction = Vector2.Normalize(Main.player[index].Center - Projectile.Center) * speed;
				Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, direction, ModContent.ProjectileType<JellyfishOrbiter_Projectile>(), Projectile.damage, 0, Main.myPlayer);
			}
        }
	}
}
