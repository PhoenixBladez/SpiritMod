using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Held
{
	public class ZephyrSpearProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Breath of the Zephyr");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Trident);
			AIType = ProjectileID.Trident;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			damage /= 3;
			Player owner = Main.player[Projectile.owner];
			int manaleech = Math.Min(Main.rand.Next(2, 5), owner.statManaMax - owner.statMana);
			if (manaleech > 0) {
				owner.ManaEffect(manaleech);
				owner.statMana += manaleech;
				SoundEngine.PlaySound(SoundID.Item15 with { Volume = 0.3f, PitchVariance = 0.1f }, owner.Center);
				if (Main.netMode == NetmodeID.MultiplayerClient && owner.whoAmI == Main.myPlayer) {
					NetMessage.SendData(MessageID.PlayerMana, -1, -1, null, owner.whoAmI, owner.statMana, owner.statManaMax);
					NetMessage.SendData(MessageID.ManaEffect, -1, -1, null, owner.whoAmI, manaleech);
				}
			}
		}
	}
}
