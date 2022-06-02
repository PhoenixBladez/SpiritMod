using Microsoft.Xna.Framework;
using SpiritMod.Buffs.DoT;
using SpiritMod.Items.Sets.BismiteSet;
using SpiritMod.NPCs.DiseasedSlime;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.Explosives.Thrown
{
	[ItemTag(ItemTags.Explosive)]
	public class BismiteGrenade : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bismite Grenade");
			Tooltip.SetDefault("Explodes into a cloud of poison\nPoison lasts for 10 seconds");
		}

		public override void SetDefaults()
		{
			item.damage = 45;
			item.noMelee = true;
			item.thrown = true;
			item.width = 18;
			item.height = 20;
			item.useTime = 25;
			item.useAnimation = 25;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.shoot = ModContent.ProjectileType<BismiteGrenadeProj>();
			item.knockBack = 4;
			item.useTurn = false;
			item.value = Item.sellPrice(0, 0, 1, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item1;
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
			recipe.AddIngredient(ModContent.ItemType<BismiteCrystal>(), 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 5);
			recipe.AddRecipe();
		}
	}

	public class BismiteGrenadeProj : ModProjectile
	{
		public bool Exploded { get => projectile.ai[0] != 0; set => projectile.ai[0] = !value ? 0 : 1; }

		public override string Texture => mod.Name + "/Items/Sets/Explosives/Thrown/BismiteGrenade";

		public override void SetStaticDefaults() => DisplayName.SetDefault("Bismite Grenade");

		public override void SetDefaults()
		{
			projectile.width = 18;
			projectile.height = 20;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.timeLeft = 180;
			projectile.penetrate = -1;
		}

		public override void AI()
		{
			projectile.rotation += 0.06f * projectile.velocity.X;
			projectile.velocity.Y += 0.2f;

			if (projectile.timeLeft < 4 && !Exploded)
			{
				const int ExplosionSize = 160;

				projectile.position -= new Vector2(ExplosionSize / 2f) - projectile.Size / 2f;
				projectile.width = projectile.height = ExplosionSize;
				projectile.hostile = true;
				projectile.friendly = true;
				projectile.hide = true;

				Exploded = true;

				Vector2 off = new Vector2(30);
				Projectile.NewProjectile(projectile.Center.X - off.X, projectile.Center.Y - off.Y, 0f, 0f, ModContent.ProjectileType<NoxiousGas>(), 0, 1, Main.myPlayer, 0, 0);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<NoxiousIndicator>(), 0, 1, Main.myPlayer, 0, 0);

				Main.PlaySound(SoundLoader.customSoundType, projectile.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/GasHiss"));
				Main.PlaySound(SoundID.Item14, projectile.position);
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.velocity.X != oldVelocity.X)
				projectile.velocity.X = -oldVelocity.X;

			if (projectile.velocity.Y != oldVelocity.Y)
				projectile.velocity.Y = -oldVelocity.Y * 0.25f;

			projectile.velocity.X *= 0.96f;
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<FesteringWounds>(), 600);

			if (projectile.timeLeft > 4)
				projectile.timeLeft = 4;
		}

		public override bool CanHitPlayer(Player target) => target.whoAmI == projectile.owner;
		public override bool? CanHitNPC(NPC target) => !target.friendly;
	}
}
