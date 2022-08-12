using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Sets.BriarDrops
{
	public class SanctifiedStabber : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sanctified Stabber");
			Tooltip.SetDefault("Inflicts 'Withering Leaf'");
		}

		public override void SetDefaults()
		{
			Item.damage = 11;
			Item.knockBack = 3f;
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.useAnimation = 4;
			Item.useTime = 10;
			Item.width = 24;
			Item.height = 28;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.MeleeNoSpeed;
			Item.autoReuse = true;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 3, 0);
			Item.shoot = ModContent.ProjectileType<StabberShortsword>();
			Item.shootSpeed = 2.1f;
			Item.useTurn = true;
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool(2))
				target.AddBuff(ModContent.BuffType<WitheringLeaf>(), 180);
		}
	}

	public class StabberShortsword : ModProjectile
	{
		public override string Texture => "SpiritMod/Items/Sets/BriarDrops/SanctifiedStabber";

		public const int FadeInDuration = 7;
		public const int FadeOutDuration = 4;
		public const int TotalDuration = 16;

		public float CollisionWidth => 10f * Projectile.scale;

		public int Timer
		{
			get => (int)Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		public override void SetStaticDefaults() => DisplayName.SetDefault("Example Shortsword");

		public override void SetDefaults()
		{
			Projectile.Size = new Vector2(18);
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.scale = 1f;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.ownerHitCheck = true;
			Projectile.extraUpdates = 1;
			Projectile.timeLeft = 360;
			Projectile.hide = true;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			Timer += 1;
			if (Timer >= TotalDuration)
			{
				Projectile.Kill();
				return;
			}
			else
				player.heldProj = Projectile.whoAmI;

			Projectile.Opacity = Utils.GetLerpValue(0f, FadeInDuration, Timer, clamped: true) * Utils.GetLerpValue(TotalDuration, TotalDuration - FadeOutDuration, Timer, clamped: true);

			Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: false, addGfxOffY: false);
			Projectile.Center = playerCenter + Projectile.velocity * (Timer - 1f);

			Projectile.spriteDirection = (Vector2.Dot(Projectile.velocity, Vector2.UnitX) >= 0f).ToDirectionInt();
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 - MathHelper.PiOver4 * Projectile.spriteDirection;

			SetVisualOffsets();
		}

		private void SetVisualOffsets()
		{
			const int HalfSpriteWidth = 32 / 2;
			const int HalfSpriteHeight = 32 / 2;

			int HalfProjWidth = Projectile.width / 2;
			int HalfProjHeight = Projectile.height / 2;

			DrawOriginOffsetX = 0;
			DrawOffsetX = -(HalfSpriteWidth - HalfProjWidth);
			DrawOriginOffsetY = -(HalfSpriteHeight - HalfProjHeight);
		}

		public override bool ShouldUpdatePosition() => false;

		public override void CutTiles() //Cuts tiles cleanly
		{
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Vector2 start = Projectile.Center;
			Vector2 end = start + Projectile.velocity.SafeNormalize(-Vector2.UnitY) * 10f;
			Utils.PlotTileLine(start, end, CollisionWidth, DelegateMethods.CutTiles);
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Vector2 start = Projectile.Center;
			Vector2 end = start + Projectile.velocity * 6f;
			float collisionPoint = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, CollisionWidth, ref collisionPoint);
		}
	}
}