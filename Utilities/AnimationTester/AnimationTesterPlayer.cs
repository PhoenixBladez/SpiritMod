using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using System.Windows.Forms;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace SpiritMod.Utilities.AnimationTester
{
	internal class AnimationTesterPlayer : ModPlayer
	{
		internal Animation animation = null;

		internal bool AnimationLoaded => animation is not null;

		public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
		{
			if (animation.activation == AnimationActivation.OnHitNPC)
				SpawnAnimProjectile(item.GetSource_OnHit(target), target.Center);
		}

		private void SpawnAnimProjectile(IEntitySource source, Vector2 center)
		{
			int proj = Projectile.NewProjectile(source, center, Vector2.Zero, ModContent.ProjectileType<AnimationProjectile>(), 0, 0, Player.whoAmI);

			AnimationProjectile animProj = Main.projectile[proj].ModProjectile as AnimationProjectile;
			animProj.SetAnimation(animation);
		}
	}
}
