using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SpiritMod.Buffs.DoT;

namespace SpiritMod.Projectiles.Clubs
{
	class ToxinField : ModProjectile
	{
		private bool[] _npcAliveLast;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Toxin Field");

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 4;
			Projectile.timeLeft = 90;
			Projectile.height = 130;
			Projectile.width = 110;
			Projectile.alpha = 255;
		}

		public override void AI()
		{
			if (_npcAliveLast == null)
				_npcAliveLast = new bool[200];

			Player player = Main.player[Projectile.owner];

			var list = Main.npc.Where(x => x.CanBeChasedBy() && x.Hitbox.Intersects(Projectile.Hitbox));
			foreach (var npc in list)
				npc.AddBuff(ModContent.BuffType<AcidBurn>(), 360);

			if (Main.rand.NextBool(3))
			{
				for (int num621 = 0; num621 < 9; num621++)
				{
					Dust dust = Main.dust[Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + 2f), Projectile.width, Projectile.height, ModContent.DustType<Dusts.PoisonGas>(), Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, new Color(), 5f)];
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
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
					DustID.Grass, 0f, 0f, 100, default, .7f);
				Dust dust = Main.dust[Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + 2f), Projectile.width, Projectile.height, ModContent.DustType<Dusts.PoisonGas>(), Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, new Color(), 8f)];
				dust.noGravity = true;
				dust.velocity.X = dust.velocity.X * 0.3f;
				dust.velocity.Y = (dust.velocity.Y * 0.2f) - 1;
			}
		}
	}
}
