using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.Explosives.Thrown
{
	[ItemTag(ItemTags.Explosive)]
	public class HealingGrenade : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Throwing Regeneration Potion");
			Tooltip.SetDefault("'No brewery required!'");
		}

		public override void SetDefaults()
		{
			item.damage = 45;
			item.noMelee = true;
			item.ranged = true;
			item.width = 14;
			item.height = 26;
			item.useTime = 25;
			item.useAnimation = 25;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.shoot = ModContent.ProjectileType<HealingGrenadeProj>();
			item.knockBack = 4;
			item.useTurn = false;
			item.value = Item.sellPrice(0, 0, 1, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item5;
			item.autoReuse = false;
			item.shootSpeed = 7.5f;
			item.noUseGraphic = true;
			item.consumable = true;
			item.maxStack = 999;
		}

		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Grenade, 5);
			recipe.AddIngredient(ItemID.HealingPotion, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 5);
			recipe.AddRecipe();
		}
	}

	public class HealingGrenadeProj : ModProjectile
	{
		public override string Texture => mod.Name + "/Items/Sets/Explosives/Thrown/HealingGrenade";

		public override void SetStaticDefaults() => DisplayName.SetDefault("Healing Grenade");

		public override void SetDefaults()
		{
			projectile.width = 14;
			projectile.height = 26;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.timeLeft = 60 * 5;
			projectile.penetrate = -1;
		}

		public override void AI()
		{
			projectile.rotation += 0.06f * projectile.velocity.X;
			projectile.velocity.Y += 0.2f;

			if (Main.rand.NextBool(12))
				SpawnGore();
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 30; i++)
				SpawnGore(new Vector2(Main.rand.NextFloat(6, 8), 0).RotatedByRandom(MathHelper.TwoPi));

			Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<HealingSplash>(), 0, 0f);
		}

		public void SpawnGore(Vector2? overrideVel = null)
		{
			var pos = new Vector2(projectile.position.X + Main.rand.Next(projectile.width), projectile.position.Y + Main.rand.Next(projectile.height));
			var vel = new Vector2(Main.rand.Next(-10, 11) * 0.1f, Main.rand.Next(-20, -10) * 0.1f);
			Gore.NewGore(pos, overrideVel ?? vel, 331, Main.rand.Next(80, 120) * 0.01f);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (projectile.timeLeft > 4)
				projectile.timeLeft = 4;
		}

		public override bool CanHitPlayer(Player target) => target.whoAmI == projectile.owner;
	}

	public class HealingSplash : ModProjectile
	{
		public ref float Timer => ref projectile.ai[0];

		public override string Texture => mod.Name + "/Items/Sets/Explosives/Thrown/HealingGrenade";

		public override void SetStaticDefaults() => DisplayName.SetDefault("Healing Splash");

		public override void SetDefaults()
		{
			projectile.width = 14;
			projectile.height = 26;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.timeLeft = 180;
			projectile.penetrate = -1;
			projectile.hide = true;
			projectile.tileCollide = false;
		}

		public override void AI()
		{
			projectile.rotation += 0.06f * projectile.velocity.X;

			if (Main.rand.NextBool(6))
				SpawnGore();

			if (Timer++ % 60 == 0)
				HealAllPlayers();

			TileEffect();
		}

		private void TileEffect()
		{
			Point tileOrig = (projectile.Center - new Vector2(4) * 16).ToTileCoordinates();

			for (int i = 0; i < 8; ++i)
			{
				for (int j = 0; j < 8; ++j)
				{
					var p = new Point(tileOrig.X + i, tileOrig.Y + j);
					Tile t = Framing.GetTileSafely(p.X, p.Y);

					if (WorldGen.SolidTile(t))
						FloorParticle(p.ToWorldCoordinates(8, 0));
				}
			}
		}

		private void FloorParticle(Vector2 pos)
		{
			Vector2 vel = new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-2.5f, 0));
			Dust d = Dust.NewDustPerfect(pos, DustID.Firework_Red, vel, 150, default, Main.rand.NextFloat(0.5f, 0.7f));
			d.noGravity = true;
			d.fadeIn = 0.7f;
		}

		private void HealAllPlayers()
		{
			for (int i = 0; i < Main.maxPlayers; ++i)
			{
				Player p = Main.player[i];
				if (p.active && !p.dead && p.DistanceSQ(projectile.Center) < 180 * 180 && p.statLife < p.statLifeMax2)
				{
					if (p.statLife < p.statLifeMax2 - 5)
					{
						p.statLife += 5;
						p.HealEffect(5);
					}
					else
					{
						p.HealEffect(p.statLifeMax2 - p.statLife);
						p.statLife = p.statLifeMax2;
					}
				}
			}
		}

		public void SpawnGore(Vector2? overrideVel = null)
		{
			var pos = new Vector2(projectile.position.X + Main.rand.Next(projectile.width), projectile.position.Y + Main.rand.Next(projectile.height));
			var vel = new Vector2(Main.rand.Next(-10, 11) * 0.1f, Main.rand.Next(-20, -10) * 0.1f);
			Gore.NewGore(pos, overrideVel ?? vel, 331, Main.rand.Next(80, 120) * 0.01f);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (projectile.timeLeft > 4)
				projectile.timeLeft = 4;
		}

		public override bool CanHitPlayer(Player target) => target.whoAmI == projectile.owner;
	}
}
