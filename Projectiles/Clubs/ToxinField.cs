using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Clubs
{
	class ToxinField : ModProjectile
	{
		private bool[] _npcAliveLast;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Toxin Field");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 4;
			projectile.timeLeft = 90;
			projectile.height = 130;
			projectile.width = 110;
			projectile.alpha = 255;
		}

		public override void AI()
		{
			if (_npcAliveLast == null)
				_npcAliveLast = new bool[200];

			Player player = Main.player[projectile.owner];
			
			var list = Main.npc.Where(x => x.Hitbox.Intersects(projectile.Hitbox));
			foreach (var npc in list) {
				if (!npc.friendly) {
					npc.AddBuff(ModContent.BuffType<AcidBurn>(), 360);
				}
			}
			if (Main.rand.NextBool(3))
            {
                for (int num621 = 0; num621 < 9; num621++)
                {
                    Dust dust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 2f), projectile.width, projectile.height, ModContent.DustType<Dusts.PoisonGas>(), projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100, new Color(), 5f)];
                    dust.noGravity = true;
                    dust.velocity.X = dust.velocity.X * 0.3f;
                    dust.velocity.Y = (dust.velocity.Y * 0.2f) - 1;
                }
            }
		}
        public override void Kill(int timeLeft)
        {
            for (int num621 = 0; num621 < 16; num621++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height,
                    2, 0f, 0f, 100, default(Color), .7f);
                Dust dust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 2f), projectile.width, projectile.height, ModContent.DustType<Dusts.PoisonGas>(), projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100, new Color(), 8f)];
                dust.noGravity = true;
                dust.velocity.X = dust.velocity.X * 0.3f;
                dust.velocity.Y = (dust.velocity.Y * 0.2f) - 1;
            }
        }
    }
}
