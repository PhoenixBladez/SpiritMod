using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using SpiritMod.NPCs.Boss.MoonWizard.Projectiles;

namespace SpiritMod.Projectiles.Bullet
{
	public class MoonshotProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moonshot");
		}

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.penetrate = 1;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
			Projectile.timeLeft = 999999;
		}

		Vector2 direction = Vector2.Zero;
        int counter = 0;
        Vector2 holdOffset = new Vector2(0, -15);

        public override bool PreAI()
		{
            Player player = Main.player[Projectile.owner];
            if (player.channel && player.HasAmmo(player.inventory[player.selectedItem]))
            {
				if (counter % 10 == 0 )
                {
                    direction = Main.MouseWorld - (player.Center - new Vector2(4, 4));
                    direction.Normalize();
                    direction *= 5f;
                }

				if (counter == 45 || counter == 140)
				{
					SoundEngine.PlaySound(SoundID.Item5, Projectile.position);
					DustHelper.DrawDustImage(player.Center, 226, counter == 45 ? 0.11f : 0.18f, "SpiritMod/Effects/DustImages/MoonSigil", 1f);
				}

                player.itemTime = 5;
                player.itemAnimation = 5;
                Projectile.position = player.Center + holdOffset;
                player.velocity.X *= 0.97f;
                counter++;
            }
            else
            {
				if (player.HasAmmo(player.inventory[player.selectedItem]))
				{
					bool canshoot = player.PickAmmo(player.inventory[player.selectedItem], out int shoot, out float speed, out Projectile.damage, out Projectile.knockBack, out int _);
					if (counter < 45)
					{
						SoundEngine.PlaySound(SoundID.Item96, Projectile.Center);
						direction /= 2f;
						int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), player.Center - new Vector2(4, 4), direction * speed, shoot, 12, Projectile.knockBack / 4, 0, Projectile.owner);
						Main.projectile[proj].GetGlobalProjectile<SpiritGlobalProjectile>().shotFromMaliwanShockCommon = true;
					}
					else if (counter >= 45 && counter < 140)
					{
						SoundEngine.PlaySound(SoundID.Item109);
						direction *= 2.25f;
						Projectile.NewProjectile(Projectile.GetSource_FromAI(), player.Center - new Vector2(4, 4), direction, ModContent.ProjectileType<MoonshotBulletLarge>(), (int)(Projectile.damage/4 * 3), 5, Projectile.owner);
					}
					else if (counter > 140)
					{
						direction *= .8f;
						SoundEngine.PlaySound(SoundID.Item109);
						int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), player.Center - new Vector2(4, 4), direction, ModContent.ProjectileType<SineBall>(), Projectile.damage/3, Projectile.knockBack * 0.25f, Projectile.owner, 180);
						int p1 = Projectile.NewProjectile(Projectile.GetSource_FromAI(), player.Center - new Vector2(4, 4), direction, ModContent.ProjectileType<SineBall>(), Projectile.damage/3, Projectile.knockBack * 0.25f, Projectile.owner, 0, proj + 1);
						Main.projectile[proj].hostile = false;
						Main.projectile[p1].hostile = false;
						Main.projectile[proj].friendly = true;
						Main.projectile[p1].friendly = true;
						Main.projectile[proj].scale *= .65f;
						Main.projectile[p1].scale *= .65f;
						Main.projectile[proj].timeLeft = 120;
						Main.projectile[p1].timeLeft = 120;
					}
				}
                Projectile.active = false;
            }
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            return true;
		}
    }
}
