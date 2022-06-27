using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using SpiritMod.Dusts;

namespace SpiritMod.Projectiles.Clubs
{
	class NautilusClubProj : ClubProj
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nautilobber");
			Main.projFrames[Projectile.type] = 3;
		}
		public override void Smash(Vector2 position)
		{
            Player player = Main.player[Projectile.owner];
            for (int k = 0; k <= 110; k++)
            {
                Dust.NewDustPerfect(Projectile.oldPosition + new Vector2(Projectile.width / 2, Projectile.height / 2), DustType<Dusts.EarthDust>(), new Vector2(0, 1).RotatedByRandom(1) * Main.rand.NextFloat(-1, 1) * Projectile.ai[0] / 10f);
            }
            for (int k = 0; k <= 40; k++)
            {
                Dust.NewDustPerfect(Projectile.oldPosition + new Vector2(Projectile.width / 2, Projectile.height / 2), DustType<Dusts.CryoDust>(), new Vector2(0, 1).RotatedByRandom(1) * Main.rand.NextFloat(-1, 1) * Projectile.ai[0] / 10f);
            }
            Projectile.NewProjectile(Projectile.GetSource_FromAI("ClubSmash"), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<NautilusBubbleSpawner>(), Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner, 8, player.direction);
        }

        public override void SafeDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            int size = 82;
            if (Projectile.ai[0] >= ChargeTime)
            {

                Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Main.player[Projectile.owner].Center - Main.screenPosition, new Rectangle(0, size * 2, size, size), Color.White * 0.9f, TrueRotation, Origin, Projectile.scale, Effects, 1);
            }
        }
        public NautilusClubProj() : base(64, 21, 48, -1, 82, 6, 11, 1.9f, 17f){}
	}
}
