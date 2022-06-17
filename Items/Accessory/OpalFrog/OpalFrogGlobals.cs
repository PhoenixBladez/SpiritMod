using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.OpalFrog
{
	public class OpalFrogProjectile : GlobalProjectile
	{
		private Player GetPlayer(Projectile projectile) => Main.player[projectile.owner];
		private OpalFrogPlayer GetOpalFrogPlayer(Projectile projectile) => GetPlayer(projectile).GetModPlayer<OpalFrogPlayer>();

		//Increase pull speed based on hook stats
		public override void GrapplePullSpeed(Projectile projectile, Player player, ref float speed)
		{
			speed *= GetOpalFrogPlayer(projectile).HookStat;

			//Bat hook in 1.3 in particular is way too strong of a synergy, so temporary hardcoded nerf until 1.4
			if (projectile.type == ProjectileID.BatHook && GetOpalFrogPlayer(projectile).AutoUnhook)
				speed *= 0.8f;
		}

		public override void GrappleRetreatSpeed(Projectile projectile, Player player, ref float speed) => speed *= GetOpalFrogPlayer(projectile).HookStat;

		public override bool PreAI(Projectile projectile)
		{
			var hitBox = projectile.Hitbox;
			hitBox.Inflate(hitBox.Width / 2, hitBox.Height / 2); //enlarge hitbox, as otherwise no intersection would happen when hooked into a tile

			//If the projectile has the hook aistyle, and intersects the owner's hitbox, and has the ai[0] value corresponding to being stuck in place, kill before stopping the player
			if(projectile.aiStyle == 7 && projectile.ai[0] == 2 && hitBox.Intersects(GetPlayer(projectile).Hitbox) && GetOpalFrogPlayer(projectile).AutoUnhook)
			{
				projectile.Kill();
				return false;
			}
			return true;
		}
	}

	public class OpalFrogGItem : GlobalItem
	{
		//Simplest way I could find to increase hook shootspeed based on player hookstat, overriding shoot doesn't seem to work (hooks don't use shoot hook in the first place?)
		public override void UpdateInventory(Item item, Player player) => UpdateItem(item, player);

		public static void UpdateItem(Item item, Player player)
		{
			OpalFrogPlayer modPlayer = player.GetModPlayer<OpalFrogPlayer>();

			//Create an instance of the projectile to check its aistyle
			Projectile shootInstance = new Projectile();
			shootInstance.SetDefaults(item.shoot);

			//Create an instance of the item to find the base shootspeed
			Item baseItemInstance = new Item();
			baseItemInstance.SetDefaults(item.type);

			//If the instance of the item's shoot projectile has hook aistyle, increase shootspeed
			if (shootInstance.aiStyle == 7)
			{
				//Due to update order, checks the last tick's hook stat value if the current hook stat value is the default
				item.shootSpeed = baseItemInstance.shootSpeed * Math.Max(modPlayer.HookStat, modPlayer.LastHookStat);
			}
		}
	}
}