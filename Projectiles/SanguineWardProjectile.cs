using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using SpiritMod.Buffs.DoT;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	class SanguineWardProjectile : ModProjectile
	{
		private bool[] _npcAliveLast;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Sanguine Ward");

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 4;
			Projectile.timeLeft = 500;
			Projectile.height = 180;
			Projectile.width = 180;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			if (_npcAliveLast == null)
				_npcAliveLast = new bool[200];

			Player player = Main.player[Projectile.owner];
			Projectile.Center = new Vector2(player.Center.X + (player.direction > 0 ? 0 : 0), player.position.Y + 30);   // I dont know why I had to set it to -60 so that it would look right   (change to -40 to 40 so that it's on the floor)

			var list = Main.npc.Where(x => x.CanBeChasedBy() && x.Hitbox.Intersects(Projectile.Hitbox));

			foreach (var npc in list)
			{
				npc.AddBuff(ModContent.BuffType<BloodCorrupt>(), 20);

				if (_npcAliveLast[npc.whoAmI] && npc.life <= 0 && Main.rand.NextBool(4)) //if the npc was alive last frame and is now dead
				{
					int healNumber = Main.rand.Next(4, 7);
					player.HealEffect(healNumber);

					if (player.statLife <= player.statLifeMax - healNumber)
						player.statLife += healNumber;
					else
						player.statLife += player.statLifeMax - healNumber;
				}
			}

			for (int i = 0; i < 4; i++)
			{
				Vector2 vector2 = Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * new Vector2(Projectile.height, Projectile.height) * Projectile.scale * 1.45f / 2f;
				int index = Dust.NewDust(Projectile.Center + vector2, 0, 0, DustID.Blood, 0.0f, 0.0f, 0, default, 1f);
				Main.dust[index].position = Projectile.Center + vector2;
				Main.dust[index].velocity = Vector2.Zero;
				Main.dust[index].noGravity = true;
			}

			//set _npcAliveLast values
			for (int i = 0; i < Main.maxNPCs; i++)
				_npcAliveLast[i] = Main.npc[i].active && Main.npc[i].life > 0;
		}
	}
}