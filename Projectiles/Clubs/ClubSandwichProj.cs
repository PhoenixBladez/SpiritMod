using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using SpiritMod.Dusts;
using SpiritMod.Items;

namespace SpiritMod.Projectiles.Clubs
{
	class ClubSandwichProj : ClubProj
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Club Sandwich");
			Main.projFrames[projectile.type] = 2;
		}
		public override void Smash(Vector2 position)
		{
			Player player = Main.player[projectile.owner];
			for (int k = 0; k <= 50; k++) {
				Dust.NewDustPerfect(projectile.oldPosition + new Vector2(projectile.width / 2, projectile.height / 2), DustType<Dusts.EarthDust>(), new Vector2(0, 1).RotatedByRandom(1) * Main.rand.NextFloat(-1, 1) * projectile.ai[0] / 10f);
			}
			for (int i = 0; i < 5; i++) {
				int type = 0;
				switch (Main.rand.Next(4)) {
					case 0:
						type = ModContent.ItemType<ClubMealOne>();
						break;
					case 1:
						type = ModContent.ItemType<ClubMealTwo>();
						break;
					case 2:
						type = ModContent.ItemType<ClubMealThree>();
						break;
					case 3:
						type = ModContent.ItemType<ClubMealFour>();
						break;
				}
				int item = Item.NewItem(projectile.position, projectile.Size, type);
				Main.item[item].velocity = Vector2.UnitY.RotatedBy(3.14f + Main.rand.NextFloat(3.14f)) * 8f * player.direction;
			}
			Main.PlaySound(2, projectile.Center, 20);
		}
		public ClubSandwichProj() : base(52, 17, 34, -1, 58, 5, 9, 1.7f, 12f){}
	}

}
