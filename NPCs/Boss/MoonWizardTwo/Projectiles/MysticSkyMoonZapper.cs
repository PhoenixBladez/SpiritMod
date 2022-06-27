using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.NPCs.Boss.MoonWizard.Projectiles;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.MoonWizardTwo.Projectiles
{
	public class MysticSkyMoonZapper : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moon Zapper");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.penetrate = 1;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.friendly = false;
			Projectile.timeLeft = 100;
            Projectile.hide = true;
			Projectile.damage = 13;
			Projectile.extraUpdates = 1;
			Projectile.width = Projectile.height = 32;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

        float alphaCounter;

		public override void AI()
        {
            alphaCounter += 0.04f;
			if (Projectile.timeLeft == 20)
			{
				SoundEngine.PlaySound(SoundID.Item, Projectile.position, 122);
				int p = Projectile.NewProjectile(Projectile.Center + new Vector2(0, 500), Vector2.Zero, ModContent.ProjectileType<MoonThunder>(), Projectile.damage, 0);
                Main.projectile[p].hostile = true;
                Main.projectile[p].friendly = true;
            }
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;

		public override void PostDraw(Color lightColor)
        {
            float sineAdd = (float)Math.Sin(alphaCounter) + 3;
            Main.spriteBatch.Draw(SpiritMod.Instance.GetTexture("Effects/Masks/Extra_49"), Projectile.Center - Main.screenPosition, null, new Color((int)(7.5f * sineAdd), (int)(16.5f * sineAdd), (int)(18f * sineAdd), 0), 0f, new Vector2(50, 50), 0.25f * (sineAdd + .6f), SpriteEffects.None, 0f);
        }
    }
}