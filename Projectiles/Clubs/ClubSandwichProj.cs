using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using SpiritMod.Items.Sets.ClubSubclass.ClubSandwich;
using static Terraria.ModLoader.ModContent;

namespace SpiritMod.Projectiles.Clubs
{
	class ClubSandwichProj : ClubProj
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Club Sandwich");
			Main.projFrames[Projectile.type] = 2;
		}

		public override void Smash(Vector2 position)
		{
			Player player = Main.player[Projectile.owner];
			for (int k = 0; k <= 50; k++)
				Dust.NewDustPerfect(Projectile.oldPosition + new Vector2(Projectile.width / 2, Projectile.height / 2), DustType<Dusts.EarthDust>(), new Vector2(0, 1).RotatedByRandom(1) * Main.rand.NextFloat(-1, 1) * Projectile.ai[0] / 10f);

			for (int i = 0; i < 5; i++)
			{
				int type = 0;
				switch (Main.rand.Next(4))
				{
					case 0:
						type = ItemType<ClubMealOne>();
						break;
					case 1:
						type = ItemType<ClubMealTwo>();
						break;
					case 2:
						type = ItemType<ClubMealThree>();
						break;
					case 3:
						type = ItemType<ClubMealFour>();
						break;
				}
				int item = Item.NewItem(Projectile.GetSource_FromAI("ClubSmash"), Projectile.position, Projectile.Size, type);
				Main.item[item].velocity = Vector2.UnitY.RotatedBy(3.14f + Main.rand.NextFloat(3.14f)) * 8f * player.direction;
			}
			SoundEngine.PlaySound(SoundID.NPCHit20, Projectile.position);
		}

		public ClubSandwichProj() : base(52, 17, 34, -1, 58, 5, 9, 1.7f, 12f) { }
	}
}