using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.Explosives.Thrown
{
	[ItemTag(ItemTags.Explosive)]
	public class HealingGrenade : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod) => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Throwing Regeneration Potion");
			Tooltip.SetDefault("'No brewery required!'");
		}

		public override void SetDefaults()
		{
			Item.damage = 45;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 14;
			Item.height = 26;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shoot = ModContent.ProjectileType<HealingGrenadeProj>();
			Item.knockBack = 4;
			Item.useTurn = false;
			Item.value = Item.sellPrice(0, 0, 1, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = false;
			Item.shootSpeed = 7.5f;
			Item.noUseGraphic = true;
			Item.consumable = true;
			Item.maxStack = 999;
		}

		//public override void AddRecipes()
		//{
		//	var recipe = new ModRecipe(mod);
		//	recipe.AddIngredient(ItemID.Grenade, 5);
		//	recipe.AddIngredient(ItemID.HealingPotion, 1);
		//	recipe.AddTile(TileID.Anvils);
		//	recipe.SetResult(this, 5);
		//	recipe.AddRecipe();
		//}
	}

	public class HealingGrenadeProj : ModProjectile
	{
		public override string Texture => Mod.Name + "/Items/Sets/Explosives/Thrown/HealingGrenade";

		public override void SetStaticDefaults() => DisplayName.SetDefault("Healing Grenade");

		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 26;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.timeLeft = 60 * 5;
			Projectile.penetrate = -1;
		}

		public override void AI()
		{
			Projectile.rotation += 0.06f * Projectile.velocity.X;
			Projectile.velocity.Y += 0.2f;

			if (Main.rand.NextBool(12))
				SpawnGore();
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 30; i++)
				SpawnGore(new Vector2(Main.rand.NextFloat(6, 8), 0).RotatedByRandom(MathHelper.TwoPi));

			Projectile.NewProjectile(Projectile.Center, Vector2.Zero, ModContent.ProjectileType<HealingSplash>(), 0, 0f);
		}

		public void SpawnGore(Vector2? overrideVel = null)
		{
			var pos = new Vector2(Projectile.position.X + Main.rand.Next(Projectile.width), Projectile.position.Y + Main.rand.Next(Projectile.height));
			var vel = new Vector2(Main.rand.Next(-10, 11) * 0.1f, Main.rand.Next(-20, -10) * 0.1f);
			Gore.NewGore(pos, overrideVel ?? vel, 331, Main.rand.Next(80, 120) * 0.01f);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Projectile.timeLeft > 4)
				Projectile.timeLeft = 4;
		}

		public override bool CanHitPlayer(Player target) => target.whoAmI == Projectile.owner;
	}

	public class HealingSplash : ModProjectile
	{
		public ref float Timer => ref Projectile.ai[0];

		public override string Texture => Mod.Name + "/Items/Sets/Explosives/Thrown/HealingGrenade";

		public override void SetStaticDefaults() => DisplayName.SetDefault("Healing Splash");

		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 26;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.timeLeft = 180;
			Projectile.penetrate = -1;
			Projectile.hide = true;
			Projectile.tileCollide = false;
		}

		public override void AI()
		{
			Projectile.rotation += 0.06f * Projectile.velocity.X;

			if (Main.rand.NextBool(6))
				SpawnGore();

			if (Timer++ % 60 == 0)
				HealAllPlayers();

			TileEffect();
		}

		private void TileEffect()
		{
			Point tileOrig = (Projectile.Center - new Vector2(4) * 16).ToTileCoordinates();

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
				if (p.active && !p.dead && p.DistanceSQ(Projectile.Center) < 180 * 180 && p.statLife < p.statLifeMax2)
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
			var pos = new Vector2(Projectile.position.X + Main.rand.Next(Projectile.width), Projectile.position.Y + Main.rand.Next(Projectile.height));
			var vel = new Vector2(Main.rand.Next(-10, 11) * 0.1f, Main.rand.Next(-20, -10) * 0.1f);
			Gore.NewGore(pos, overrideVel ?? vel, 331, Main.rand.Next(80, 120) * 0.01f);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Projectile.timeLeft > 4)
				Projectile.timeLeft = 4;
		}

		public override bool CanHitPlayer(Player target) => target.whoAmI == Projectile.owner;
	}
}
