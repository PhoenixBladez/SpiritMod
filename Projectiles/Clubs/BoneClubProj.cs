using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using static Terraria.ModLoader.ModContent;
using SpiritMod.Dusts;

namespace SpiritMod.Projectiles.Clubs
{
	class BoneClubProj : ClubProj
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bone Club");
			Main.projFrames[Projectile.type] = 3;
		}

		public override void Smash(Vector2 position)
		{
			for (int k = 0; k <= 100; k++)
				Dust.NewDustPerfect(Projectile.oldPosition + (Projectile.Size / 2f), DustType<BoneDust>(), new Vector2(0, 1).RotatedByRandom(1) * Main.rand.NextFloat(-1, 1) * Projectile.ai[0] / 10f);

            for (int i = 0; i < 6; i++)
            {
                float rotation = (float)(Main.rand.Next(180, 361) * (Math.PI / 180));
                float rotation2 = (float)(Main.rand.Next(180, 270) * (Math.PI / 180));
                Vector2 velocity = new Vector2((float)Math.Cos(rotation2), (float)Math.Sin(rotation));
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI("ClubSmash"), Projectile.Center - new Vector2(0, 32), velocity * new Vector2(-4 * Main.player[Projectile.owner].direction, 4), ProjectileType<BoneShard>(), (int)(Projectile.damage / 4f), Projectile.knockBack, Projectile.owner);
                Main.projectile[proj].scale *= Main.rand.NextFloat(.6f, 1f);
            }
        }

		public override void SafeDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			const int Size = 62;

			if (Projectile.ai[0] >= ChargeTime)
				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Main.player[Projectile.owner].Center - Main.screenPosition, new Rectangle(0, Size * 2, Size, Size), Color.White * 0.9f, TrueRotation, Origin, Projectile.scale, Effects, 1);
		}
		public BoneClubProj() : base(50, 20, 80, -1, 62, 5, 9, 1.7f, 12f){}
	}
}
