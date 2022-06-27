using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace SpiritMod.Projectiles.Clubs
{
	public class NautilusBubbleProj : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Bubble");

		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.width = 42;
			Projectile.height = 42;
			Projectile.friendly = true;
			Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 2;
			Projectile.timeLeft = 100;
			Projectile.alpha = 110;
		}
        float alphaCounter;
		public override void AI()
		{
            alphaCounter += .04f;
			if (Projectile.timeLeft > 55)
                Projectile.tileCollide = false;
			else
                Projectile.tileCollide = true;
			Projectile.velocity.Y -= 0.01f;
        }

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Color color = new Color(255, 255, 255) * 0.95f * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Texture2D tex = ModContent.Request<Texture2D>("SpiritMod/Projectiles/Yoyo/MoonburstBubble_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

				spriteBatch.Draw(tex, Projectile.oldPos[k] + Projectile.Size / 2 - Main.screenPosition, null, color, 0f, tex.Size() / 2, Projectile.scale, default, default);
			}
		}

        public override bool PreDraw(ref Color lightColor)
        {
            float sineAdd = (float)Math.Sin(alphaCounter) + 3;
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;
            int xpos = (int)((Projectile.Center.X + 21) - Main.screenPosition.X) - (int)(TextureAssets.Projectile[Projectile.type].Value.Width / 2);
            int ypos = (int)((Projectile.Center.Y + 21) - Main.screenPosition.Y) - (int)(TextureAssets.Projectile[Projectile.type].Value.Width / 2);
            Texture2D ripple = TextureAssets.Extra[49].Value;
            Main.spriteBatch.Draw(ripple, new Vector2(xpos, ypos), new Microsoft.Xna.Framework.Rectangle?(), new Color((int)(7.5f * sineAdd), (int)(16.5f * sineAdd), (int)(18f * sineAdd), 0), Projectile.rotation, ripple.Size() / 2f, Projectile.scale, spriteEffects, 0);
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!target.boss && !target.friendly && target.knockBackResist != 0f && !target.dontTakeDamage)
                target.velocity.Y -= 5.6f;
        }

        public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item54, Projectile.position);
			for (int i = 0; i < 20; i++) {
				int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.CryoDust>(), 0f, -2f, 0, default, 2.2f);
				Main.dust[num].noGravity = true;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].scale *= .2825f;
				if (Main.dust[num].position != Projectile.Center)
					Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 1f;
			}
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;
            return true;
        }
    }
}
