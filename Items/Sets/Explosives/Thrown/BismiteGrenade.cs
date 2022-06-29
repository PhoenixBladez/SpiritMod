using Microsoft.Xna.Framework;
using SpiritMod.Buffs.DoT;
using SpiritMod.Items.Sets.BismiteSet;
using SpiritMod.NPCs.DiseasedSlime;
using Terraria;
using Terraria.Audio;
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
			Item.damage = 45;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Throwing;
			Item.width = 18;
			Item.height = 20;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shoot = ModContent.ProjectileType<BismiteGrenadeProj>();
			Item.knockBack = 4;
			Item.useTurn = false;
			Item.value = Item.sellPrice(0, 0, 1, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
			Item.shootSpeed = 7.5f;
			Item.noUseGraphic = true;
			Item.consumable = true;
			Item.maxStack = 999;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe(5);
			recipe.AddIngredient(ItemID.Grenade, 5);
			recipe.AddIngredient(ModContent.ItemType<BismiteCrystal>(), 1);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}

	public class BismiteGrenadeProj : ModProjectile
	{
		public bool Exploded { get => Projectile.ai[0] != 0; set => Projectile.ai[0] = !value ? 0 : 1; }

		public override string Texture => Mod.Name + "/Items/Sets/Explosives/Thrown/BismiteGrenade";

		public override void SetStaticDefaults() => DisplayName.SetDefault("Bismite Grenade");

		public override void SetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 20;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.timeLeft = 180;
			Projectile.penetrate = -1;
		}

		public override void AI()
		{
			Projectile.rotation += 0.06f * Projectile.velocity.X;
			Projectile.velocity.Y += 0.2f;

			if (Projectile.timeLeft < 4 && !Exploded)
			{
				const int ExplosionSize = 160;

				Projectile.position -= new Vector2(ExplosionSize / 2f) - Projectile.Size / 2f;
				Projectile.width = Projectile.height = ExplosionSize;
				Projectile.hostile = true;
				Projectile.friendly = true;
				Projectile.hide = true;

				Exploded = true;

				Vector2 off = new Vector2(30);
				Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center.X - off.X, Projectile.Center.Y - off.Y, 0f, 0f, ModContent.ProjectileType<NoxiousGas>(), 0, 1, Main.myPlayer, 0, 0);
				Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<NoxiousIndicator>(), 0, 1, Main.myPlayer, 0, 0);

				SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/GasHiss"), Projectile.position);
				SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X)
				Projectile.velocity.X = -oldVelocity.X;

			if (Projectile.velocity.Y != oldVelocity.Y)
				Projectile.velocity.Y = -oldVelocity.Y * 0.25f;

			Projectile.velocity.X *= 0.96f;
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<FesteringWounds>(), 600);

			if (Projectile.timeLeft > 4)
				Projectile.timeLeft = 4;
		}

		public override bool CanHitPlayer(Player target) => target.whoAmI == Projectile.owner;
		public override bool? CanHitNPC(NPC target) => !target.friendly;
	}
}
