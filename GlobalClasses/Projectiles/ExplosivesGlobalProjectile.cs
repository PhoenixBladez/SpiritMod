using SpiritMod.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.GlobalClasses.Projectiles
{
	public class ExplosivesCache
	{
		public static int[] Explosives
		{
			get => new int[] { ProjectileID.Bomb, ProjectileID.BombFish, ProjectileID.BouncyBomb, ProjectileID.Grenade, ProjectileID.BouncyGrenade,
				ProjectileID.StickyGrenade, ProjectileID.Dynamite, ProjectileID.BouncyDynamite, ProjectileID.StickyDynamite };
		}

		public static List<int> LoadedExplosives = null;
		public static List<int> AllExplosives = null;

		public static void Initialize(Mod mod)
		{
			LoadedExplosives = new List<int>();

			var types = typeof(ExplosivesCache).Assembly.GetTypes();
			foreach (var type in types)
			{
				if (typeof(ModItem).IsAssignableFrom(type))
				{
					var tag = (ItemTagAttribute)Attribute.GetCustomAttribute(type, typeof(ItemTagAttribute));

					if (tag == null || tag.Tags.Length == 0)
						continue;

					if (tag.Tags.Contains(ItemTags.Explosive) && !tag.Tags.Contains(ItemTags.Unloaded))
					{
						var item = new Item();
						item.SetDefaults(mod.Find<ModItem>(type.Name).Type);
						LoadedExplosives.Add(item.shoot);
					}
				}
			}

			AllExplosives = new List<int>();
			AllExplosives.AddRange(Explosives);
			AllExplosives.AddRange(LoadedExplosives);
		}
	}

	public class ExplosivesGlobalProjectile : GlobalProjectile
	{
		public override bool InstancePerEntity => true;

		private bool _fused = false;

		public override bool PreAI(Projectile projectile)
		{
			if (projectile.owner > -1 && projectile.friendly && _fused)
			{
				_fused = true;

				MyPlayer modPlayer = Main.player[projectile.owner].GetSpiritPlayer();

				if (modPlayer.longFuse && ExplosivesCache.AllExplosives.Contains(projectile.type)) //Long fuse functionality
					projectile.timeLeft = (int)(projectile.timeLeft * 1.5f); //Makes it last 150% longer
			}
			return true;
		}
	}
}
